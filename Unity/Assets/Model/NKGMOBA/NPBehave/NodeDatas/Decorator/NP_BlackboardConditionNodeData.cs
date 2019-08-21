//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:12:13
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NP_BlackboardConditionNodeData:NP_NodeDataBase
    {
        [LabelText("黑板条件结点")]
        public BlackboardCondition mBlackboardConditionNode;
        
        public override Node GetNPBehaveNode()
        {
            return this.mBlackboardConditionNode;
        }

        public override void AutoSetNodeData(List<Node>  nextNode)
        {
            throw new System.NotImplementedException();
        }
    }
}