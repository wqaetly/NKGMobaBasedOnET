//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/检查技能能否释放", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/检查技能能否释放", typeof (SkillGraph))]
    public class NP_CheckSkillCanBeCastActionNode: NP_TaskNodeBase
    {
        public override string name => "检查技能能否释放";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_CheckSkillCanBeCastAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}