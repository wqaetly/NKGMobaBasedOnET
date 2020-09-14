//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月14日 21:48:24
//------------------------------------------------------------

using ETModel;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using UnityEditor;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/Decorator/BlackboardMultipleConditions", typeof (NPBehaveCanvas))]
    public class NP_BlackboardMultipleConditionsNode: NP_DecoratorNodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "黑板多条件节点";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        public NP_BlackboardMultipleConditionsNodeData NP_BlackboardMultipleConditionsNodeData =
                new NP_BlackboardMultipleConditionsNodeData { NodeType = NodeType.Decorator, NodeDes = "黑板多条件节点" };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_BlackboardMultipleConditionsNodeData;
        }

        public override void NodeGUI()
        {
            NP_BlackboardMultipleConditionsNodeData.NodeDes = EditorGUILayout.TextField(NP_BlackboardMultipleConditionsNodeData.NodeDes);
        }
    }
}