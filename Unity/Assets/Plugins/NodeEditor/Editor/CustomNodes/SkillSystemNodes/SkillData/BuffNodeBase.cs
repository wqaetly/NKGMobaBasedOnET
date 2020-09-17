//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月23日 18:36:38
//------------------------------------------------------------

using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using UnityEditor;
using UnityEngine;

namespace Plugins
{
    [Node(false, "技能数据结点", typeof (NeverbeUsedCanvas))]
    public class BuffNodeBase: Node
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "技能数据结点";

        public override string GetID => Id;

        public override Vector2 DefaultSize => new Vector2(150, 60);
        
        [ValueConnectionKnob("PrevBuffType", Direction.In, "PrevNodeDatas", NodeSide.Left, 30, MaxConnectionCount = ConnectionCount.Multi)]
        public ValueConnectionKnob PrevNode;
        
        [ValueConnectionKnob("NextBuffType", Direction.Out, "NextNodeDatas", NodeSide.Right, 30)]
        public ValueConnectionKnob NextNode;

        public virtual void AutoAddLinkedBuffs()
        {
            
        }

        public override void NodeGUI()
        {
            EditorGUILayout.TextField("不允许使用此结点");
        }
    }
}