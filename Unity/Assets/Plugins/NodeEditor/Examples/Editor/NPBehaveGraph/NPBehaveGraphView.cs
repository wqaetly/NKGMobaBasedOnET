using System.Collections.Generic;
using System.Linq;
using ET;
using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public class NPBehaveGraphView : UniversalGraphView
    {
        private float m_LevelPosX;
        private float m_LevelPosY;
        private int m_Level;
        private const float m_NodeXGap = 40f;

        private const float m_NodeYGap = 60f;
        
        private List<List<NP_NodeView>> m_HashTree = new List<List<NP_NodeView>>();
        public NPBehaveGraphWindow NpBehaveGraphWindow;

        private NP_NodeView m_RootNodeView
        {
            get { return (NP_NodeView) this.nodeViews.Find(x => x.nodeTarget.name == "行为树根节点"); }
        }

        public NPBehaveGraphView(EditorWindow window) : base(window)
        {
            NpBehaveGraphWindow = window as NPBehaveGraphWindow;
        }

        /// <summary>
        /// 自动排序布局
        /// </summary>
        public void AutoSortLayout()
        {
            var rootNodeView = m_RootNodeView;
            if (rootNodeView == null)
            {
                return;
            }

            m_Level = 0;
            m_LevelPosX = 0;
            m_LevelPosY = 0;

            // 先计算节点之间的联系（配置父节点和子节点）
            CalculateNodeRelationShip(rootNodeView);
            // 计算每个节点的Level信息
            CalculateNodeLevelInfo(rootNodeView, m_Level);
            // 从根节点开始，递归配置每个节点及其子节点的位置
            LayoutChildren(rootNodeView);
            // 因为配置完之后可能两个节点会发生重叠，所以要再做一次判断展开
            LayoutOverlaps();
            // 最后将缓存的位置信息设置到NodeView的真实位置上
            UpdateNodeRect(rootNodeView);
        }

        private void CalculateNodeRelationShip(NP_NodeView rootNodeView)
        {
            rootNodeView.Parent = null;
            rootNodeView.Children.Clear();
            var outputPort = rootNodeView.outputPortViews;
            var inputPort = rootNodeView.inputPortViews;

            if (inputPort.Count > 0)
            {
                var inputEdges = inputPort[0].GetEdges();
                if (inputEdges.Count > 0)
                {
                    rootNodeView.Parent = inputEdges[0].output.node as NP_NodeView;
                }
                else
                {
                    Log.Error("当前行为树配置有误，请检查是否有节点未正确链接");
                }
            }

            if (outputPort.Count > 0)
            {
                var outputEdges = outputPort[0].GetEdges();
                if (outputEdges.Count > 0)
                {
                    foreach (var outputEdge in outputEdges)
                    {
                        var childNodeView = outputEdge.input.node as NP_NodeView;
                        CalculateNodeRelationShip(childNodeView);
                        rootNodeView.Children.Add(childNodeView);
                    }

                    // 根据x坐标进行排序
                    rootNodeView.Children.Sort((x, y) => x.GetPosition().x.CompareTo(y.GetPosition().x));
                }
                else
                {
                    Log.Error("当前行为树配置有误，请检查是否有节点未正确链接");
                }
            }
        }

        private void CalculateNodeLevelInfo(NP_NodeView rootNodeView, int parentLevel)
        {
            rootNodeView.NpNodeBase.Level = parentLevel + 1;
            foreach (var npNodeView in rootNodeView.Children)
            {
                CalculateNodeLevelInfo(npNodeView, rootNodeView.NpNodeBase.Level);
            }
        }

        /// <summary>
        /// 将当前节点的所有子节点按照等间距布局
        /// </summary>
        /// <param name="root"></param>
        private void LayoutChildren(NP_NodeView rootNodeView)
        {
            if (rootNodeView == null)
            {
                return;
            }

            // 更新节点的纵坐标位置
            Rect rootNodeViewRect = rootNodeView.CachedPositionRect;
            rootNodeViewRect.y = m_NodeYGap * rootNodeView.NpNodeBase.Level + m_LevelPosY;
            m_LevelPosY += rootNodeView.CachedSizeRect.height;
            rootNodeView.SetPositionRect(rootNodeViewRect);

            var childrenNodeViews = rootNodeView.Children;
            var childrenCount = childrenNodeViews.Count;
            if (childrenCount == 0)
            {
                m_LevelPosY -= rootNodeView.CachedSizeRect.height;
                return;
            }

            m_LevelPosX = 0;
            foreach (var child in childrenNodeViews)
            {
                m_LevelPosX += child.CachedSizeRect.width + m_NodeXGap;
            }

            float startPosX = rootNodeViewRect.x - (childrenCount - 1) * m_LevelPosX / childrenCount / 2;
            int i = 0;
            float x = startPosX;
            foreach (var child in childrenNodeViews)
            {
                // 计算当前子节点横坐标
                if (i != 0)
                {
                    x += childrenNodeViews[i - 1].CachedSizeRect.width + m_NodeXGap;
                }

                // 移动该子节点及以该子节点为根的整棵树
                TranslateTree(child, x);
                // 递归布局该子节点
                LayoutChildren(child);
                ++i;
            }

            CenterChild(rootNodeView);
            m_LevelPosY -= rootNodeViewRect.height;
        }

        /// <summary>
        /// 回推函数，寻找重叠的节点，优化节点布局
        /// </summary>
        private void LayoutOverlaps()
        {
            UpdateHashTree();

            // 从最底层开始往上获取节点
            for (int i = m_HashTree.Count - 1; i >= 0; i--)
            {
                // 获取当前层
                var curLayer = m_HashTree[i];

                // 遍历该层所有节点
                for (int j = 0; j < curLayer.Count - 1; j++)
                {
                    // 获取相邻的两个节点，保存为n1，n2
                    var n1 = curLayer[j];
                    var n2 = curLayer[j + 1];

                    // 若n1，n2有重叠
                    if (IsOverlaps(n1, n2))
                    {
                        // 计算需要水平移动的距离
                        float dx = n1.CachedPositionRect.x + n1.CachedSizeRect.width + m_NodeXGap -
                                   n2.CachedPositionRect.x;
                        // 找出与n1的某个祖先为兄弟节点的n2的祖先
                        var node2Move = FindCommonParentNode(n1, n2);

                        // 往右移动n2
                        TranslateTree(node2Move, node2Move.CachedPositionRect.x + dx);
                        // 居中对齐
                        CenterChild(node2Move.Parent);

                        // 移动后下层节点有可能再次发生重叠，所以重新从底层扫描
                        i = m_HashTree.Count;
                    }
                }
            }
        }

        /// <summary>
        /// 判断节点之间是否重合
        /// </summary>
        /// <returns></returns>
        private bool IsOverlaps(NP_NodeView node1,
            NP_NodeView node2)
        {
            return (node1.CachedPositionRect.x - node2.CachedPositionRect.x) > 0 ||
                   (node2.CachedPositionRect.x - node1.CachedPositionRect.x) < node1.GetPosition().width;
        }

        /// <summary>
        /// 所有节点居中对齐
        /// </summary>
        private void CenterChild(NP_NodeView parentNodeView)
        {
            // 偏移的位置
            float dx = 0;
            if (parentNodeView == null)
            {
                return;
            }

            var childNodes = parentNodeView.Children;
            // 只有一个子节点，直接将子节点位置和父节点位置对其（考虑不同的节点可能宽度不一样，所以还得把宽度因素考虑进去）
            if (parentNodeView.ChildCount == 1)
            {
                dx = parentNodeView.CachedPositionRect.x - childNodes[0].CachedPositionRect.x + (parentNodeView.CachedSizeRect.width - childNodes[0].CachedSizeRect.width) / 2;
            }

            if (parentNodeView.ChildCount > 1)
            {
                dx = parentNodeView.CachedPositionRect.x - (childNodes[0].CachedPositionRect.x +
                                                            (childNodes[parentNodeView.ChildCount - 1]
                                                                 .CachedPositionRect.x -
                                                             childNodes[0].CachedPositionRect.x) / 2);
            }

            if (dx != 0)
            {
                foreach (var t in parentNodeView.Children)
                {
                    TranslateTree(t, t.CachedPositionRect.x + dx);
                }
            }
        }

        /// <summary>
        /// 位移整棵子树
        /// </summary>
        /// <param name="rootNodeView"> 树的根节点 </param>
        /// <param name="x"> 要移动到的水平位置 </param>
        private void TranslateTree(NP_NodeView rootNodeView, float x)
        {
            Rect rect = rootNodeView.CachedPositionRect;
            float dx = x - rect.x;
            rect.x = x;
            rootNodeView.SetPositionRect(rect);

            foreach (var t in rootNodeView.Children)
            {
                TranslateTree(t, t.CachedPositionRect.x + dx);
            }
        }
        
        private void UpdateHashTree()
        {
            m_HashTree.Clear();

            Queue<NP_NodeView> nodeQueue = new Queue<NP_NodeView>();
            nodeQueue.Enqueue(m_RootNodeView);
            m_HashTree.Add(new List<NP_NodeView>() {m_RootNodeView});

            while (nodeQueue.Count > 0)
            {
                int size = nodeQueue.Count;
                List<NP_NodeView> tempList = new List<NP_NodeView>();
                for (int i = 0; i < size; ++i)
                {
                    var node = nodeQueue.Dequeue();
                    foreach (var child in node.Children)
                    {
                        tempList.Add(child);
                        nodeQueue.Enqueue(child);
                    }
                }

                m_HashTree.Add(tempList);
            }
        }

        /// <summary>
        /// 找到和node1的祖先节点是兄弟节点的node2的祖先节点
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        private NP_NodeView FindCommonParentNode(NP_NodeView node1,
            NP_NodeView node2)
        {
            if (node1 == null || node2 == null)
            {
                return null;
            }

            if (node1.Parent == node2.Parent)
            {
                return node2;
            }

            return FindCommonParentNode(node1.Parent, node2.Parent);
        }

        /// <summary>
        /// 更新每个节点的位置
        /// </summary>
        /// <param name="node"></param>
        private void UpdateNodeRect(NP_NodeView node)
        {
            if (node == null)
            {
                return;
            }

            if (node.CachedPositionRect != node.GetPosition())
            {
                node.SetPosition(node.CachedPositionRect);
            }

            foreach (var child in node.Children)
            {
                UpdateNodeRect(child);
            }
        }
    }
}