//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 21:10:35
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;


namespace ETModel
{
    [BoxGroup("用于包含Action的数据类"),GUIColor(0.961f, 0.902f, 0.788f, 1f)]
    [HideLabel]
    public class NP_ClassForStoreAction
    {
        /// <summary>
        /// 归属的UnitID
        /// </summary>
        [HideInEditorMode]
        public long Unitid;

        /// <summary>
        /// 归属的运行时行为树实例
        /// </summary>
        [HideInEditorMode]
        public NP_RuntimeTree BelongtoRuntimeTree;

        [HideInEditorMode]
        public Action Action;

        [HideInEditorMode]
        public Func<bool> Func1;

        [HideInEditorMode]
        public Func<bool, NPBehave.Action.Result> Func2;

        /// <summary>
        /// 获取将要执行的委托函数，也可以在这里面做一些初始化操作
        /// </summary>
        /// <returns></returns>
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
            if (this.Action != null)
            {
                return new NPBehave.Action(this.Action);
            }

            GetFunc1ToBeDone();
            if (this.Func1 != null)
            {
                return new NPBehave.Action(this.Func1);
            }

            GetFunc2ToBeDone();
            if (this.Func2 != null)
            {
                return new NPBehave.Action(this.Func2);
            }

            Log.Info("_CreateNPBehaveAction失败，因为没有找到可以绑定的委托");
            return null;
        }
    }
}