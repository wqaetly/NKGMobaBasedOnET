//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 19:47:29
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    /// <summary>
    /// 等待一个可变化的时间，用于处理突如其来的CD变化
    /// </summary>
    [NodeMenuItem("NPBehave行为树/Task/等待一个可变化的时间", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/等待一个可变化的时间", typeof (SkillGraph))]
    public class NP_WaitAChangeableTimeActionNode: NP_TaskNodeBase
    {
        public override string name => "等待一个可变化的时间";

        public NP_ActionNodeData NP_ActionNodeData =
                new NP_ActionNodeData() { NpClassForStoreAction = new NP_WaitAChangeableTimeAction() };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ActionNodeData;
        }
    }
}