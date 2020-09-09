//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using ETModel;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using UnityEditor;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/Task/播放特效", typeof(NPBehaveCanvas))]
    public class NP_PlayParticalActionNode : NP_TaskNodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "播放特效";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;
        
        public NP_ActionNodeData NP_ActionNodeData = new NP_ActionNodeData() { NpClassForStoreAction = new NP_PlayParticalAction() };

        private void OnEnable()
        {
            if (NP_ActionNodeData == null)
            {
                this.NP_ActionNodeData = new NP_ActionNodeData {NodeType = NodeType.Task};
            }
        }

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }

        public override void NodeGUI()
        {
            NP_ActionNodeData.NodeDes = EditorGUILayout.TextField(NP_ActionNodeData.NodeDes);
        }
    }
}
