//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月13日 20:28:02
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/Wait", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/Wait", typeof (SkillGraph))]
    public class NP_WaitNode: NP_TaskNodeBase
    {
        public override string name => "等待节点";

        public NP_WaitNodeData NP_WaitNodeData = new NP_WaitNodeData { };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_WaitNodeData;
        }
    }
}