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
        /// <summary>
        /// 归属的UnitID
        /// </summary>
        public long Unitid;

        /// <summary>
        /// 归属的运行时行为树id
        /// </summary>
        public long RuntimeTreeID;
        
        public Action m_Action;

        public virtual Action GetActionToBeDone()
        {
            return null;
        }
    }
}