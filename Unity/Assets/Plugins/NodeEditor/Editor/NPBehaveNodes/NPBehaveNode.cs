//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月20日 8:00:48
//------------------------------------------------------------

using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/Test", typeof (NPBehaveCanvas))]
    public class NPBehaveNode: Node
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "行为树测试结点";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        public override Vector2 DefaultSize => new Vector2(150, 60);

        [LabelText("结点行为描述")]
        public string NodeDescribe;

        [ValueConnectionKnob("NPBehave_PreNode", Direction.In, "NPBehave_PrevNodeDatas", NodeSide.Top, 75)]
        public ValueConnectionKnob PrevNode;

        [ValueConnectionKnob("NPBehave_NextNode", Direction.Out, "NPBehave_NextNodeDatas", NodeSide.Bottom, 75)]
        public ValueConnectionKnob NextNode;

        /// <summary>
        /// 英雄数据
        /// </summary>
        public NPBehave.Node m_HeroData;

        public override void NodeGUI()
        {
            EditorGUILayout.TextField(NodeDescribe);
        }
    }
}