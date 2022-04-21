//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:31:14
//------------------------------------------------------------

using Box2DSharp.Common;
using ET.EventType;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET
{
    /// <summary>
    /// 通过数据表中的Id进行初始化，用于我们自定义的，在配置表中配置的碰撞体
    /// </summary>
    public class
        B2S_ColliderComponentAwakeSystem : AwakeSystem<B2S_ColliderComponent, UnitFactory.CreateSkillColliderArgs>
    {
        public override void Awake(B2S_ColliderComponent self,
            UnitFactory.CreateSkillColliderArgs createSkillColliderArgs)
        {
            Server_B2SCollisionRelationConfig serverB2SCollisionRelationConfig =
                Server_B2SCollisionRelationConfigCategory.Instance
                    .Get(createSkillColliderArgs.collisionRelationDataConfigId);
            string collisionHandlerName = serverB2SCollisionRelationConfig.B2S_ColliderHandlerName;

            self.WorldComponent = self.GetParent<Unit>().BelongToRoom.GetComponent<B2S_WorldComponent>();
            self.BelongToUnit = createSkillColliderArgs.belontToUnit;
            self.B2S_CollisionRelationConfigId = serverB2SCollisionRelationConfig.Id;
            self.B2S_ColliderDataConfigId = serverB2SCollisionRelationConfig.B2S_ColliderConfigId;
            self.CollisionHandlerName = collisionHandlerName;

            self.SyncPosToBelongUnit = createSkillColliderArgs.FollowUnitPos;
            self.SyncRotToBelongUnit = createSkillColliderArgs.FollowUnitRot;

            Unit selfUnit = self.GetParent<Unit>();
            if (createSkillColliderArgs.FollowUnitPos)
            {
                selfUnit.Position = self.BelongToUnit.Position + createSkillColliderArgs.offset;
            }
            else
            {
                selfUnit.Position = createSkillColliderArgs.targetPos;
            }

            if (createSkillColliderArgs.FollowUnitRot)
            {
                selfUnit.Rotation = self.BelongToUnit.Rotation;
            }
            else
            {
                selfUnit.Rotation = Quaternion.Euler(new Vector3(0, createSkillColliderArgs.angle, 0));
            }

            self.CreateB2S_Collider();
            self.SyncBody();
        }
    }


    /// <summary>
    /// 直接传递碰撞体数据进行初始化
    /// </summary>
    public class
        B2S_ColliderComponentAwakeSystem1 : AwakeSystem<B2S_ColliderComponent, UnitFactory.CreateHeroColliderArgs>
    {
        public override void Awake(B2S_ColliderComponent self, UnitFactory.CreateHeroColliderArgs args)
        {
            self.WorldComponent = self.GetParent<Unit>().BelongToRoom.GetComponent<B2S_WorldComponent>();
            self.BelongToUnit = args.Unit;
            self.SyncPosToBelongUnit = args.FollowUnit;
            self.CollisionHandlerName = args.CollisionHandler;
            self.B2S_ColliderDataStructureBase = args.B2SColliderDataStructureBase;
            self.Body = self.WorldComponent.CreateDynamicBody();

            B2S_ColliderDataLoadHelper.ApplyFixture(self.B2S_ColliderDataStructureBase, self.Body,
                self.GetParent<Unit>());
            self.SyncBody();
        }
    }

    public class B2S_HeroColliderDataFixedUpdateSystem : FixedUpdateSystem<B2S_ColliderComponent>
    {
        public override void FixedUpdate(B2S_ColliderComponent self)
        {
            //如果刚体处于激活状态，且设定上此刚体是跟随Unit的话，就同步位置和角度
            if (self.Body.IsEnabled && !self.WorldComponent.GetWorld().IsLocked)
            {
                Unit unit = self.GetParent<Unit>();

#if SERVER
                if (self.BelongToUnit.GetComponent<MailBoxComponent>() == null)
                {
                    if (unit.Position!=self.BelongToUnit.Position)
                    {
                        Log.Info($"进行了位置移动");
                    }
                }  
#endif


                if (self.SyncPosToBelongUnit)
                {
                    unit.Position = self.BelongToUnit.Position;
                }

                if (self.SyncRotToBelongUnit)
                {
                    unit.Rotation = self.BelongToUnit.Rotation;
                }

                self.SyncBody();

#if !SERVER
                Game.EventSystem.Publish(new DebugVisualBox2D() {Unit = unit}).Coroutine();
#endif


            }
        }
    }

    public class B2S_HeroColliderDataDestroySystem : DestroySystem<B2S_ColliderComponent>
    {
        public override void Destroy(B2S_ColliderComponent self)
        {
            self.WorldComponent.AddBodyTobeDestroyed(self.Body);

            self.Body = null;
            self.B2S_ColliderDataStructureBase = null;
        }
    }


    public static class B2S_HeroColliderComponentHelper
    {
        /// <summary>
        /// 同步刚体（依据Unit载体，例如诺克UnitA释放碰撞体UnitB，这里的Unit同步是UnitB的同步）
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void SyncBody(this B2S_ColliderComponent self)
        {
            //Log.Info($"{new Vector2(self.BelongToUnit.Position.x, self.BelongToUnit.Position.z)}");
            Unit selfUnit = self.GetParent<Unit>();
            self.SetColliderBodyPos(new Vector2(selfUnit.Position.x, selfUnit.Position.z));

#if SERVER
            self.SetColliderBodyAngle(-Quaternion.QuaternionToEuler(selfUnit.Rotation).y * Settings.Pi / 180);
#else
            self.SetColliderBodyAngle(selfUnit.Rotation.eulerAngles.y * Mathf.PI / 180);
#endif
        }

        /// <summary>
        /// 设置刚体位置
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void SetColliderBodyPos(this B2S_ColliderComponent self, Vector2 pos)
        {
            self.Body.SetTransform(pos, self.Body.GetAngle());
            //Log.Info($"位置为{self.Body.GetPosition()} 类型为{self.Body.IsSleepingAllowed}");
        }

        /// <summary>
        /// 设置刚体角度
        /// </summary>
        /// <param name="self"></param>
        /// <param name="angle"></param>
        public static void SetColliderBodyAngle(this B2S_ColliderComponent self, float angle)
        {
            self.Body.SetTransform(self.Body.GetPosition(), angle);
        }

        /// <summary>
        /// 设置刚体状态
        /// </summary>
        /// <param name="self"></param>
        /// <param name="state"></param>
        public static void SetColliderBodyState(this B2S_ColliderComponent self, bool state)
        {
            self.Body.IsEnabled = state;
        }
    }
}