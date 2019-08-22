//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:11:32
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NP_SequenceNodeData: NP_NodeDataBase
    {
        [LabelText("队列结点")]
        public Sequence mSequenceNode;

        public override Node NP_GetNode()
        {
            return this.mSequenceNode;
        }
    }
}