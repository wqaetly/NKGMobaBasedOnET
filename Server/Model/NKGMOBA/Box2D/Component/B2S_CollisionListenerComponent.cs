//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月20日 20:09:20
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Box2DSharp.Collision.Collider;
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
            B2S_FixtureUserData aUserData = (B2S_FixtureUserData) contact.FixtureA.UserData;
            B2S_FixtureUserData bUserData = (B2S_FixtureUserData) contact.FixtureB.UserData;
            if (this.collisionRecorder.ContainsKey((aUserData.Entity.InstanceId, bUserData.Entity.InstanceId)))
            {
                this.collisionRecorder[(aUserData.Entity.InstanceId, bUserData.Entity.InstanceId)] = true;
            }
            else
            {
                this.collisionRecorder.Add((aUserData.Entity.InstanceId, bUserData.Entity.InstanceId), true);
            }

            aUserData.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideStart(bUserData);
            bUserData.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideStart(aUserData);
        }

        public void EndContact(Contact contact)
        {
            B2S_FixtureUserData aUserData = (B2S_FixtureUserData) contact.FixtureA.UserData;
            B2S_FixtureUserData bUserData = (B2S_FixtureUserData) contact.FixtureB.UserData;

            this.collisionRecorder[(aUserData.Entity.InstanceId, bUserData.Entity.InstanceId)] = false;

            aUserData.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideFinish(bUserData);
            bUserData.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideFinish(aUserData);
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
                    a.GetComponent<B2S_CollisionResponseComponent>().OnCollideSustain(b.GetComponent<B2S_FixtureUserData>());
                    b.GetComponent<B2S_CollisionResponseComponent>().OnCollideSustain(a.GetComponent<B2S_FixtureUserData>());
                }
            }
        }
    }
}