//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 22:28:26
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// Buff池组件,包括Buff的事件池化
    /// </summary>
    public class BuffPoolComponent: Component
    {
        public Dictionary<Type, Queue<BuffSystemBase>> MBuffBases = new Dictionary<Type, Queue<BuffSystemBase>>();

        /// <summary>
        /// 取得Buff
        /// </summary>
        /// <param name="buffDataBase">Buff数据</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <typeparam name="T">要取得的具体Buff</typeparam>
        /// <returns></returns>
        public T AcquireBuff<T>(BuffDataBase buffDataBase, Unit theUnitFrom, Unit theUnitBelongTo) where T : BuffSystemBase
        {
            Queue<BuffSystemBase> buffBase;
            if (this.MBuffBases.TryGetValue(typeof (T), out buffBase))
            {
                if (buffBase.Count > 0)
                {
                    T tempBuffBase = (T) buffBase.Dequeue();
                    tempBuffBase.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
                    return tempBuffBase;
                }
            }

            T temp = (T) Activator.CreateInstance(typeof (T));
            temp.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
            return temp;
        }

        /// <summary>
        /// 取得Buff
        /// </summary>
        /// <param name="buffDataBase">Buff数据</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <returns></returns>
        public BuffSystemBase AcquireBuff(BuffDataBase buffDataBase, Unit theUnitFrom, Unit theUnitBelongTo)
        {
            Queue<BuffSystemBase> buffBase;
            if (this.MBuffBases.TryGetValue(buffDataBase.BelongBuffSystemType, out buffBase))
            {
                if (buffBase.Count > 0)
                {
                    BuffSystemBase tempBuffBase = buffBase.Dequeue();
                    tempBuffBase.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
                    return tempBuffBase;
                }
            }

            BuffSystemBase temp = (BuffSystemBase) Activator.CreateInstance(buffDataBase.BelongBuffSystemType);
            temp.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
            return temp;
        }

        public void RecycleBuff(BuffSystemBase buffBase)
        {
            Type type = buffBase.GetType();
            if (this.MBuffBases.ContainsKey(type))
            {
                MBuffBases[type].Enqueue(buffBase);
            }
            else
            {
                Queue<BuffSystemBase> temp = new Queue<BuffSystemBase>();
                temp.Enqueue(buffBase);
                this.MBuffBases.Add(type, temp);
            }
        }
    }
}