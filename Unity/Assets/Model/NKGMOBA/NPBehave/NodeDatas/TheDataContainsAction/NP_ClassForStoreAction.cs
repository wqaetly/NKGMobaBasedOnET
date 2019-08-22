//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 21:10:35
//------------------------------------------------------------

using System;

namespace ETModel.TheDataContainsAction
{
    public class NP_ClassForStoreAction
    {
        public Action m_Action;

        public virtual Action GetActionToBeDone()
        {
            return null;
        }
    }
}