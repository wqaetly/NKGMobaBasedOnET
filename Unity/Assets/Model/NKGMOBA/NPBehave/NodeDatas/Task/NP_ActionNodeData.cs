//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:13:30
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel.TheDataContainsAction;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NP_ActionNodeData:NP_NodeDataBase
    {
        public Action m_ActionNode;

        [LabelText("承载Action的数据结构")]
        public NP_ClassForStoreAction MNpClassForStoreAction;
        
        public override Task CreateTask()
        {
            this.m_ActionNode = new Action(MNpClassForStoreAction.GetActionToBeDone());
            return this.m_ActionNode;
        }

        public override Node NP_GetNode()
        {
            return m_ActionNode;
        }
    }
}