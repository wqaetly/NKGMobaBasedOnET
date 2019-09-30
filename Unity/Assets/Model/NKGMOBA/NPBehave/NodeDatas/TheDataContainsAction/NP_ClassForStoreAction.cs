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

        public Func<bool> m_Func1;

        public Func<bool, NPBehave.Action.Result> m_Func2;

        public virtual Action GetActionToBeDone()
        {
            return null;
        }

        public virtual Func<bool> GetFunc1ToBeDone()
        {
            return null;
        }

        public virtual Func<bool, NPBehave.Action.Result> GetFunc2ToBeDone()
        {
            return null;
        }

        public NPBehave.Action _CreateNPBehaveAction()
        {
            GetActionToBeDone();
            GetFunc1ToBeDone();
            GetFunc2ToBeDone();
            if (m_Action != null)
            {
                return new NPBehave.Action(this.m_Action);
            }

            if (m_Func1 != null)
            {
                return new NPBehave.Action(m_Func1);
            }

            if (m_Func2 != null)
            {
                return new NPBehave.Action(m_Func2);
            }

            Log.Info("_CreateNPBehaveAction失败，因为没有找到可以绑定的委托");
            return null;
        }
    }
}