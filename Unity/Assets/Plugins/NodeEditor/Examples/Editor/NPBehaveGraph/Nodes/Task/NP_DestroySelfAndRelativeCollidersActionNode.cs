//------------------------------------------------------------
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
// 代码由工具自动生成，请勿手动修改
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/销毁自己和相关联的Collider", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/销毁自己和相关联的Collider", typeof (SkillGraph))]
    public class NP_DestroySelfAndRelativeCollidersActionNode: NP_TaskNodeBase
    {
        public override string name => "销毁自己和相关联的Collider";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_DestroySelfAndRelativeCollidersAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}