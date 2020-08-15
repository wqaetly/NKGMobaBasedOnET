//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 13:11:41
//------------------------------------------------------------

using System.Collections.Generic;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树结点", typeof(NeverbeUsedCanvas))]
    public abstract class NP_NodeBase : Node
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "行为树节点";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        public override Vector2 DefaultSize => NodeSize;

        [LabelText("结点大小")] [OnValueChanged("ApplyNodeSize")]
        public Vector2 NodeSize = new Vector2(150, 60);

        public virtual void AutoBindAllDelegate()
        {
        }

        /// <summary>
        /// 获取下面链接的节点
        /// </summary>
        /// <returns></returns>
        public abstract ValueConnectionKnob GetNextNodes();

        public abstract void ApplyNodeSize();

        public override void NodeGUI()
        {
            EditorGUILayout.TextField("不允许使用此结点");
        }
    }
}