//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月26日 18:12:50
//------------------------------------------------------------

using ETModel;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/Task/WaitUntilStopped", typeof (NPBehaveCanvas))]
    public class NP_WaitUntilStoppedNode: NP_TaskNodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "一直等待，直到Stopped";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        public NP_WaitUntilStoppedData NpWaitUntilStoppedData = new NP_WaitUntilStoppedData { NodeType = NodeType.Task, NodeDes = "阻止轮询，提高效率" };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NpWaitUntilStoppedData;
        }

        public override void NodeGUI()
        {
            NpWaitUntilStoppedData.NodeDes = EditorGUILayout.TextField(NpWaitUntilStoppedData.NodeDes);
        }
    }
}