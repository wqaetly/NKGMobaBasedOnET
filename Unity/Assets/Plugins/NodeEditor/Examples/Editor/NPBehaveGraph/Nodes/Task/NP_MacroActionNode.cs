//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/宏行为节点", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/宏行为节点", typeof (SkillGraph))]
    public class NP_MacroActionNode: NP_TaskNodeBase
    {
        public override string name => "宏行为节点";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_MacroAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}