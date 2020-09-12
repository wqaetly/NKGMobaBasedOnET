//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月16日 10:54:20
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace NodeEditorFramework.Utilities.CreateNodesWindow
{
    public class CreateNodesAdvancedDropdown: AdvancedDropdown
    {
        private static Vector2 WindowSize = new Vector2(200, 200);

        /// <summary>
        /// 所有Item
        /// </summary>
        private static Dictionary<string, NodeItem> s_AllItems = new Dictionary<string, NodeItem>();

        private static Vector2 m_CanvasPos = Vector2.zero;

        /// <summary>
        /// 从哪个端口引过来的连线
        /// </summary>
        private static ConnectionKnob s_FromConnectionKnob;

        /// <summary>
        /// 展示下拉框
        /// </summary>
        /// <param name="position"></param>
        public static CreateNodesAdvancedDropdown ShowDropdown(Rect position, ConnectionKnob fromConnectionKnob = null)
        {
            CreateNodesAdvancedDropdown window = new CreateNodesAdvancedDropdown(new AdvancedDropdownState());
            window.minimumSize = WindowSize;
            window.Show(position);
            m_CanvasPos = NodeEditor.ScreenToCanvasSpace(position.position);
            if (fromConnectionKnob != null)
            {
                s_FromConnectionKnob = fromConnectionKnob;
            }

            return window;
        }

        public CreateNodesAdvancedDropdown(AdvancedDropdownState state): base(state)
        {
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new NodeItem("添加内容");
            s_AllItems.Clear();
            BuildResources(root);
            NodeItem groupItem = new NodeItem("创建Group", "创建Group");
            root.AddChild(groupItem);
            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);
            if (!(item is NodeItem nodeItem))
            {
                Debug.Log("转型失败");
                return;
            }

            if (item.name == "创建Group")
            {
                new NodeGroup("Group", m_CanvasPos);
            }
            else
            {
                Node node = Node.Create(nodeItem.NodeId, m_CanvasPos, NodeEditor.curEditorState.canvas, NodeEditor.curEditorState.connectKnob);
                if (s_FromConnectionKnob != null && node.connectionKnobs.Count > 0)
                {
                    s_FromConnectionKnob.TryApplyConnection(node.connectionKnobs[0]);
                    s_FromConnectionKnob = null;
                }
            }
            
            NodeEditor.RepaintClients();
        }

        private void BuildResources(NodeItem root)
        {
            foreach (var nodeID in NodeTypes.GetCompatibleNodes(null))
            {
                if (NodeCanvasManager.CheckCanvasCompability(nodeID, NodeEditor.curNodeCanvas.GetType()))
                {
                    NodeTypeData nodeTypeData = NodeTypes.GetNodeData(nodeID);
                    BuildSingleCategory(root, NodeTypes.GetNodeData(nodeID).adress, nodeTypeData.typeID);
                }
            }
        }

        private void BuildSingleCategory(NodeItem root, string targetContent, string nodeId)
        {
            string[] items = targetContent.Split('/');
            NodeItem parentItem = root;
            NodeItem tempItem;
            for (int i = 0; i < items.Length; i++)
            {
                if (s_AllItems.TryGetValue(items[i], out tempItem))
                {
                    parentItem = tempItem;
                }
                else
                {
                    NodeItem childItem = new NodeItem(items[i], nodeId);
                    parentItem.AddChild(childItem);
                    parentItem = childItem;
                    s_AllItems.Add(items[i], parentItem);
                }
            }
        }
    }
}