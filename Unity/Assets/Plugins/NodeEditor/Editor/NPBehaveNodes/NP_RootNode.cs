//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月20日 8:00:48
//------------------------------------------------------------

using System;
using ETModel;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/根结点", typeof (NPBehaveCanvas))]
    public class NP_RootNode: Node
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "行为树根结点";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        public override Vector2 DefaultSize => new Vector2(150, 60);

        [ValueConnectionKnob("NPBehave_PreNode", Direction.In, "NPBehave_PrevNodeDatas", NodeSide.Top, 75)]
        public ValueConnectionKnob PrevNode;

        [ValueConnectionKnob("NPBehave_NextNode", Direction.Out, "NPBehave_NextNodeDatas", NodeSide.Bottom, 75)]
        public ValueConnectionKnob NextNode;

        [LabelText("根结点")]
        public NP_RootNodeData MRootNodeData;

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return this.MRootNodeData;
        }

        private void OnEnable()
        {
            if (this.MRootNodeData == null)
            {
                this.MRootNodeData = new NP_RootNodeData();
            }

            this.MRootNodeData.NodeDes = "根结点，是整棵行为树的根";
        }

        public override void NodeGUI()
        {
            EditorGUILayout.TextField(this.MRootNodeData.NodeDes);
        }
    }
}