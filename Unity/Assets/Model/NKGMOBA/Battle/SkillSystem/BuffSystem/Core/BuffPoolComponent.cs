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
    /// Buff池组件
    /// </summary>
    public class BuffPoolComponent: Component
    {
        public Dictionary<Type, Queue<BuffSystemBase>> BuffSystemBases = new Dictionary<Type, Queue<BuffSystemBase>>();

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="buffDataBase">Buff数据</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <typeparam name="T">要取得的具体Buff</typeparam>
        /// <returns></returns>
        public T AcquireBuff<T>(BuffDataBase buffDataBase, Unit theUnitFrom, Unit theUnitBelongTo) where T : BuffSystemBase
        {
            Queue<BuffSystemBase> buffBase;
            if (this.BuffSystemBases.TryGetValue(typeof (T), out buffBase))
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
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="buffDataBase">Buff数据</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <returns></returns>
        public BuffSystemBase AcquireBuff(BuffDataBase buffDataBase, Unit theUnitFrom, Unit theUnitBelongTo)
        {
            Queue<BuffSystemBase> buffBase;
            Type tempType = typeof (BuffSystemBase);
            switch (buffDataBase.BelongBuffSystemType)
            {
                case BuffSystemType.FlashDamageBuffSystem:
                    tempType = typeof (FlashDamageBuffSystem);
                    break;
                case BuffSystemType.SustainDamageBuffSystem:
                    tempType = typeof (SustainDamageBuffSystem);
                    break;
                case BuffSystemType.ChangePropertyBuffSystem:
                    tempType = typeof (ChangePropertyBuffSystem);
                    break;
                case BuffSystemType.ListenBuffCallBackBuffSystem:
                    tempType = typeof (ListenBuffCallBackBuffSystem);
                    break;
                case BuffSystemType.BindStateBuffSystem:
                    tempType = typeof (BindStateBuffSystem);
                    break;
                case BuffSystemType.TreatmentBuffSystem:
                    tempType = typeof (TreatmentBuffSystem);
                    break;
                case BuffSystemType.PlayEffectBuffSystem:
                    tempType = typeof (PlayEffectBuffSystem);
                    break;
                case BuffSystemType.RefreshTargetBuffTimeBuffSystem:
                    tempType = typeof (RefreshTargetBuffTimeBuffSystem);
                    break;
                //TODO 如果要加新的Buff逻辑类型，需要在这里拓展，本人架构能力的确有限。。。
            }

            if (this.BuffSystemBases.TryGetValue(tempType, out buffBase))
            {
                if (buffBase.Count > 0)
                {
                    BuffSystemBase tempBuffBase = buffBase.Dequeue();
                    tempBuffBase.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
                    return tempBuffBase;
                }
            }

            BuffSystemBase temp = (BuffSystemBase) Activator.CreateInstance(tempType);
            temp.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
            return temp;
        }

        public void RecycleBuff(BuffSystemBase buffSystemBase)
        {
            if (this.BuffSystemBases.TryGetValue(buffSystemBase.GetType(), out Queue<BuffSystemBase> temp))
            {
                temp.Enqueue(buffSystemBase);
            }
            else
            {
                this.BuffSystemBases.Add(buffSystemBase.GetType(), new Queue<BuffSystemBase>());
                this.BuffSystemBases[buffSystemBase.GetType()].Enqueue(buffSystemBase);
            }
        }
    }
}