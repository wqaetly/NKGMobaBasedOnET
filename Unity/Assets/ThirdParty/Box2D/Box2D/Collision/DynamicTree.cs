using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
using Box2DSharp.Dynamics.Internal;

namespace Box2DSharp.Collision
{
    public class DynamicTree
    {
        public const int NullNode = -1;

        private int _freeList;

        private int _nodeCapacity;

        private int _nodeCount;

        private int _root;

        private TreeNode[] _treeNodes;

        public DynamicTree()
        {
            _root = NullNode;

            _nodeCapacity = 16;
            _nodeCount = 0;
            _treeNodes = ArrayPool<TreeNode>.Shared.Rent(_nodeCapacity);

            // Build a linked list for the free list.
            // 节点数组初始化
            for (var i = 0; i < _nodeCapacity; ++i)
            {
                _treeNodes[i] = new TreeNode {Next = i + 1, Height = -1};
            }

            // 最后一个节点Next为null
            _treeNodes[_nodeCapacity - 1].Next = NullNode;
            _treeNodes[_nodeCapacity - 1].Height = -1;

            _freeList = 0;
        }

        private int AllocateNode()
        {
            // Expand the node pool as needed.
            if (_freeList == NullNode)
            {
                Debug.Assert(_nodeCount == _nodeCapacity);

                // The free list is empty. Rebuild a bigger pool.
                // 剩余节点为0,增加可用节点
                var oldNodes = _treeNodes;
                _nodeCapacity *= 2;

                _treeNodes = ArrayPool<TreeNode>.Shared.Rent(_nodeCapacity);
                Array.Copy(oldNodes, _treeNodes, _nodeCount);
                Array.Clear(oldNodes, 0, _nodeCount);
                ArrayPool<TreeNode>.Shared.Return(oldNodes);

                // Build a linked list for the free list. The parent
                // pointer becomes the "next" pointer.
                for (var i = _nodeCount; i < _nodeCapacity; ++i)
                {
                    _treeNodes[i] = new TreeNode {Next = i + 1, Height = -1};
                }

                _treeNodes[_nodeCapacity - 1].Next = NullNode;
                _treeNodes[_nodeCapacity - 1].Height = -1;
                _freeList = _nodeCount;
            }

            // Peel a node off the free list.
            var nodeId = _freeList;
            _freeList = _treeNodes[nodeId].Next;
            ref var newNode = ref _treeNodes[nodeId];
            newNode.Parent = NullNode;
            newNode.Child1 = NullNode;
            newNode.Child2 = NullNode;
            newNode.Height = 0;
            newNode.UserData = null;
            newNode.Moved = false;
            ++_nodeCount;
            return nodeId;
        }

        private void FreeNode(int nodeId)
        {
            Debug.Assert(0 <= nodeId && nodeId < _nodeCapacity);
            Debug.Assert(0 < _nodeCount);
            ref var freeNode = ref _treeNodes[nodeId];
            freeNode.Reset();
            freeNode.Next = _freeList;
            freeNode.Height = -1;
            _freeList = nodeId;
            --_nodeCount;
        }

        /// Create a proxy. Provide a tight fitting AABB and a userData pointer.
        public int CreateProxy(in AABB aabb, object userData)
        {
            var proxyId = AllocateNode();
            ref var proxyNode = ref _treeNodes[proxyId];

            // Fatten the aabb.
            var r = new Vector2(Settings.AABBExtension, Settings.AABBExtension);
            proxyNode.AABB.LowerBound = aabb.LowerBound - r;
            proxyNode.AABB.UpperBound = aabb.UpperBound + r;
            proxyNode.UserData = userData;
            proxyNode.Height = 0;
            proxyNode.Moved = true;
            InsertLeaf(proxyId);

            return proxyId;
        }

        /// Destroy a proxy. This asserts if the id is invalid.
        public void DestroyProxy(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < _nodeCapacity);
            Debug.Assert(_treeNodes[proxyId].IsLeaf());

            RemoveLeaf(proxyId);
            FreeNode(proxyId);
        }

