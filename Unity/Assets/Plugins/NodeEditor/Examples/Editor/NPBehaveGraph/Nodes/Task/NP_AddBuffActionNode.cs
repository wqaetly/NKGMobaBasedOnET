//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/添加BuffAction", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/添加BuffAction", typeof (SkillGraph))]
    public class NP_AddBuffActionNode: NP_TaskNodeBase
    {
        public override string name => "添加BuffAction";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_AddBuffAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}