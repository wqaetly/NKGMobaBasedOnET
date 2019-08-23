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

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/Composite/Sequence", typeof (NPBehaveCanvas))]
    public class NP_SequenceNode: NP_NodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "队列结点";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        [LabelText("Log结点数据")]
        public NP_ServiceNodeData NP_ServiceNodeData;

        private void OnEnable()
        {
            if (NP_ServiceNodeData == null)
            {
                this.NP_ServiceNodeData = new NP_ServiceNodeData();
            }

            NP_ServiceNodeData.NodeDes = "队列结点，一遇到失败即失败";
        }

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ServiceNodeData;
        }

        public override void NodeGUI()
        {
            EditorGUILayout.TextField(NP_ServiceNodeData.NodeDes);
            EditorGUILayout.TextField($"优先级：{NP_ServiceNodeData.priority}");
        }
    }
}