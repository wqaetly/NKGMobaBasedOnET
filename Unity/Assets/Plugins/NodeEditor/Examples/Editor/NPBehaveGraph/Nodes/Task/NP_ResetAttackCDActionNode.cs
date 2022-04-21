//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/重置普攻CD", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/重置普攻CD", typeof (SkillGraph))]
    public class NP_ResetAttackCDActionNode: NP_TaskNodeBase
    {
        public override string name => "重置普攻CD";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_ResetAttackCDAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}