        /// Move a proxy with a swepted AABB. If the proxy has moved outside of its fattened AABB,
        /// then the proxy is removed from the tree and re-inserted. Otherwise
        /// the function returns immediately.
        /// @return true if the proxy was re-inserted.
        public bool MoveProxy(int proxyId, in AABB aabb, in Vector2 displacement)
        {
            Debug.Assert(0 <= proxyId && proxyId < _nodeCapacity);

            Debug.Assert(_treeNodes[proxyId].IsLeaf());

            // Extend AABB
            var r = new Vector2(Settings.AABBExtension, Settings.AABBExtension);
            var fatAABB = new AABB
            {
                LowerBound = aabb.LowerBound - r,
                UpperBound = aabb.UpperBound + r
            };

            // Predict AABB movement
            var d = Settings.AABBMultiplier * displacement;

            if (d.X < 0.0f)
            {
                fatAABB.LowerBound.X += d.X;
            }
            else
            {
                fatAABB.UpperBound.X += d.X;
            }

            if (d.Y < 0.0f)
            {
                fatAABB.LowerBound.Y += d.Y;
            }
            else
            {
                fatAABB.UpperBound.Y += d.Y;
            }

            ref var proxyNode = ref _treeNodes[proxyId];
            ref var treeAABB = ref proxyNode.AABB;
            if (treeAABB.Contains(aabb))
            {
                // The tree AABB still contains the object, but it might be too large.
                // Perhaps the object was moving fast but has since gone to sleep.
                // The huge AABB is larger than the new fat AABB.
                var hugeAABB = new AABB
                {
                    LowerBound = fatAABB.LowerBound - 4.0f * r,
                    UpperBound = fatAABB.UpperBound + 4.0f * r
                };

                if (hugeAABB.Contains(treeAABB))
                {
                    // The tree AABB contains the object AABB and the tree AABB is
                    // not too large. No tree update needed.
                    return false;
                }

                // Otherwise the tree AABB is huge and needs to be shrunk
            }

            RemoveLeaf(proxyId);

            proxyNode.AABB = fatAABB;

            InsertLeaf(proxyId);

            proxyNode.Moved = true;

            return true;
        }

