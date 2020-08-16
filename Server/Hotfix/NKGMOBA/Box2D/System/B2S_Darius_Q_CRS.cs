//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019/8/14 16:22:55
// Description: 此代码switch case与System部分由工具生成，请勿进行增减操作
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [Event(EventIdType_Collision.B2S_Darius_Q_CRS)]
    public class AddB2S_Darius_Q_CRSSystem: AEvent<Entity>
    {
        public override void Run(Entity a)
        {
            a.AddComponent<B2S_Darius_Q_CRS>();
        }
    }

    [ObjectSystem]
    public class B2S_Darius_Q_CRSAwakeSystem: AwakeSystem<B2S_Darius_Q_CRS>
    {
        public override void Awake(B2S_Darius_Q_CRS self)
        {
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideStartAction += self.OnCollideStart;
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideSustainAction += self.OnCollideSustain;
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideFinishAction += self.OnCollideFinish;
        }
    }

    public class B2S_Darius_Q_CRS: Component
    {
        /// <summary>
        /// 当发生碰撞时
        /// </summary>
        /// <param name="b2SColliderEntity">碰撞到的对象数据集合</param>
        public void OnCollideStart(B2S_ColliderEntity b2SColliderEntity)
        {
            //Log.Info("诺克Q技能打到了东西");
            switch (b2SColliderEntity.m_B2S_CollisionInstance.nodeDataId)
            {
                case 10006: //诺克：自身
                    //TODO:这一步需要在结点编辑器配好支持自动升成
                    if (b2SColliderEntity.m_BelongUnit.GetComponent<B2S_RoleCastComponent>().RoleCast != RoleCast.Adverse) return;
                    Stopwatch sw = new Stopwatch();

                    sw.Start();
                    Unit unit = ((B2S_ColliderEntity) this.Entity).m_BelongUnit;
                    Dictionary<long, SkillBaseNodeData> skillNodeDataSupporter =
                            unit.GetComponent<NP_RuntimeTreeManager>()
                                    .GetTreeByPrefabID(NP_Client_TreeIds.Darius_Q_Server).m_BelongNP_DataSupportor.mSkillDataDic;
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    Console.WriteLine("DateTime costed for Shuffle function is: {0}ms", ts.TotalMilliseconds);

                    BuffPoolComponent buffPoolComponent = Game.Scene.GetComponent<BuffPoolComponent>();
                    //Log.Info("开始执行正式判断逻辑");

                    //敌方英雄
                    if (Vector3.Distance(((B2S_ColliderEntity) this.Entity).m_BelongUnit.Position, b2SColliderEntity.m_BelongUnit.Position) >=
                        2.3f)
                    {
                        try
                        {
                            Log.Info("Q技能打到了诺克，外圈，开始添加Buff");
                            buffPoolComponent.AcquireBuff<FlashDamageBuffSystem>(
                                ((NodeDataForSkillBuff) skillNodeDataSupporter[10002]).SkillBuffBases,
                                ((B2S_ColliderEntity) this.Entity).m_BelongUnit, b2SColliderEntity.m_BelongUnit);
                            MessageHelper.Broadcast(new M2C_FrieBattleEvent_PlayEffect()
                            {
                                BattleKey = "Darius_Q_OutHit", FromUnitId = unit.Id, BelongToUnitId = b2SColliderEntity.m_BelongUnit.Id
                            });
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                    else
                    {
                        Log.Info("Q技能打到了诺克，内圈，开始添加Buff");
                        try
                        {
                            buffPoolComponent.AcquireBuff<FlashDamageBuffSystem>(
                                ((NodeDataForSkillBuff) skillNodeDataSupporter[10003]).SkillBuffBases,
                                ((B2S_ColliderEntity) this.Entity).m_BelongUnit, b2SColliderEntity.m_BelongUnit);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }

                    break;
            }
        }

        public void OnCollideSustain(B2S_ColliderEntity b2SColliderEntity)
        {
            //Log.Info("持续碰撞了");
        }

        public void OnCollideFinish(B2S_ColliderEntity b2SColliderEntity)
        {
            //Log.Info("不再碰撞了");
        }
    }
}