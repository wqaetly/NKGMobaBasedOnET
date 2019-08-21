//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:12:40
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NP_BlackboardQueryNodeData: NP_NodeDataBase
    {
        [LabelText("黑板查询结点")]
        public BlackboardQuery mBlackboardQueryNode;

        public override Node GetNPBehaveNode()
        {
            return this.mBlackboardQueryNode;
        }

        public override void AutoSetNodeData(List<Node>  nextNode)
        {
            throw new System.NotImplementedException();
        }

    }
}