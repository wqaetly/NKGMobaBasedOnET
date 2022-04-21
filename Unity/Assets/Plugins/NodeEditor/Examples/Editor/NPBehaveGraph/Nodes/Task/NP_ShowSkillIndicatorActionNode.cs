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
    [NodeMenuItem("NPBehave行为树/Task/显示技能指示器", typeof(SkillGraph))]
    [NodeMenuItem("NPBehave行为树/Task/显示技能指示器", typeof(NPBehaveGraph))]
    public class NP_ShowSkillIndicatorActionNode : NP_TaskNodeBase
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        public override string name => "显示技能指示器";
        
        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_ShowSkillIndicatorAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}
