//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:31:14
//------------------------------------------------------------

using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using ETMode;
using ETModel;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ETHotfix
{
    [ObjectSystem]
    public class B2S_HeroColliderDataAwakeSystem: AwakeSystem<B2S_HeroColliderData, B2S_CollisionInstance, long>
    {
        public override void Awake(B2S_HeroColliderData self, B2S_CollisionInstance b2SCollisionInstance, long id)
        {
            self.ID = id;
            self.m_B2S_CollisionInstance = b2SCollisionInstance;
            self.m_Unit = ComponentFactory.Create<Unit>();
            self.m_BelongUnit = (Unit) self.Entity;
            LoadDependenceRes(self);
        }

        /// <summary>
        /// 加载依赖数据
        /// </summary>
        /// <param name="self"></param>
        private void LoadDependenceRes(B2S_HeroColliderData self)
        {
            B2S_ColliderDataRepositoryComponent b2SColliderDataRepositoryComponent =
                    Game.Scene.GetComponent<B2S_ColliderDataRepositoryComponent>();

            self.AddComponent<B2S_CollisionResponseComponent>();

            foreach (var VARIABLE in self.m_B2S_CollisionInstance.collisionId)
            {
                self.m_B2S_ColliderDataStructureBase.Add(b2SColliderDataRepositoryComponent.GetDataById(VARIABLE));
            }

            self.m_Body = B2S_BodyUtility.CreateDynamicBody();

            //根据数据加载具体的碰撞体，有的技能可能会产生多个碰撞体
            foreach (var VARIABLE in self.m_B2S_ColliderDataStructureBase)
            {
                switch (VARIABLE.b2SColliderType)
                {
                    case B2S_ColliderType.BoxColllider:
                        self.m_Body.CreateBoxFixture(((B2S_BoxColliderDataStructure) VARIABLE).hx, ((B2S_BoxColliderDataStructure) VARIABLE).hy,
                            VARIABLE.finalOffset, 0, VARIABLE.isSensor, self);
                        break;
                    case B2S_ColliderType.CircleCollider:
                        self.m_Body.CreateCircleFixture(((B2S_CircleColliderDataStructure) VARIABLE).radius, VARIABLE.finalOffset, VARIABLE.isSensor,
                            self);
                        break;
                    case B2S_ColliderType.PolygonCollider:
                        foreach (var VARIABLE1 in ((B2S_PolygonColliderDataStructure) VARIABLE).finalPoints)
                        {
                            self.m_Body.CreatePolygonFixture(VARIABLE1, VARIABLE.isSensor, self);
                        }

                        break;
                }
            }

            //根据ID添加对应的碰撞处理组件
            Game.EventSystem.Run(self.m_B2S_CollisionInstance.nodeDataId.ToString(), self);
            //Log.Info($"已经分发{self.m_B2S_CollisionInstance.nodeDataId}技能组装事件");
            //Log.Info("FixTureList大小为"+self.m_Body.FixtureList.Count.ToString());
        }
    }

    [ObjectSystem]
    public class B2S_HeroColliderDataFixedUpdateSystem: FixedUpdateSystem<B2S_HeroColliderData>
    {
        public override void FixedUpdate(B2S_HeroColliderData self)
        {
            //如果刚体处于激活状态，且设定上此刚体是跟随Unit的话，就同步位置和角度
            if (self.m_Body.IsActive && self.m_B2S_CollisionInstance.FollowUnit && !Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().IsLocked)
            {
                self.SyncBody();
                //Log.Info($"进行了位置移动，数据结点为{self.ID}");
            }
        }
    }

    public static class B2S_HeroColliderComponentHelper
    {
        /// <summary>
        /// 同步刚体（依据归属Unit）
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void SyncBody(this B2S_HeroColliderData self)
        {
            self.SetColliderBodyPos(new Vector2(self.m_BelongUnit.Position.x, self.m_BelongUnit.Position.z));
            self.SetColliderBodyAngle(-Quaternion.QuaternionToEuler(self.m_BelongUnit.Rotation).y * Settings.Pi / 180);
        }

        /// <summary>
        /// 设置刚体位置
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void SetColliderBodyPos(this B2S_HeroColliderData self, Vector2 pos)
        {
            self.m_Unit.Position = new Vector3(pos.X, pos.Y, 0);
            self.m_Body.SetTransform(pos, self.m_Body.GetAngle());
            //Log.Info($"位置为{pos}");
        }

        /// <summary>
        /// 设置刚体角度
        /// </summary>
        /// <param name="self"></param>
        /// <param name="angle"></param>
        public static void SetColliderBodyAngle(this B2S_HeroColliderData self, float angle)
        {
            self.m_Unit.Rotation = Quaternion.Euler(0, angle, 0);
            self.m_Body.SetTransform(self.m_Body.GetPosition(), angle);
        }

        /// <summary>
        /// 设置刚体状态
        /// </summary>
        /// <param name="self"></param>
        /// <param name="state"></param>
        public static void SetColliderBodyState(this B2S_HeroColliderData self, bool state)
        {
            self.m_Body.IsActive = state;
        }
    }
}