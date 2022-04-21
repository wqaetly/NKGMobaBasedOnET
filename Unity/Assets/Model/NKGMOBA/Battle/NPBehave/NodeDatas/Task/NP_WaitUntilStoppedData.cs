//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月26日 18:08:42
//------------------------------------------------------------

using NPBehave;
using Sirenix.OdinInspector;

namespace ET
{
    [BoxGroup("等待到停止节点数据")]
    [HideLabel]
    public class NP_WaitUntilStoppedData: NP_NodeDataBase
    {
        [HideInEditorMode]
        private WaitUntilStopped m_WaitUntilStopped;

        public override Node NP_GetNode()
        {
            return this.m_WaitUntilStopped;
        }

        public override Task CreateTask(Unit unit, NP_RuntimeTree runtimeTree)
        {
            this.m_WaitUntilStopped = new WaitUntilStopped();
            return this.m_WaitUntilStopped;
        }
    }
}