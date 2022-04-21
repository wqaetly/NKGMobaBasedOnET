//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 8:18:19
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/Log", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/Log", typeof (SkillGraph))]
    public class NP_LogActionNode: NP_TaskNodeBase
    {
        public override string name => "Log节点";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_LogAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}