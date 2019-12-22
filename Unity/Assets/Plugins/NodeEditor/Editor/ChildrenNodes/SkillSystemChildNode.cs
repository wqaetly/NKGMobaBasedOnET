//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年12月22日 14:40:16
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor.Editor.ChildrenNodes
{
    [Node(false, "子图部分/生成的碰撞体", typeof (NPBehaveCanvas))]
    public class SkillSystemChildNode: Node
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "子图——生成的碰撞体";

        public override string GetID => Id;

        public override Vector2 DefaultSize => new Vector2(150, 60);

        [ValueConnectionKnob("NPBehave_PreNode", Direction.In, "NPBehave_PrevNodeDatas", NodeSide.Top, 75)]
        public ValueConnectionKnob PrevNode;

        [LabelText("子图集合")]
        public List<ChildrenNodeDatas> ChildrenNodeDatas;

        private void OnEnable()
        {
            this.backgroundColor = Color.magenta;
        }

        public override void NodeGUI()
        {
            EditorGUILayout.TextField("子图——生成的碰撞体");
        }
    }
}