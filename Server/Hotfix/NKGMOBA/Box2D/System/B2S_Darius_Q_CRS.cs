//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019/8/14 16:22:55
// Description: 此代码switch case与System部分由工具生成，请勿进行增减操作
//------------------------------------------------------------

using System;
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
            //Log.Info($"诺手Q碰撞体创建完成，帧步进为{BenchmarkHelper.CurrentFrameCount}");
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
        /// <param name="b2SCollider">碰撞到的对象</param>
        public void OnCollideStart(Entity b2SCollider)
        {
            //Log.Info("诺克Q技能打到了东西");
            switch (b2SCollider.GetComponent<B2S_ColliderComponent>().B2S_CollisionInstance.nodeDataId)
            {
                case 10006: //诺克：自身
                    //TODO:这一步需要在结点编辑器配好支持自动升成
                    if (b2SCollider.GetComponent<B2S_ColliderComponent>().BelongToUnit.GetComponent<B2S_RoleCastComponent>().RoleCast !=
                        RoleCast.Adverse) return;
                    
                    //Log.Info($"诺手Q碰撞体碰撞响应创建完成，帧步进为{BenchmarkHelper.CurrentFrameCount}");
                    //敌方英雄
                    if (Vector3.Distance(this.Entity.GetComponent<B2S_ColliderComponent>().BelongToUnit.Position,
                            b2SCollider.GetComponent<B2S_ColliderComponent>().BelongToUnit.Position) >=
                        2.3f)
                    {
                        try
                        {
                            Log.Info("Q技能打到了诺克，外圈，开始添加Buff");
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

        public void OnCollideSustain(Entity b2SCollider)
        {
            //Log.Info("持续碰撞了");
        }

        public void OnCollideFinish(Entity b2SCollider)
        {
            //Log.Info("不再碰撞了");
        }
    }
}