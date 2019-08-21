//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:55:58
//------------------------------------------------------------

namespace ETModel
{
    public class NP_LogActionNodeData: NP_ActionNodeData
    {
        public override void AutoBindAllDelegate()
        {
            this.mActionNode.action += this.LogHelloWorld;
        }

        public void LogHelloWorld()
        {
            Log.Info("My Action Test：HelloWorld");
        }
    }
}