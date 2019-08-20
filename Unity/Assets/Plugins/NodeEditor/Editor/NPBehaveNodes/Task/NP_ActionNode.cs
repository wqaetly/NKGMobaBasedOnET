//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月20日 20:15:12
//------------------------------------------------------------

using ETModel;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/行为节点", typeof (NPBehaveCanvas))]
    public class NP_ActionNode: Node
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "行为树行为结点";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;
        
        public override Vector2 DefaultSize => new Vector2(150, 60);

        [ValueConnectionKnob("NPBehave_PreNode", Direction.In, "NPBehave_PrevNodeDatas", NodeSide.Top, 75)]
        public ValueConnectionKnob PrevNode;

        [ValueConnectionKnob("NPBehave_NextNode", Direction.Out, "NPBehave_NextNodeDatas", NodeSide.Bottom, 75)]
        public ValueConnectionKnob NextNode;

        [LabelText("行为结点")]
        public NP_ActionNodeData MNpActionNodeData;

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return this.MNpActionNodeData;
        }

        private void OnEnable()
        {
            if (this.MNpActionNodeData == null)
            {
                this.MNpActionNodeData = new NP_ActionNodeData();
            }

            this.MNpActionNodeData.NodeDes = "行为树的行为节点";
        }

        public override void NodeGUI()
        {
            EditorGUILayout.TextField(this.MNpActionNodeData.NodeDes);
        }
    }
}