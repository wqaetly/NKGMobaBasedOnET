//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月20日 20:09:20
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;

namespace ETModel
{
    [ObjectSystem]
    public class B2S_CollisionListenerComponentAwake: AwakeSystem<B2S_CollisionListenerComponent>
    {
        public override void Awake(B2S_CollisionListenerComponent self)
        {
            self.UnitComponent = Game.Scene.GetComponent<UnitComponent>();
            //绑定指定的物理世界，正常来说一个房间一个物理世界,这里是Demo，直接获取了
            
            Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().SetContactListener(self);
            //self.TestCollision();
            
        }
    }

    [ObjectSystem]
    public class B2S_CollisionListenerComponentFixedUpdate: FixedUpdateSystem<B2S_CollisionListenerComponent>
    {
        public override void FixedUpdate(B2S_CollisionListenerComponent self)
        {
            self.FixedUpdate();
        }
    }

    /// <summary>
    /// 某一物理世界所有碰撞的监听者，负责碰撞事件的分发
    /// </summary>
    public class B2S_CollisionListenerComponent: Component, IContactListener
    {
        public UnitComponent UnitComponent;
        public Dictionary<(long, long), bool> collisionRecorder = new Dictionary<(long, long), bool>();

        public void BeginContact(Contact contact)
        {
            B2S_HeroColliderData aUserData = (B2S_HeroColliderData) contact.FixtureA.UserData;
            B2S_HeroColliderData bUserData = (B2S_HeroColliderData) contact.FixtureB.UserData;
            if (this.collisionRecorder.ContainsKey((aUserData.Id, bUserData.Id)))
            {
                this.collisionRecorder[(aUserData.Id, bUserData.Id)] = true;
            }
            else
            {
                this.collisionRecorder.Add((aUserData.Id, bUserData.Id), true);
            }

            aUserData.GetComponent<B2S_CollisionResponseComponent>().OnCollideStart(bUserData);
            bUserData.GetComponent<B2S_CollisionResponseComponent>().OnCollideStart(aUserData);
        }

        public void EndContact(Contact contact)
        {
            B2S_HeroColliderData aUserData = (B2S_HeroColliderData) contact.FixtureA.UserData;
            B2S_HeroColliderData bUserData = (B2S_HeroColliderData) contact.FixtureB.UserData;

            this.collisionRecorder[(aUserData.Id, bUserData.Id)] = false;

            aUserData.GetComponent<B2S_CollisionResponseComponent>().OnCollideFinish(bUserData);
            bUserData.GetComponent<B2S_CollisionResponseComponent>().OnCollideFinish(aUserData);
        }

        public void PreSolve(Contact contact, in Manifold oldManifold)
        {
        }

        public void PostSolve(Contact contact, in ContactImpulse impulse)
        {
        }

        public void FixedUpdate()
        {
            foreach (var VARIABLE in this.collisionRecorder)
            {
                if (VARIABLE.Value)
                {
                    var a = this.UnitComponent.Get(VARIABLE.Key.Item1);
                    var b = this.UnitComponent.Get(VARIABLE.Key.Item2);
                    a.GetComponent<B2S_CollisionResponseComponent>().OnCollideSustain(b.GetComponent<B2S_HeroColliderData>());
                    b.GetComponent<B2S_CollisionResponseComponent>().OnCollideSustain(a.GetComponent<B2S_HeroColliderData>());
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.IsDisposed)
                return;
            this.collisionRecorder.Clear();
        }

        /// <summary>
        /// 测试碰撞
        /// </summary>
        public void TestCollision()
        {
            BodyDef bodyDef = new BodyDef{BodyType = BodyType.DynamicBody};
            Body m_Body = Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().CreateBody(bodyDef);
            CircleShape m_CircleShape = new CircleShape();
            m_CircleShape.Radius = 5;
            m_Body.CreateFixture(m_CircleShape, 5);
            
            BodyDef bodyDef1 = new BodyDef{BodyType = BodyType.DynamicBody};
            Body m_Body1 = Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().CreateBody(bodyDef1);
            CircleShape m_CircleShape1 = new CircleShape();
            m_CircleShape1.Radius = 5;
            m_Body1.CreateFixture(m_CircleShape1, 5);
            Log.Info("创建完毕");
        }
    }
}