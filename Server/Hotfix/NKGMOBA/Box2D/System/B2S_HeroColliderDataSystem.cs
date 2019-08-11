//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:31:14
//------------------------------------------------------------

using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using ETMode;
using ETModel;

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

            foreach (var VARIABLE in self.m_B2S_CollisionInstance.collisionId)
            {
                self.m_B2S_ColliderDataStructureBase.Add(b2SColliderDataRepositoryComponent.GetDataById(VARIABLE));
            }

            BodyDef bodyDef = new BodyDef();
            self.m_Body = Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().CreateBody(bodyDef);

            //根据数据加载具体的碰撞体，有的技能可能会产生多个碰撞体
            foreach (var VARIABLE in self.m_B2S_ColliderDataStructureBase)
            {
                switch (VARIABLE.b2SColliderType)
                {
                    case B2S_ColliderType.BoxColllider:
                        PolygonShape m_BoxShape = new PolygonShape();
                        m_BoxShape.SetAsBox(((B2S_BoxColliderDataStructure) VARIABLE).hx, ((B2S_BoxColliderDataStructure) VARIABLE).hy,
                            VARIABLE.finalOffset, 0);
                        FixtureDef fixtureDef1 = new FixtureDef();
                        fixtureDef1.IsSensor = VARIABLE.isSensor;
                        fixtureDef1.Shape = m_BoxShape;
                        fixtureDef1.UserData = self;
                        self.m_Body.CreateFixture(fixtureDef1);
                        break;
                    case B2S_ColliderType.CircleCollider:
                        CircleShape m_CircleShape = new CircleShape();
                        m_CircleShape.Radius = ((B2S_CircleColliderDataStructure) VARIABLE).radius;
                        m_CircleShape.Position = VARIABLE.finalOffset;
                        FixtureDef fixtureDef2 = new FixtureDef();
                        fixtureDef2.IsSensor = VARIABLE.isSensor;
                        fixtureDef2.Shape = m_CircleShape;
                        fixtureDef2.UserData = self;
                        self.m_Body.CreateFixture(fixtureDef2);
                        break;
                    case B2S_ColliderType.PolygonCollider:
                        foreach (var VARIABLE1 in ((B2S_PolygonColliderDataStructure) VARIABLE).finalPoints)
                        {
                            PolygonShape m_PolygonShape = new PolygonShape();
                            m_PolygonShape.Set(VARIABLE1.ToArray());
                            FixtureDef fixtureDef3 = new FixtureDef();
                            fixtureDef3.IsSensor = VARIABLE.isSensor;
                            fixtureDef3.Shape = m_PolygonShape;
                            fixtureDef3.UserData = self;
                            self.m_Body.CreateFixture(fixtureDef3);
                        }
                        break;
                }
            }

            //根据ID添加对应的碰撞处理组件
            Game.EventSystem.Run(self.m_B2S_CollisionInstance.collisionId.ToString(), self);
            //Log.Info("FixTureList大小为"+self.m_Body.FixtureList.Count.ToString());
        }
    }

    [ObjectSystem]
    public class B2S_HeroColliderDataFixedUpdateSystem: FixedUpdateSystem<B2S_HeroColliderData>
    {
        public override void FixedUpdate(B2S_HeroColliderData self)
        {
            //如果刚体处于激活状态，且设定上此刚体是跟随Unit的话，就同步位置和角度
            if (self.m_Body.IsActive && self.m_B2S_CollisionInstance.FollowUnit)
            {
                self.SetColliderBodyTransform();
                //Log.Info($"进行了位置移动，数据结点为{self.ID}");
            }
        }
    }

    public static class B2S_HeroColliderComponentHelper
    {
        /// <summary>
        /// 设置刚体位置
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void SetColliderBodyTransform(this B2S_HeroColliderData self)
        {
            self.SetColliderBodyPos(new Vector2(self.m_Unit.Position.x, self.m_Unit.Position.z));
            self.SetColliderBodyAngle(-UnityEngine.Quaternion.QuaternionToEuler(self.m_Unit.Rotation).y * Settings.Pi / 180);
        }
        
        /// <summary>
        /// 设置刚体位置
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void SetColliderBodyPos(this B2S_HeroColliderData self, Vector2 pos)
        {
            self.m_Body.SetTransform(pos, self.m_Body.GetAngle());
        }

        /// <summary>
        /// 设置刚体角度
        /// </summary>
        /// <param name="self"></param>
        /// <param name="angle"></param>
        public static void SetColliderBodyAngle(this B2S_HeroColliderData self, float angle)
        {
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