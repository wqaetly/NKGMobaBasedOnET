//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:31:14
//------------------------------------------------------------

using Box2DSharp.Common;
using ETModel;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ETHotfix
{
    [ObjectSystem]
    public class B2S_ColliderComponentAwakeSystem: AwakeSystem<B2S_ColliderComponent, Unit, B2S_CollisionInstance, long>
    {
        public override void Awake(B2S_ColliderComponent self, Unit belongToUnit, B2S_CollisionInstance b2SCollisionInstance, long id)
        {
            self.NodeDataId = id;
            self.B2S_CollisionInstance = b2SCollisionInstance;
            self.BelongToUnit = belongToUnit;
            LoadDependenceRes(self);
        }

        /// <summary>
        /// 加载依赖数据，并且进行碰撞体的生成
        /// </summary>
        /// <param name="self"></param>
        private void LoadDependenceRes(B2S_ColliderComponent self)
        {
            B2S_ColliderDataRepositoryComponent b2SColliderDataRepositoryComponent =
                    Game.Scene.GetComponent<B2S_ColliderDataRepositoryComponent>();

            self.Entity.AddComponent<B2S_CollisionResponseComponent>();

            foreach (var VARIABLE in self.B2S_CollisionInstance.collisionId)
            {
                self.B2S_ColliderDataStructureBase.Add(b2SColliderDataRepositoryComponent.GetDataById(VARIABLE));
            }

            self.Body = B2S_BodyUtility.CreateDynamicBody();

            //根据数据加载具体的碰撞体，有的技能可能会产生多个碰撞体
            foreach (var colliderData in self.B2S_ColliderDataStructureBase)
            {
                switch (colliderData.b2SColliderType)
                {
                    case B2S_ColliderType.BoxColllider:
                        self.Body.CreateBoxFixture(((B2S_BoxColliderDataStructure) colliderData).hx, ((B2S_BoxColliderDataStructure) colliderData).hy,
                            colliderData.finalOffset, 0, colliderData.isSensor, self.Entity);
                        break;
                    case B2S_ColliderType.CircleCollider:
                        self.Body.CreateCircleFixture(((B2S_CircleColliderDataStructure) colliderData).radius, colliderData.finalOffset, colliderData.isSensor,
                            self.Entity);
                        break;
                    case B2S_ColliderType.PolygonCollider:
                        foreach (var VARIABLE1 in ((B2S_PolygonColliderDataStructure) colliderData).finalPoints)
                        {
                            self.Body.CreatePolygonFixture(VARIABLE1, colliderData.isSensor, self.Entity);
                        }

                        break;
                }
            }

            //根据ID添加对应的碰撞处理组件
            Game.EventSystem.Run(self.B2S_CollisionInstance.nodeDataId.ToString(), self.Entity);
            //Log.Info($"已经分发{self.m_B2S_CollisionInstance.nodeDataId}技能组装事件");
            //Log.Info("FixTureList大小为"+self.m_Body.FixtureList.Count.ToString());
        }
    }

    [ObjectSystem]
    public class B2S_HeroColliderDataFixedUpdateSystem: FixedUpdateSystem<B2S_ColliderComponent>
    {
        public override void FixedUpdate(B2S_ColliderComponent self)
        {
            //如果刚体处于激活状态，且设定上此刚体是跟随Unit的话，就同步位置和角度
            if (self.Body.IsEnabled && self.B2S_CollisionInstance.FollowUnit && !Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().IsLocked)
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
        public static void SyncBody(this B2S_ColliderComponent self)
        {
            self.SetColliderBodyPos(new Vector2(self.BelongToUnit.Position.x, self.BelongToUnit.Position.z));
            self.SetColliderBodyAngle(-Quaternion.QuaternionToEuler(self.BelongToUnit.Rotation).y * Settings.Pi / 180);
        }

        /// <summary>
        /// 设置刚体位置
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void SetColliderBodyPos(this B2S_ColliderComponent self, Vector2 pos)
        {
            (self.Entity as Unit).Position = new Vector3(pos.X, pos.Y, 0);
            self.Body.SetTransform(pos, self.Body.GetAngle());
            //Log.Info($"位置为{pos}");
        }

        /// <summary>
        /// 设置刚体角度
        /// </summary>
        /// <param name="self"></param>
        /// <param name="angle"></param>
        public static void SetColliderBodyAngle(this B2S_ColliderComponent self, float angle)
        {
            (self.Entity as Unit).Rotation = Quaternion.Euler(0, angle, 0);
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