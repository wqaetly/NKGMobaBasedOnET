//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/检查目标Unit是否被指定方式杀死", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/检查目标Unit是否被指定方式杀死", typeof (SkillGraph))]
    public class NP_CheckUnitKilledBySpecialMethodActionNode: NP_TaskNodeBase
    {
        public override string name => "检查目标Unit是否被指定方式杀死";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_CheckUnitKilledBySpecialMethodAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}