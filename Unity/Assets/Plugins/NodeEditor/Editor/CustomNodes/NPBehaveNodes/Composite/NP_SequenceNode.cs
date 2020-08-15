//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 17:30:16
//------------------------------------------------------------

using ETModel;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/Composite/Sequence", typeof(NPBehaveCanvas))]
    public class NP_SequenceNode : NP_CompositeNodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "序列结点";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        [BoxGroup("序列结点数据")] [HideReferenceObjectPicker] [HideLabel]
        public NP_SequenceNodeData NP_SequenceNodeData;

        private void OnEnable()
        {
            if (NP_SequenceNodeData == null)
            {
                this.NP_SequenceNodeData = new NP_SequenceNodeData {NodeType = NodeType.Composite};
                NP_SequenceNodeData.NodeDes = "序列组合器";
                this.backgroundColor = new Color(0f, 245 / 255f, 255 / 255f);
            }
        }

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_SequenceNodeData;
        }

        public override void NodeGUI()
        {
            NP_SequenceNodeData.NodeDes = EditorGUILayout.TextField(NP_SequenceNodeData.NodeDes);
        }
    }
}