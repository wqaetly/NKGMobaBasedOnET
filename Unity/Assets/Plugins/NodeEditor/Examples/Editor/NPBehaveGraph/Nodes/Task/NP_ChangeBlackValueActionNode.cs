//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/修改黑板值", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/修改黑板值", typeof (SkillGraph))]
    public class NP_ChangeBlackValueActionNode: NP_TaskNodeBase
    {
        public override string name => "修改黑板值";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_ChangeBlackValueAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}