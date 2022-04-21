//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月26日 18:12:50
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Task/WaitUntilStopped", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/WaitUntilStopped", typeof (SkillGraph))]
    public class NP_WaitUntilStoppedNode: NP_TaskNodeBase
    {
        public override string name => "一直等待，直到Stopped";

        public NP_WaitUntilStoppedData NpWaitUntilStoppedData = new NP_WaitUntilStoppedData { NodeDes = "阻止轮询，提高效率" };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NpWaitUntilStoppedData;
        }
    }
}