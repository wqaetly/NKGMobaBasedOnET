//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:55:58
//------------------------------------------------------------

using NPBehave;

namespace ETModel
{
    public class NP_LogActionNodeData: NP_ActionNodeData
    {
        public override Action CreateAction()
        {
            this.m_Action = new Action(LogHelloWorld);
            return this.m_Action;
        }

        public override Node NP_GetNode()
        {
            return this.m_Action;
        }

        public void LogHelloWorld()
        {
            Log.Info("My Action Test：HelloWorld");
        }
    }
}