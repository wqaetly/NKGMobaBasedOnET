using System;
using System.Buffers;
using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Internal;

namespace Box2DSharp.Collision
{
    public class BroadPhase : ITreeQueryCallback
    {
        public const int NullProxy = -1;

        private DynamicTree _tree;

        private int _proxyCount;

        private int[] _moveBuffer;

        private int _moveCapacity;

        private int _moveCount;

        private Pair[] _pairBuffer;

        private int _pairCapacity;

        private int _pairCount;

        private int _queryProxyId;

        public BroadPhase()
        {
            _proxyCount = 0;
            _tree = new DynamicTree();
            _pairCapacity = 16;
            _pairCount = 0;
            _pairBuffer = ArrayPool<Pair>.Shared.Rent(_pairCapacity);
            _moveCapacity = 16;
            _moveCount = 0;
            _moveBuffer = ArrayPool<int>.Shared.Rent(_moveCapacity);
        }

        ~BroadPhase()
        {
            if (_pairBuffer != null)
            {
                ArrayPool<Pair>.Shared.Return(_pairBuffer, true);
            }

            if (_moveBuffer != null)
            {
                ArrayPool<int>.Shared.Return(_moveBuffer, true);
            }
        }

        /// Create a proxy with an initial AABB. Pairs are not reported until
        /// UpdatePairs is called.
        public int CreateProxy(in AABB aabb, FixtureProxy userData)
        {
            var proxyId = _tree.CreateProxy(aabb, userData);
            ++_proxyCount;
            BufferMove(proxyId);
            return proxyId;
        }

        /// Destroy a proxy. It is up to the client to remove any pairs.
        public void DestroyProxy(int proxyId)
        {
            UnBufferMove(proxyId);
            --_proxyCount;
            _tree.DestroyProxy(proxyId);
        }

        /// Call MoveProxy as many times as you like, then when you are done
        /// call UpdatePairs to finalized the proxy pairs (for your time step).
        public void MoveProxy(int proxyId, in AABB aabb, in Vector2 displacement)
        {
            var buffer = _tree.MoveProxy(proxyId, aabb, displacement);
            if (buffer)
            {
                BufferMove(proxyId);
            }
        }

        /// Call to trigger a re-processing of it's pairs on the next call to UpdatePairs.
        public void TouchProxy(int proxyId)
        {
            BufferMove(proxyId);
        }

        /// Get the fat AABB for a proxy.
        public AABB GetFatAABB(int proxyId)
        {
            return _tree.GetFatAABB(proxyId);
        }

        /// Get user data from a proxy. Returns nullptr if the id is invalid.
        internal object GetUserData(int proxyId)
        {
            return _tree.GetUserData(proxyId);
        }

        /// Test overlap of fat AABBs.
        public bool TestOverlap(int proxyIdA, int proxyIdB)
        {
            return CollisionUtils.TestOverlap(_tree.GetFatAABB(proxyIdA), _tree.GetFatAABB(proxyIdB));
        }

        /// Get the number of proxies.
        public int GetProxyCount()
        {
            return _proxyCount;
        }

        /// Update the pairs. This results in pair callbacks. This can only add pairs.
        internal void UpdatePairs<T>(T callback)
            where T : IAddPairCallback
        {
            // Reset pair buffer
            _pairCount = 0;

            // Perform tree queries for all moving proxies.
            for (var j = 0; j < _moveCount; ++j)
            {
                _queryProxyId = _moveBuffer[j];
                if (_queryProxyId == NullProxy)
                {
                    continue;
                }

                // We have to query the tree with the fat AABB so that
                // we don't fail to create a pair that may touch later.
                var fatAABB = _tree.GetFatAABB(_queryProxyId);

                // Query tree, create pairs and add them pair buffer.
                _tree.Query(this, fatAABB);
            }

            // Send pairs to caller
            for (var i = 0; i < _pairCount; ++i)
            {
                ref readonly var primaryPair = ref _pairBuffer[i];
                var userDataA = _tree.GetUserData(primaryPair.ProxyIdA);
                var userDataB = _tree.GetUserData(primaryPair.ProxyIdB);

                callback.AddPairCallback(userDataA, userDataB);
            }

            // Clear move flags
            for (var i = 0; i < _moveCount; ++i)
            {
                var proxyId = _moveBuffer[i];
                if (proxyId == NullProxy)
                {
                    continue;
                }

                _tree.ClearMoved(proxyId);
            }

            // Reset move buffer
            _moveCount = 0;
        }

