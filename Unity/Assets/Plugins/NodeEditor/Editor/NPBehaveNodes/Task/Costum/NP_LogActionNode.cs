//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 8:18:19
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
    [Node(false, "NPBehave行为树/Task/LogAction", typeof (NPBehaveCanvas))]
    public class NP_LogActionNode: NP_NodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "行为节点";

        /// <summary>
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        [LabelText("Log结点数据")]
        public NP_LogActionNodeData NP_LogActionNodeData;

        private void OnEnable()
        {
            if (NP_LogActionNodeData == null)
            {
                this.NP_LogActionNodeData = new NP_LogActionNodeData();
            }

            NP_LogActionNodeData.NodeDes = "Log节点，打印数据";
        }

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_LogActionNodeData;
        }

        public override void NodeGUI()
        {
            EditorGUILayout.TextField(NP_LogActionNodeData.NodeDes);
            EditorGUILayout.TextField($"优先级：{NP_LogActionNodeData.priority}");
        }
    }
}