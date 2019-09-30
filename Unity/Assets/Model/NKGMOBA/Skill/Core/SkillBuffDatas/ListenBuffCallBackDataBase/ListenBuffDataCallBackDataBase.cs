//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月21日 10:09:03
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ILRuntime.Runtime;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 监听Buff事件数据基类，用以监听指定事件
    /// </summary>
    public class ListenBuffEventBase: AEvent<BuffSystemBase>
    {
        /// <summary>
        /// Buff回调条件达成时会添加的Buff
        /// </summary>
        [LabelText("Buff回调条件达成时会添加的Buff")]
        public List<BuffDataBase> m_BuffsWillBeAdded = new List<BuffDataBase>();

        public override void Run(BuffSystemBase a)
        {
            foreach (var VARIABLE in m_BuffsWillBeAdded)
            {
                a.theUnitFrom.GetComponent<BuffManagerComponent>()
                        .AddBuff(Game.Scene.GetComponent<BuffPoolComponent>().AcquireBuff(VARIABLE, a.theUnitFrom, a.theUnitBelongto));
            }
        }
    }

    /// <summary>
    /// 监听Buff事件数据基类，用以监听指定事件
    /// </summary>
    public class ListenBuffDataBase: BuffDataBase
    {
        [LabelText("事件ID标识")]
        public string EventId;

        /// <summary>
        /// Buff事件
        /// </summary>
        [LabelText("Buff回调条件达成时会添加的Buff")]
        public ListenBuffEventBase ListenBuffEventBase;
    }
}