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
        /// <summary>
        /// 记录所有BuffSystem类型，用于运行时创建对应的BuffSystem
        /// </summary>
        public static Dictionary<BuffSystemType, Type> AllBuffSystemTypes = new Dictionary<BuffSystemType, Type>()
        {
            //TODO 如果要加新的Buff逻辑类型，需要在这里拓展，本人架构能力的确有限。。。
            { BuffSystemType.FlashDamageBuffSystem, typeof (FlashDamageBuffSystem) },
            { BuffSystemType.SustainDamageBuffSystem, typeof (SustainDamageBuffSystem) },
            { BuffSystemType.ChangePropertyBuffSystem, typeof (ChangePropertyBuffSystem) },
            { BuffSystemType.ListenBuffCallBackBuffSystem, typeof (ListenBuffCallBackBuffSystem) },
            { BuffSystemType.BindStateBuffSystem, typeof (BindStateBuffSystem) },
            { BuffSystemType.TreatmentBuffSystem, typeof (TreatmentBuffSystem) },
            { BuffSystemType.RefreshTargetBuffTimeBuffSystem, typeof (RefreshTargetBuffTimeBuffSystem) },
        };

        /// <summary>
        /// BuffPool中所有BuffSystem
        /// </summary>
        public Dictionary<Type, Queue<ABuffSystemBase>> BuffSystems = new Dictionary<Type, Queue<ABuffSystemBase>>();

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="dataId">Buff数据归属的数据块Id</param>
        /// <param name="buffNodeId">Buff节点的Id</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <returns></returns>
        public ABuffSystemBase AcquireBuff(long dataId, long buffNodeId, Unit theUnitFrom, Unit theUnitBelongTo, NP_RuntimeTree theSkillCanvasBelongTo)
        {
            return AcquireBuff(
                (Game.Scene.GetComponent<NP_TreeDataRepository>().GetNP_TreeData(dataId).BuffNodeDataDic[buffNodeId] as NormalBuffNodeData).BuffData,
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
        public ABuffSystemBase AcquireBuff(NP_DataSupportor npDataSupportor, long buffNodeId, Unit theUnitFrom, Unit theUnitBelongTo,
        NP_RuntimeTree theSkillCanvasBelongTo)
        {
            return AcquireBuff((npDataSupportor.BuffNodeDataDic[buffNodeId] as NormalBuffNodeData).BuffData, theUnitFrom, theUnitBelongTo,
                theSkillCanvasBelongTo);
        }

        /// <summary>
        /// 取得Buff,Buff流程是Acquire->OnInit(CalculateTimerAndOverlay)->AddTemp->经过筛选->AddReal
        /// </summary>
        /// <param name="buffDataBase">Buff数据</param>
        /// <param name="theUnitFrom">Buff来源者</param>
        /// <param name="theUnitBelongTo">Buff寄生者</param>
        /// <returns></returns>
        public ABuffSystemBase AcquireBuff(BuffDataBase buffDataBase, Unit theUnitFrom, Unit theUnitBelongTo, NP_RuntimeTree theSkillCanvasBelongTo)
        {
            Queue<ABuffSystemBase> buffBase;
            Type targetBuffSystemType = AllBuffSystemTypes[buffDataBase.BelongBuffSystemType];
            ABuffSystemBase resultBuff;
            if (this.BuffSystems.TryGetValue(targetBuffSystemType, out buffBase))
            {
                if (buffBase.Count > 0)
                {
                    resultBuff = buffBase.Dequeue();
                    resultBuff.BelongtoRuntimeTree = theSkillCanvasBelongTo;
                    resultBuff.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
                    return resultBuff;
                }
            }

            resultBuff = (ABuffSystemBase) Activator.CreateInstance(targetBuffSystemType);
            resultBuff.BelongtoRuntimeTree = theSkillCanvasBelongTo;
            resultBuff.OnInit(buffDataBase, theUnitFrom, theUnitBelongTo);
            return resultBuff;
        }

        /// <summary>
        /// 回收一个Buff
        /// </summary>
        /// <param name="aBuffSystemBase"></param>
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