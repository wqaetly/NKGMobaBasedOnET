//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019/8/14 16:22:55
// Description: 此代码switch case与System部分由工具生成，请勿进行增减操作
//------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public void OnCollideStart(B2S_HeroColliderData b2SHeroColliderData)
        {
            switch (b2SHeroColliderData.m_B2S_CollisionInstance.nodeDataId)
            {
                case 10006: //诺克：自身
                    Dictionary<long, SkillBaseNodeData> skillNodeDataSupporter =
                            Game.Scene.GetComponent<NP_TreeDataRepository>().GetNP_TreeData_DeepCopy(102892373671953).mSkillDataDic;
                    BuffPoolComponent buffPoolComponent = Game.Scene.GetComponent<BuffPoolComponent>();
                    //Log.Info("开始执行正式判断逻辑");
                    
                    //敌方英雄
                    if (Vector3.Distance(((B2S_HeroColliderData) this.Entity).m_BelongUnit.Position, b2SHeroColliderData.m_BelongUnit.Position) <=
                        2.3f)
                    {
                        try
                        {
                            Log.Info("Q技能打到了诺克，内圈，但这里模拟外圈，开始添加Buff");

                            Log.Info("添加监听回血Buff");
                            b2SHeroColliderData.m_BelongUnit.GetComponent<BuffManagerComponent>()
                                    .AddBuff(buffPoolComponent.AcquireBuff<ListenBuffCallBackBuffSystem>(
                                        ((NodeDataForSkillBuff) skillNodeDataSupporter[10005]).SkillBuffBases,
                                        ((B2S_HeroColliderData) this.Entity).m_BelongUnit, b2SHeroColliderData.m_BelongUnit));

                            Log.Info("添加外圈伤害Buff");
                            b2SHeroColliderData.m_BelongUnit.GetComponent<BuffManagerComponent>()
                                    .AddBuff(buffPoolComponent.AcquireBuff<FlashDamageBuffSystem>(
                                        ((NodeDataForSkillBuff) skillNodeDataSupporter[10002]).SkillBuffBases,
                                        ((B2S_HeroColliderData) this.Entity).m_BelongUnit, b2SHeroColliderData.m_BelongUnit));
                            
                            Log.Info("添加血怒Buff");
                            b2SHeroColliderData.m_BelongUnit.GetComponent<BuffManagerComponent>()
                                    .AddBuff(buffPoolComponent.AcquireBuff<BindStateBuffSystem>(
                                        ((NodeDataForSkillBuff) skillNodeDataSupporter[10004]).SkillBuffBases,
                                        ((B2S_HeroColliderData) this.Entity).m_BelongUnit, b2SHeroColliderData.m_BelongUnit));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                    else
                    {
                        Log.Info("Q技能打到了诺克，外圈，但这里模拟内圈，开始添加Buff");
                        try
                        {
                            b2SHeroColliderData.m_BelongUnit.GetComponent<BuffManagerComponent>()
                                    .AddBuff(buffPoolComponent.AcquireBuff<FlashDamageBuffSystem>(
                                        ((NodeDataForSkillBuff) skillNodeDataSupporter[10003]).SkillBuffBases,
                                        ((B2S_HeroColliderData) this.Entity).m_BelongUnit, b2SHeroColliderData.m_BelongUnit));
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

        public void OnCollideSustain(B2S_HeroColliderData b2SHeroColliderData)
        {
        }

        public void OnCollideFinish(B2S_HeroColliderData b2SHeroColliderData)
        {
        }
    }
}