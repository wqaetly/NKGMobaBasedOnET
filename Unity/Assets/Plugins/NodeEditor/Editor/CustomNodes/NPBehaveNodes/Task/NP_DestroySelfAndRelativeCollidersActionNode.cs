//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Node = NPBehave.Node;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树/Task/销毁自己和相关联的Collider", typeof(NPBehaveCanvas))]
    public class NP_DestroySelfAndRelativeCollidersActionNode : NP_TaskNodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "销毁自己和相关联的Collider";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;
        
        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NodeType = NodeType.Task, NpClassForStoreAction = new NP_DestroySelfAndRelativeCollidersAction() };

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
