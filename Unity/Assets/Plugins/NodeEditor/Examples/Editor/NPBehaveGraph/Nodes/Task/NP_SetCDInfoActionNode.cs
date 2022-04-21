//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using System.Collections.Generic;
using ET;
using GraphProcessor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Node = NPBehave.Node;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/设置CD信息", typeof(SkillGraph))]
    [NodeMenuItem("NPBehave行为树/Task/设置CD信息", typeof(NPBehaveGraph))]
    public class NP_SetCDInfoActionNode : NP_TaskNodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        public override string name => "设置CD信息";
        
        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() {NpClassForStoreAction = new NP_SetCDInfoAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}
