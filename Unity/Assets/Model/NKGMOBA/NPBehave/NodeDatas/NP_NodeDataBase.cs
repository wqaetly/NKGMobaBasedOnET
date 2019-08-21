//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:14:44
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    public abstract class NP_NodeDataBase
    {
        [LabelText("结点信息描述")]
        public string NodeDes;
        
        public abstract Node GetNPBehaveNode();

        public abstract void AutoSetNodeData(List<Node> nextNodes);
        
        public virtual void AutoBindAllDelegate()
        {
        }
    }
}