        /// Get proxy user data.
        /// @return the proxy user data or 0 if the id is invalid.
        public object GetUserData(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < _nodeCapacity);
            return _treeNodes[proxyId].UserData;
        }

        public bool WasMoved(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < _nodeCapacity);
            return _treeNodes[proxyId].Moved;
        }

        public void ClearMoved(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < _nodeCapacity);
            _treeNodes[proxyId].Moved = false;
        }

        /// Get the fat AABB for a proxy.
        public AABB GetFatAABB(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < _nodeCapacity);
            return _treeNodes[proxyId].AABB;
        }

        private readonly Stack<int> _queryStack = new Stack<int>(256);

        /// Query an AABB for overlapping proxies. The callback class
        /// is called for each proxy that overlaps the supplied AABB.
        public void Query(in ITreeQueryCallback callback, in AABB aabb)
        {
            var stack = _queryStack;
            stack.Clear();
            stack.Push(_root);

            while (stack.Count > 0)
            {
                var nodeId = stack.Pop();
                if (nodeId == NullNode)
                {
                    continue;
                }

                ref readonly var node = ref _treeNodes[nodeId];

                if (CollisionUtils.TestOverlap(node.AABB, aabb))
                {
                    if (node.IsLeaf())
                    {
                        var proceed = callback.QueryCallback(nodeId);
                        if (proceed == false)
                        {
                            return;
                        }
                    }
                    else
                    {
                        stack.Push(node.Child1);
                        stack.Push(node.Child2);
                    }
                }
            }

            stack.Clear();
        }

        private readonly Stack<int> _rayCastStack = new Stack<int>(256);

        /// Ray-cast against the proxies in the tree. This relies on the callback
        /// to perform a exact ray-cast in the case were the proxy contains a shape.
        /// The callback also performs the any collision filtering. This has performance
        /// roughly equal to k * log(n), where k is the number of collisions and n is the
        /// number of proxies in the tree.
        /// @param input the ray-cast input data. The ray extends from p1 to p1 + maxFraction * (p2 - p1).
        /// @param callback a callback class that is called for each proxy that is hit by the ray.
        public void RayCast(in ITreeRayCastCallback inputCallback, in RayCastInput input)
        {
            var p1 = input.P1;
            var p2 = input.P2;
            var r = p2 - p1;
            Debug.Assert(r.LengthSquared() > 0.0f);
            r.Normalize();

            // v is perpendicular to the segment.
            var v = MathUtils.Cross(1.0f, r);
            var abs_v = Vector2.Abs(v);

            // Separating axis for segment (Gino, p80).
            // |dot(v, p1 - c)| > dot(|v|, h)

            var maxFraction = input.MaxFraction;

            // Build a bounding box for the segment.
            var segmentAABB = new AABB();
            {
                var t = p1 + maxFraction * (p2 - p1);
                segmentAABB.LowerBound = Vector2.Min(p1, t);
                segmentAABB.UpperBound = Vector2.Max(p1, t);
            }

            var stack = _rayCastStack;
            stack.Clear();
            stack.Push(_root);

            while (stack.Count > 0)
            {
                var nodeId = stack.Pop();
                if (nodeId == NullNode)
                {
                    continue;
                }

                ref readonly var node = ref _treeNodes[nodeId];

                if (CollisionUtils.TestOverlap(node.AABB, segmentAABB) == false)
                {
                    continue;
                }

                // Separating axis for segment (Gino, p80).
                // |dot(v, p1 - c)| > dot(|v|, h)
                var c = node.AABB.GetCenter();
                var h = node.AABB.GetExtents();
                var separation = Math.Abs(Vector2.Dot(v, p1 - c)) - Vector2.Dot(abs_v, h);
                if (separation > 0.0f)
                {
                    continue;
                }

                if (node.IsLeaf())
                {
                    var subInput = new RayCastInput
                    {
                        P1 = input.P1,
                        P2 = input.P2,
                        MaxFraction = maxFraction
                    };

                    var value = inputCallback.RayCastCallback(subInput, nodeId);

                    if (value.Equals(0.0f))
                    {
                        // The client has terminated the ray cast.
                        return;
                    }

                    if (value > 0.0f)
                    {
                        // Update segment bounding box.
                        maxFraction = value;
                        var t = p1 + maxFraction * (p2 - p1);
                        segmentAABB.LowerBound = Vector2.Min(p1, t);
                        segmentAABB.UpperBound = Vector2.Max(p1, t);
                    }
                }
                else
                {
                    stack.Push(node.Child1);
                    stack.Push(node.Child2);
                }
            }
        }

        /// Validate this tree. For testing.
        public void Validate()
        {
#if b2DEBUG
	ValidateStructure(m_root);
	ValidateMetrics(m_root);

	int freeCount = 0;
	int freeIndex = m_freeList;
	while (freeIndex != b2_nullNode)
	{
		Debug.Assert(0 <= freeIndex && freeIndex < m_nodeCapacity);
		freeIndex = m_nodes[freeIndex].next;
		++freeCount;
	}

	Debug.Assert(GetHeight() == ComputeHeight());

	Debug.Assert(m_nodeCount + freeCount == m_nodeCapacity);
#endif
        }

        /// Compute the height of the binary tree in O(N) time. Should not be
        /// called often.
        public int GetHeight()
        {
            if (_root == NullNode)
            {
                return 0;
            }

            return _treeNodes[_root].Height;
        }

        /// Get the maximum balance of an node in the tree. The balance is the difference
        /// in height of the two children of a node.
        public int GetMaxBalance()
        {
            var maxBalance = 0;
            for (var i = 0; i < _nodeCapacity; ++i)
            {
                ref readonly var node = ref _treeNodes[i];
                if (node.Height <= 1)
                {
                    continue;
                }

                Debug.Assert(node.IsLeaf() == false);

                var child1 = node.Child1;
                var child2 = node.Child2;
                var balance = Math.Abs(_treeNodes[child2].Height - _treeNodes[child1].Height);
                maxBalance = Math.Max(maxBalance, balance);
            }

            return maxBalance;
        }

        /// Get the ratio of the sum of the node areas to the root area.
        public float GetAreaRatio()
        {
            if (_root == NullNode)
            {
                return 0.0f;
            }

            ref readonly var root = ref _treeNodes[_root];
            var rootArea = root.AABB.GetPerimeter();

            var totalArea = 0.0f;
            for (var i = 0; i < _nodeCapacity; ++i)
            {
                ref readonly var node = ref _treeNodes[i];
                if (node.Height < 0)
                {
                    // Free node in pool
                    continue;
                }

                totalArea += node.AABB.GetPerimeter();
            }

            return totalArea / rootArea;
        }

        /// Build an optimal tree. Very expensive. For testing.
        public void RebuildBottomUp()
        {
            var nodes = new int[_nodeCount];
            var count = 0;

            // Build array of leaves. Free the rest.
            for (var i = 0; i < _nodeCapacity; ++i)
            {
                ref var nodeI = ref _treeNodes[i];
                if (nodeI.Height < 0)
                {
                    // free node in pool
                    continue;
                }

                if (nodeI.IsLeaf())
                {
                    nodeI.Parent = NullNode;
                    nodes[count] = i;
                    ++count;
                }
                else
                {
                    FreeNode(i);
                }
            }

            while (count > 1)
            {
                var minCost = Settings.MaxFloat;
                int iMin = -1, jMin = -1;
                for (var i = 0; i < count; ++i)
                {
                    ref readonly var aabbi = ref _treeNodes[nodes[i]].AABB;

                    for (var j = i + 1; j < count; ++j)
                    {
                        var aabbj = _treeNodes[nodes[j]].AABB;
                        AABB.Combine(aabbi, aabbj, out var b);
                        var cost = b.GetPerimeter();
                        if (cost < minCost)
                        {
                            iMin = i;
                            jMin = j;
                            minCost = cost;
                        }
                    }
                }

                var index1 = nodes[iMin];
                var index2 = nodes[jMin];
                ref var child1 = ref _treeNodes[index1];
                ref var child2 = ref _treeNodes[index2];

                var parentIndex = AllocateNode();
                ref var parent = ref _treeNodes[parentIndex];
                parent.Child1 = index1;
                parent.Child2 = index2;
                parent.Height = 1 + Math.Max(child1.Height, child2.Height);
                parent.AABB.Combine(child1.AABB, child2.AABB);
                parent.Parent = NullNode;

                child1.Parent = parentIndex;
                child2.Parent = parentIndex;

                nodes[jMin] = nodes[count - 1];
                nodes[iMin] = parentIndex;
                --count;
            }

            _root = nodes[0];
            Validate();
        }

        /// Shift the world origin. Useful for large worlds.
        /// The shift formula is: position -= newOrigin
        /// @param newOrigin the new origin with respect to the old origin
        public void ShiftOrigin(in Vector2 newOrigin)
        {
            // Build array of leaves. Free the rest.
            for (var i = 0; i < _nodeCapacity; ++i)
            {
                _treeNodes[i].AABB.LowerBound -= newOrigin;
                _treeNodes[i].AABB.UpperBound -= newOrigin;
            }
        }

        private void InsertLeaf(int leaf)
        {
            if (_root == NullNode)
            {
                _root = leaf;
                _treeNodes[_root].Parent = NullNode;
                return;
            }

            // Find the best sibling for this node
            var leafAABB = _treeNodes[leaf].AABB;
            var index = _root;
            while (_treeNodes[index].IsLeaf() == false)
            {
                ref var indexNode = ref _treeNodes[index];
                var child1 = indexNode.Child1;
                var child2 = indexNode.Child2;

                var area = indexNode.AABB.GetPerimeter();

                AABB.Combine(indexNode.AABB, leafAABB, out var combinedAABB);
                var combinedArea = combinedAABB.GetPerimeter();

                // Cost of creating a new parent for this node and the new leaf
                var cost = 2.0f * combinedArea;

                // Minimum cost of pushing the leaf further down the tree
                var inheritanceCost = 2.0f * (combinedArea - area);

                // Cost of descending into child1
                float cost1;
                if (_treeNodes[child1].IsLeaf())
                {
                    AABB.Combine(leafAABB, _treeNodes[child1].AABB, out var aabb);
                    cost1 = aabb.GetPerimeter() + inheritanceCost;
                }
                else
                {
                    AABB.Combine(leafAABB, _treeNodes[child1].AABB, out var aabb);
                    var oldArea = _treeNodes[child1].AABB.GetPerimeter();
                    var newArea = aabb.GetPerimeter();
                    cost1 = newArea - oldArea + inheritanceCost;
                }

                // Cost of descending into child2
                float cost2;
                if (_treeNodes[child2].IsLeaf())
                {
                    AABB.Combine(leafAABB, _treeNodes[child2].AABB, out var aabb);
                    cost2 = aabb.GetPerimeter() + inheritanceCost;
                }
                else
                {
                    AABB.Combine(leafAABB, _treeNodes[child2].AABB, out var aabb);
                    var oldArea = _treeNodes[child2].AABB.GetPerimeter();
                    var newArea = aabb.GetPerimeter();
                    cost2 = newArea - oldArea + inheritanceCost;
                }

                // Descend according to the minimum cost.
                if (cost < cost1 && cost < cost2)
                {
                    break;
                }

                // Descend
                if (cost1 < cost2)
                {
                    index = child1;
                }
                else
                {
                    index = child2;
                }
            }

            var sibling = index;

            // Create a new parent.
            ref readonly var oldNode = ref _treeNodes[sibling];

            var oldParent = oldNode.Parent;
            var newParent = AllocateNode();
            ref var newParentNode = ref _treeNodes[newParent];
            newParentNode.Parent = oldParent;
            newParentNode.UserData = null;
            newParentNode.AABB.Combine(leafAABB, oldNode.AABB);
            newParentNode.Height = oldNode.Height + 1;

            if (oldParent != NullNode)
            {
                ref var oldParentNode = ref _treeNodes[oldParent];

                // The sibling was not the root.
                if (oldParentNode.Child1 == sibling)
                {
                    oldParentNode.Child1 = newParent;
                }
                else
                {
                    oldParentNode.Child2 = newParent;
                }

                newParentNode.Child1 = sibling;
                newParentNode.Child2 = leaf;
                _treeNodes[sibling].Parent = newParent;
                _treeNodes[leaf].Parent = newParent;
            }
            else
            {
                // The sibling was the root.
                newParentNode.Child1 = sibling;
                newParentNode.Child2 = leaf;
                _treeNodes[sibling].Parent = newParent;
                _treeNodes[leaf].Parent = newParent;
                _root = newParent;
            }

            // Walk back up the tree fixing heights and AABBs
            index = _treeNodes[leaf].Parent;
            while (index != NullNode)
            {
                index = Balance(index);
                ref var indexNode = ref _treeNodes[index];
                Debug.Assert(indexNode.Child1 != NullNode);
                Debug.Assert(indexNode.Child2 != NullNode);
                ref var child1 = ref _treeNodes[indexNode.Child1];
                ref var child2 = ref _treeNodes[indexNode.Child2];
                indexNode.Height = 1 + Math.Max(child1.Height, child2.Height);
                indexNode.AABB.Combine(child1.AABB, child2.AABB);

                index = indexNode.Parent;
            }

            //Validate();
        }

        private void RemoveLeaf(int leaf)
        {
            if (leaf == _root)
            {
                _root = NullNode;
                return;
            }

            var parent = _treeNodes[leaf].Parent;
            ref var parentNode = ref _treeNodes[parent];
            var grandParent = parentNode.Parent;

            var sibling = parentNode.Child1 == leaf ? parentNode.Child2 : parentNode.Child1;

            if (grandParent != NullNode)
            {
                ref var grandParentNode = ref _treeNodes[grandParent];

                // Destroy parent and connect sibling to grandParent.
                if (grandParentNode.Child1 == parent)
                {
                    grandParentNode.Child1 = sibling;
                }
                else
                {
                    grandParentNode.Child2 = sibling;
                }

                _treeNodes[sibling].Parent = grandParent;
                FreeNode(parent);

                // Adjust ancestor bounds.
                var index = grandParent;
                while (index != NullNode)
                {
                    index = Balance(index);
                    ref var indexNode = ref _treeNodes[index];
                    ref var child1 = ref _treeNodes[indexNode.Child1];
                    ref var child2 = ref _treeNodes[indexNode.Child2];

                    indexNode.AABB.Combine(child1.AABB, child2.AABB);
                    indexNode.Height = 1 + Math.Max(child1.Height, child2.Height);

                    index = indexNode.Parent;
                }
            }
            else
            {
                _root = sibling;
                _treeNodes[sibling].Parent = NullNode;
                FreeNode(parent);
            }

            //Validate();
        }

        private int Balance(int iA)
        {
            Debug.Assert(iA != NullNode);

            ref var A = ref _treeNodes[iA];
            if (A.IsLeaf() || A.Height < 2)
            {
                return iA;
            }

            var iB = A.Child1;
            var iC = A.Child2;
            Debug.Assert(0 <= iB && iB < _nodeCapacity);
            Debug.Assert(0 <= iC && iC < _nodeCapacity);

            ref var B = ref _treeNodes[iB];
            ref var C = ref _treeNodes[iC];

            var balance = C.Height - B.Height;

            // Rotate C up
            if (balance > 1)
            {
                var iF = C.Child1;
                var iG = C.Child2;
                ref var F = ref _treeNodes[iF];
                ref var G = ref _treeNodes[iG];
                Debug.Assert(0 <= iF && iF < _nodeCapacity);
                Debug.Assert(0 <= iG && iG < _nodeCapacity);

                // Swap A and C
                C.Child1 = iA;
                C.Parent = A.Parent;
                A.Parent = iC;

                // A's old parent should point to C
                if (C.Parent != NullNode)
                {
                    ref var cParentNode = ref _treeNodes[C.Parent];
                    if (cParentNode.Child1 == iA)
                    {
                        cParentNode.Child1 = iC;
                    }
                    else
                    {
                        Debug.Assert(_treeNodes[C.Parent].Child2 == iA);
                        cParentNode.Child2 = iC;
                    }
                }
                else
                {
                    _root = iC;
                }

                // Rotate
                if (F.Height > G.Height)
                {
                    C.Child2 = iF;
                    A.Child2 = iG;
                    G.Parent = iA;
                    A.AABB.Combine(B.AABB, G.AABB);
                    C.AABB.Combine(A.AABB, F.AABB);

                    A.Height = 1 + Math.Max(B.Height, G.Height);
                    C.Height = 1 + Math.Max(A.Height, F.Height);
                }
                else
                {
                    C.Child2 = iG;
                    A.Child2 = iF;
                    F.Parent = iA;
                    A.AABB.Combine(B.AABB, F.AABB);
                    C.AABB.Combine(A.AABB, G.AABB);

                    A.Height = 1 + Math.Max(B.Height, F.Height);
                    C.Height = 1 + Math.Max(A.Height, G.Height);
                }

                return iC;
            }

            // Rotate B up
            if (balance < -1)
            {
                var iD = B.Child1;
                var iE = B.Child2;
                ref var D = ref _treeNodes[iD];
                ref var E = ref _treeNodes[iE];
                Debug.Assert(0 <= iD && iD < _nodeCapacity);
                Debug.Assert(0 <= iE && iE < _nodeCapacity);

                // Swap A and B
                B.Child1 = iA;
                B.Parent = A.Parent;
                A.Parent = iB;

                // A's old parent should point to B
                if (B.Parent != NullNode)
                {
                    ref var bParentNode = ref _treeNodes[B.Parent];
                    if (bParentNode.Child1 == iA)
                    {
                        bParentNode.Child1 = iB;
                    }
                    else
                    {
                        Debug.Assert(_treeNodes[B.Parent].Child2 == iA);
                        bParentNode.Child2 = iB;
                    }
                }
                else
                {
                    _root = iB;
                }

                // Rotate
                if (D.Height > E.Height)
                {
                    B.Child2 = iD;
                    A.Child1 = iE;
                    E.Parent = iA;
                    A.AABB.Combine(C.AABB, E.AABB);
                    B.AABB.Combine(A.AABB, D.AABB);

                    A.Height = 1 + Math.Max(C.Height, E.Height);
                    B.Height = 1 + Math.Max(A.Height, D.Height);
                }
                else
                {
                    B.Child2 = iE;
                    A.Child1 = iD;
                    D.Parent = iA;
                    A.AABB.Combine(C.AABB, D.AABB);
                    B.AABB.Combine(A.AABB, E.AABB);

                    A.Height = 1 + Math.Max(C.Height, D.Height);
                    B.Height = 1 + Math.Max(A.Height, E.Height);
                }

                return iB;
            }

            return iA;
        }

        private int ComputeHeight()
        {
            var height = ComputeHeight(_root);
            return height;
        }

        private int ComputeHeight(int nodeId)
        {
            Debug.Assert(0 <= nodeId && nodeId < _nodeCapacity);
            ref readonly var node = ref _treeNodes[nodeId];

            if (node.IsLeaf())
            {
                return 0;
            }

            var height1 = ComputeHeight(node.Child1);
            var height2 = ComputeHeight(node.Child2);
            return 1 + Math.Max(height1, height2);
        }

        private void ValidateStructure(int index)
        {
            if (index == NullNode)
            {
                return;
            }

            if (index == _root)
            {
                Debug.Assert(_treeNodes[index].Parent == NullNode);
            }

            ref readonly var node = ref _treeNodes[index];

            var child1 = node.Child1;
            var child2 = node.Child2;

            if (node.IsLeaf())
            {
                Debug.Assert(child1 == NullNode);
                Debug.Assert(child2 == NullNode);
                Debug.Assert(node.Height == 0);
                return;
            }

            Debug.Assert(0 <= child1 && child1 < _nodeCapacity);
            Debug.Assert(0 <= child2 && child2 < _nodeCapacity);

            Debug.Assert(_treeNodes[child1].Parent == index);
            Debug.Assert(_treeNodes[child2].Parent == index);

            ValidateStructure(child1);
            ValidateStructure(child2);
        }

        private void ValidateMetrics(int index)
        {
            if (index == NullNode)
            {
                return;
            }

            ref readonly var node = ref _treeNodes[index];

            var child1 = node.Child1;
            var child2 = node.Child2;

            if (node.IsLeaf())
            {
                Debug.Assert(child1 == NullNode);
                Debug.Assert(child2 == NullNode);
                Debug.Assert(node.Height == 0);
                return;
            }

            Debug.Assert(0 <= child1 && child1 < _nodeCapacity);
            Debug.Assert(0 <= child2 && child2 < _nodeCapacity);

            var height1 = _treeNodes[child1].Height;
            var height2 = _treeNodes[child2].Height;
            var height = 1 + Math.Max(height1, height2);
            Debug.Assert(node.Height == height);

            AABB.Combine(_treeNodes[child1].AABB, _treeNodes[child2].AABB, out var aabb);

            Debug.Assert(aabb.LowerBound == node.AABB.LowerBound);
            Debug.Assert(aabb.UpperBound == node.AABB.UpperBound);

            ValidateMetrics(child1);
            ValidateMetrics(child2);
        }
    }

    public struct TreeNode
    {
        /// Enlarged AABB
        public AABB AABB;

        public int Child1;

        public int Child2;

        // leaf = 0, free node = -1
        public int Height;

        public object UserData;

        // union next
        public int Parent { get; set; }

        // union parent
        public int Next
        {
            get => Parent;
            set => Parent = value;
        }

        public bool Moved;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public bool IsLeaf()
        {
            return Child1 == DynamicTree.NullNode;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            AABB = default;
            Child1 = default;
            Child2 = default;
            Height = default;
            UserData = default;
            Parent = default;
        }
    }
}