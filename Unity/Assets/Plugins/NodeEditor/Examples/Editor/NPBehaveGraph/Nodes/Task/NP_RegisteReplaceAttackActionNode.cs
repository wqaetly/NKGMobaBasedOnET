//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/替换攻击流程Action", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/替换攻击流程Action", typeof (SkillGraph))]
    public class NP_RegisteReplaceAttackActionNode: NP_TaskNodeBase
    {
        public override string name => "替换攻击流程Action";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_RegisteReplaceAttackAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}