        /// Query an AABB for overlapping proxies. The callback class
        /// is called for each proxy that overlaps the supplied AABB.
        public void Query(in ITreeQueryCallback callback, in AABB aabb)
        {
            _tree.Query(callback, aabb);
        }

        /// Ray-cast against the proxies in the tree. This relies on the callback
        /// to perform a exact ray-cast in the case were the proxy contains a shape.
        /// The callback also performs the any collision filtering. This has performance
        /// roughly equal to k * log(n), where k is the number of collisions and n is the
        /// number of proxies in the tree.
        /// @param input the ray-cast input data. The ray extends from p1 to p1 + maxFraction * (p2 - p1).
        /// @param callback a callback class that is called for each proxy that is hit by the ray.
        public void RayCast(in ITreeRayCastCallback callback, in RayCastInput input)
        {
            _tree.RayCast(callback, input);
        }

        /// Get the height of the embedded tree.
        public int GetTreeHeight()
        {
            return _tree.GetHeight();
        }

        /// Get the balance of the embedded tree.
        public int GetTreeBalance()
        {
            return _tree.GetMaxBalance();
        }

        /// Get the quality metric of the embedded tree.
        public float GetTreeQuality()
        {
            return _tree.GetAreaRatio();
        }

        /// Shift the world origin. Useful for large worlds.
        /// The shift formula is: position -= newOrigin
        /// @param newOrigin the new origin with respect to the old origin
        public void ShiftOrigin(in Vector2 newOrigin)
        {
            _tree.ShiftOrigin(newOrigin);
        }

        private void BufferMove(int proxyId)
        {
            if (_moveCount == _moveCapacity)
            {
                var oldBuffer = _moveBuffer;
                _moveCapacity *= 2;
                _moveBuffer = ArrayPool<int>.Shared.Rent(_moveCapacity);
                Array.Copy(oldBuffer, _moveBuffer, _moveCount);
                Array.Clear(oldBuffer, 0, _moveCount);
                ArrayPool<int>.Shared.Return(oldBuffer);
            }

            _moveBuffer[_moveCount] = proxyId;
            ++_moveCount;
        }

        private void UnBufferMove(int proxyId)
        {
            for (var i = 0; i < _moveCount; ++i)
            {
                if (_moveBuffer[i] == proxyId)
                {
                    _moveBuffer[i] = NullProxy;
                }
            }
        }

        public bool QueryCallback(int proxyId)
        {
            // A proxy cannot form a pair with itself.
            if (proxyId == _queryProxyId)
            {
                return true;
            }

            var moved = _tree.WasMoved(proxyId);
            if (moved && proxyId > _queryProxyId)
            {
                // Both proxies are moving. Avoid duplicate pairs.
                return true;
            }

            // Grow the pair buffer as needed.
            if (_pairCount == _pairCapacity)
            {
                var oldBuffer = _pairBuffer;
                _pairCapacity += _pairCapacity >> 1;
                _pairBuffer = ArrayPool<Pair>.Shared.Rent(_pairCapacity);
                Array.Copy(oldBuffer, _pairBuffer, _pairCount);
                Array.Clear(oldBuffer, 0, _pairCount);
                ArrayPool<Pair>.Shared.Return(oldBuffer);
            }

            _pairBuffer[_pairCount].ProxyIdA = Math.Min(proxyId, _queryProxyId);
            _pairBuffer[_pairCount].ProxyIdB = Math.Max(proxyId, _queryProxyId);
            ++_pairCount;

            return true;
        }
    }

    public struct Pair
    {
        public int ProxyIdA;

        public int ProxyIdB;
    }
}