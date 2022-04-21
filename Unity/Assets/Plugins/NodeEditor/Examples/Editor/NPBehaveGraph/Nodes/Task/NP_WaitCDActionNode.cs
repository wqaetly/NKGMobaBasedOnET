//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using System.Collections.Generic;
using ET;
using Sirenix.OdinInspector;
using UnityEditor;
using GraphProcessor;
using UnityEngine;
using Node = NPBehave.Node;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/等待CD", typeof(SkillGraph))]
    [NodeMenuItem("NPBehave行为树/Task/等待CD", typeof(NPBehaveGraph))]
    public class NP_WaitCDActionNode : NP_TaskNodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        public override string name => "等待CD";
        
        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_WaitCDAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}
