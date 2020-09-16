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
        public Dictionary<Type, Queue<ABuffSystemBase>> BuffSystems = new Dictionary<Type, Queue<ABuffSystemBase>>();

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="dataId">Buff数据归属的数据块Id</param>
        /// <param name="buffId">Buff的Id</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <returns></returns>
        public ABuffSystemBase AcquireBuff(long dataId, long buffId, Unit theUnitFrom, Unit theUnitBelongTo)
        {
            BuffDataBase buffDataBase =
                    (Game.Scene.GetComponent<NP_TreeDataRepository>().GetNP_TreeData(dataId).BuffDataDic[buffId] as NormalBuffNodeData).BuffData;

            return AcquireBuff(buffDataBase, theUnitFrom, theUnitBelongTo);
        }

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="npDataSupportor">Buff数据归属的数据块</param>
        /// <param name="buffId">Buff的Id</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <returns></returns>
        public ABuffSystemBase AcquireBuff(NP_DataSupportor npDataSupportor, long buffId, Unit theUnitFrom, Unit theUnitBelongTo)
        {
            BuffDataBase buffDataBase = (npDataSupportor.BuffDataDic[buffId] as NormalBuffNodeData).BuffData;
            return AcquireBuff(buffDataBase, theUnitFrom, theUnitBelongTo);
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
                    tempType = typeof (FlashDamageBuffSystem);
                    break;
                case BuffSystemType.SustainDamageBuffSystem:
                    tempType = typeof (SustainDamageBuffSystem);
                    break;
                case BuffSystemType.ChangePropertyBuffSystem:
                    tempType = typeof (ChangePropertyBuffSystem);
                    break;
                case BuffSystemType.ListenBuffCallBackBuffSystem:
                    tempType = typeof (ListenABuffCallBackBuffSystem);
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
                    tempType = typeof (RefreshTargetABuffTimeBuffSystem);
                    break;
                //TODO 如果要加新的Buff逻辑类型，需要在这里拓展，本人架构能力的确有限。。。
            }

            ABuffSystemBase resultBuff;
            if (this.BuffSystems.TryGetValue(tempType, out buffBase))
            {
                if (buffBase.Count > 0)
                {
                    resultBuff = buffBase.Dequeue();
                    resultBuff.BelongToBuffDataSupportorId = buffDataBase.BuffId;
                    resultBuff.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
                    return resultBuff;
                }
            }

            resultBuff = (ABuffSystemBase) Activator.CreateInstance(tempType);
            resultBuff.BelongToBuffDataSupportorId = buffDataBase.BuffId;
            resultBuff.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
            return resultBuff;
        }

        public void RecycleBuff(ABuffSystemBase aBuffSystemBase)
        {
            if (this.BuffSystems.TryGetValue(aBuffSystemBase.GetType(), out Queue<ABuffSystemBase> temp))
            {
                temp.Enqueue(aBuffSystemBase);
            }
            else
            {
                this.BuffSystems.Add(aBuffSystemBase.GetType(), new Queue<ABuffSystemBase>());
                this.BuffSystems[aBuffSystemBase.GetType()].Enqueue(aBuffSystemBase);
            }
        }
    }
}