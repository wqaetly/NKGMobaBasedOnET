//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 22:28:26
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// Buff工厂
    /// </summary>
    public static class BuffFactory
    {
        /// <summary>
        /// 记录所有BuffSystem类型，用于运行时创建对应的BuffSystem
        /// </summary>
        public static Dictionary<Type, Type> AllBuffSystemTypes = new Dictionary<Type, Type>()
        {
            {typeof(ChangePropertyBuffData), typeof(ChangePropertyBuffSystem)},
            {typeof(ListenBuffCallBackBuffData), typeof(ListenBuffCallBackBuffSystem)},
            {typeof(BindStateBuffData), typeof(BindStateBuffSystem)},
            {typeof(PlayEffectBuffData), typeof(PlayEffectBuffSystem)},
            {typeof(ReplaceAnimBuffData), typeof(ReplaceAnimBuffSystem)},
            {typeof(ChangeMaterialBuffData), typeof(ChangeMaterialBuffSystem)},

            {typeof(RefreshTargetBuffTimeBuffData), typeof(RefreshTargetBuffTimeBuffSystem)},
            {typeof(FlashDamageBuffData), typeof(FlashDamageBuffSystem)},
            {typeof(SustainDamageBuffData), typeof(SustainDamageBuffSystem)},
            {typeof(TreatmentBuffData), typeof(TreatmentBuffSystem)},
            {typeof(ReplaceAttackBuffData), typeof(ReplaceAttackBuffSystem)}
        };

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="room"></param>
        /// <param name="npsupportDataId">Buff数据归属的数据块Id</param>
        /// <param name="buffNodeId">Buff节点的Id</param>
        /// <param name="theUnitIdFrom">Buff来源者</param>
        /// <param name="theUnitIdBelongTo">Buff寄生者</param>
        /// <param name="theSkillCanvasIdBelongTo"></param>
        /// <returns></returns>
        public static IBuffSystem AcquireBuff(Room room, long npsupportDataId, long buffNodeId, long theUnitIdFrom,
            long theUnitIdBelongTo,
            long theSkillCanvasIdBelongTo)
        {
            UnitComponent unitComponent = room.GetComponent<UnitComponent>();
            Unit theUnitFrom = unitComponent.Get(theUnitIdFrom);
            Unit theUnitBelongTo = unitComponent.Get(theUnitIdBelongTo);

            NP_RuntimeTree npRuntimeTree = theUnitFrom.GetComponent<NP_RuntimeTreeManager>()
                .GetTreeByRuntimeID(theSkillCanvasIdBelongTo);

            return AcquireBuff(
                (theUnitFrom.DomainScene().GetComponent<NP_TreeDataRepositoryComponent>()
                    .GetNP_TreeData(npsupportDataId)
                    .BuffNodeDataDic[buffNodeId] as NormalBuffNodeData)?.BuffData, buffNodeId,
                theUnitFrom, theUnitBelongTo, npRuntimeTree);
        }

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="dataId">Buff数据归属的数据块Id</param>
        /// <param name="buffNodeId">Buff节点的Id</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <param name="theSkillCanvasBelongTo"></param>
        /// <returns></returns>
        public static IBuffSystem AcquireBuff(long dataId, long buffNodeId, Unit theUnitFrom, Unit theUnitBelongTo,
            NP_RuntimeTree theSkillCanvasBelongTo)
        {
            return AcquireBuff(
                (theUnitFrom.DomainScene().GetComponent<NP_TreeDataRepositoryComponent>().GetNP_TreeData(dataId)
                    .BuffNodeDataDic[buffNodeId] as NormalBuffNodeData)?.BuffData, buffNodeId,
                theUnitFrom, theUnitBelongTo, theSkillCanvasBelongTo);
        }

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="npDataSupportor">Buff数据归属的数据块</param>
        /// <param name="buffNodeId">Buff节点的Id</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <returns></returns>
        public static IBuffSystem AcquireBuff(NP_DataSupportor npDataSupportor, long buffNodeId, Unit theUnitFrom,
            Unit theUnitBelongTo,
            NP_RuntimeTree theSkillCanvasBelongTo)
        {
            return AcquireBuff((npDataSupportor.BuffNodeDataDic[buffNodeId] as NormalBuffNodeData)?.BuffData,
                buffNodeId,
                theUnitFrom, theUnitBelongTo,
                theSkillCanvasBelongTo);
        }

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="buffDataBase">Buff数据</param>
        /// <param name="buffNodeId"></param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <param name="theSkillCanvasBelongTo"></param>
        /// <returns></returns>
        public static IBuffSystem AcquireBuff(BuffDataBase buffDataBase, long buffNodeId, Unit theUnitFrom,
            Unit theUnitBelongTo,
            NP_RuntimeTree theSkillCanvasBelongTo)
        {
            LSF_Component lsfComponent = theUnitFrom.BelongToRoom.GetComponent<LSF_Component>();
            IBuffSystem resultBuff = ReferencePool.Acquire(AllBuffSystemTypes[buffDataBase.GetType()]) as IBuffSystem;
            resultBuff.BelongtoRuntimeTree = theSkillCanvasBelongTo;
            resultBuff.BuffNodeId = buffNodeId;
            resultBuff.Init(buffDataBase, theUnitFrom, theUnitBelongTo, lsfComponent.CurrentFrame);
            return resultBuff;
        }

        /// <summary>
        /// 回收一个Buff
        /// </summary>
        /// <param name="aBuffSystemBase"></param>
        public static void RecycleBuff<T>(ABuffSystemBase<T> aBuffSystemBase) where T : BuffDataBase
        {
            ReferencePool.Release(aBuffSystemBase);
        }
    }
}