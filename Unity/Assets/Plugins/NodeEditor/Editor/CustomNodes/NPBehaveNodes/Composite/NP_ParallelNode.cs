//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月7日 12:30:38
//------------------------------------------------------------

using ETModel;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/Composite/Parallel", typeof(NPBehaveCanvas))]
    public class NP_ParallelNode : NP_CompositeNodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "并行节点";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        [BoxGroup("并行结点数据")] [HideReferenceObjectPicker] [HideLabel]
        public NP_ParallelNodeData NP_ParallelNodeData;

        private void OnEnable()
        {
            if (NP_ParallelNodeData == null)
            {
                this.NP_ParallelNodeData = new NP_ParallelNodeData {NodeType = NodeType.Composite};
                NP_ParallelNodeData.NodeDes = "并行组合器";
                backgroundColor = new Color(205 / 255f, 92 / 255f, 92 / 255f);
            }
        }

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ParallelNodeData;
        }

        public override void NodeGUI()
        {
            NP_ParallelNodeData.NodeDes = EditorGUILayout.TextField(NP_ParallelNodeData.NodeDes);
        }
    }
}