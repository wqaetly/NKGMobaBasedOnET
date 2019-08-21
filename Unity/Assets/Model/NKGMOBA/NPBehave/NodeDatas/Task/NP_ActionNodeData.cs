//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:13:30
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NP_ActionNodeData:NP_NodeDataBase
    {
        [LabelText("行为结点")]
        public Action mActionNode;

        public override Node GetNPBehaveNode()
        {
            if (this.mActionNode==null)
            {
                this.mActionNode = new Action();
            }
            return this.mActionNode;
        }

        public override void AutoSetNodeData(List<Node>  nextNode)
        {
            throw new System.NotImplementedException();
        }
    }
}