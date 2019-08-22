//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 10:42:35
//------------------------------------------------------------

using NPBehave;

namespace ETModel
{
    public class NP_NodeFactory
    {
        public static Action CreateActionNode(System.Action action)
        {
            return new Action(action);
        }
        
        public static Root CreateRootNode(Action action)
        {
            return new Root(action);
        }
    }
}