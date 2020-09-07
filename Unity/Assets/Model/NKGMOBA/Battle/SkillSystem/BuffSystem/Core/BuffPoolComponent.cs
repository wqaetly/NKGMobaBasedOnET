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
        public Dictionary<Type, Queue<ABuffSystemBase>> BuffSystemBases = new Dictionary<Type, Queue<ABuffSystemBase>>();

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="buffDataBase">Buff数据</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <typeparam name="T">要取得的具体Buff</typeparam>
        /// <returns></returns>
        public T AcquireBuff<T>(BuffDataBase buffDataBase, Unit theUnitFrom, Unit theUnitBelongTo) where T : ABuffSystemBase
        {
            Queue<ABuffSystemBase> buffBase;
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
        public ABuffSystemBase AcquireBuff(BuffDataBase buffDataBase, Unit theUnitFrom, Unit theUnitBelongTo)
        {
            Queue<ABuffSystemBase> buffBase;
            Type tempType = typeof (ABuffSystemBase);
            switch (buffDataBase.BelongBuffSystemType)
            {
                case BuffSystemType.FlashDamageBuffSystem:
                    tempType = typeof (FlashDamageABuffSystem);
                    break;
                case BuffSystemType.SustainDamageBuffSystem:
                    tempType = typeof (SustainDamageABuffSystem);
                    break;
                case BuffSystemType.ChangePropertyBuffSystem:
                    tempType = typeof (ChangePropertyABuffSystem);
                    break;
                case BuffSystemType.ListenBuffCallBackBuffSystem:
                    tempType = typeof (ListenABuffCallBackABuffSystem);
                    break;
                case BuffSystemType.BindStateBuffSystem:
                    tempType = typeof (BindStateABuffSystem);
                    break;
                case BuffSystemType.TreatmentBuffSystem:
                    tempType = typeof (TreatmentABuffSystem);
                    break;
                case BuffSystemType.PlayEffectBuffSystem:
                    tempType = typeof (PlayEffectABuffSystem);
                    break;
                case BuffSystemType.RefreshTargetBuffTimeBuffSystem:
                    tempType = typeof (RefreshTargetABuffTimeABuffSystem);
                    break;
                //TODO 如果要加新的Buff逻辑类型，需要在这里拓展，本人架构能力的确有限。。。
            }

            if (this.BuffSystemBases.TryGetValue(tempType, out buffBase))
            {
                if (buffBase.Count > 0)
                {
                    ABuffSystemBase tempABuffBase = buffBase.Dequeue();
                    tempABuffBase.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
                    return tempABuffBase;
                }
            }

            ABuffSystemBase temp = (ABuffSystemBase) Activator.CreateInstance(tempType);
            temp.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
            return temp;
        }

        public void RecycleBuff(ABuffSystemBase aBuffSystemBase)
        {
            if (this.BuffSystemBases.TryGetValue(aBuffSystemBase.GetType(), out Queue<ABuffSystemBase> temp))
            {
                temp.Enqueue(aBuffSystemBase);
            }
            else
            {
                this.BuffSystemBases.Add(aBuffSystemBase.GetType(), new Queue<ABuffSystemBase>());
                this.BuffSystemBases[aBuffSystemBase.GetType()].Enqueue(aBuffSystemBase);
            }
        }
    }
}