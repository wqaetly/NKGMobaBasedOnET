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

namespace ET
{
    [ObjectSystem]
    public class B2S_CollisionListenerComponentAwake : AwakeSystem<B2S_CollisionListenerComponent>
    {
        public override void Awake(B2S_CollisionListenerComponent self)
        {
            //绑定指定的物理世界，正常来说一个房间一个物理世界,这里是Demo，直接获取了
            self.Parent.GetComponent<B2S_WorldComponent>().GetWorld().SetContactListener(self);
            //self.TestCollision();
            self.B2SWorldColliderManagerComponent = self.Parent.GetComponent<B2S_WorldColliderManagerComponent>();
        }
    }

    /// <summary>
    /// 某一物理世界所有碰撞的监听者，负责碰撞事件的分发
    /// </summary>
    public class B2S_CollisionListenerComponent : Entity, IContactListener
    {
        public B2S_WorldColliderManagerComponent B2SWorldColliderManagerComponent;

        private List<(long, long)> m_CollisionRecorder = new List<(long, long)>();

        private List<(long, long)> m_ToBeRemovedCollisionData = new List<(long, long)>();

        public void BeginContact(Contact contact)
        {
            //这里获取的是碰撞实体，比如诺克Q技能的碰撞体Unit，这里获取的就是它
            Unit unitA = (Unit) contact.FixtureA.UserData;
            Unit unitB = (Unit) contact.FixtureB.UserData;

            if (unitA.IsDisposed || unitB.IsDisposed)
            {
                return;
            }
            
            m_CollisionRecorder.Add((unitA.Id, unitB.Id));

            B2S_CollisionDispatcherComponent.Instance.HandleCollisionStart(unitA, unitB);
            B2S_CollisionDispatcherComponent.Instance.HandleCollisionStart(unitB, unitA);
        }

        public void EndContact(Contact contact)
        {
            Unit unitA = (Unit) contact.FixtureA.UserData;
            Unit unitB = (Unit) contact.FixtureB.UserData;
            
            // Id不分顺序，防止移除失败
            this.m_ToBeRemovedCollisionData.Add((unitA.Id, unitB.Id));
            this.m_ToBeRemovedCollisionData.Add((unitB.Id, unitA.Id));

            if (unitA.IsDisposed || unitB.IsDisposed)
            {
                return;
            }
            
            B2S_CollisionDispatcherComponent.Instance.HandleCollsionEnd(unitA, unitB);
            B2S_CollisionDispatcherComponent.Instance.HandleCollsionEnd(unitB, unitA);
        }

        public void PreSolve(Contact contact, in Manifold oldManifold)
        {
        }

        public void PostSolve(Contact contact, in ContactImpulse impulse)
        {
        }

        public void FixedUpdate()
        {
            foreach (var tobeRemovedData in m_ToBeRemovedCollisionData)
            {
                this.m_CollisionRecorder.Remove(tobeRemovedData);
            }

            m_ToBeRemovedCollisionData.Clear();
            
            foreach (var cachedCollisionData in this.m_CollisionRecorder)
            {
                Unit unitA = this.GetParent<Room>().GetComponent<UnitComponent>().Get(cachedCollisionData.Item1);
                Unit unitB = this.GetParent<Room>().GetComponent<UnitComponent>().Get(cachedCollisionData.Item2);
                
                if (unitA == null || unitB == null || unitA.IsDisposed || unitB.IsDisposed)
                {
                    // Id不分顺序，防止移除失败
                    this.m_ToBeRemovedCollisionData.Add((cachedCollisionData.Item1, cachedCollisionData.Item2));
                    this.m_ToBeRemovedCollisionData.Add((cachedCollisionData.Item2, cachedCollisionData.Item1));
                    continue;
                }

                B2S_CollisionDispatcherComponent.Instance.HandleCollisionSustain(unitA, unitB);
                B2S_CollisionDispatcherComponent.Instance.HandleCollisionSustain(unitB, unitA);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.IsDisposed)
                return;
            m_ToBeRemovedCollisionData.Clear();
            this.m_CollisionRecorder.Clear();
        }

        /// <summary>
        /// 测试碰撞
        /// </summary>
        public void TestCollision()
        {
            BodyDef bodyDef = new BodyDef {BodyType = BodyType.DynamicBody};
            Body m_Body = this.parent.GetComponent<B2S_WorldComponent>().GetWorld().CreateBody(bodyDef);
            CircleShape m_CircleShape = new CircleShape();
            m_CircleShape.Radius = 5;
            m_Body.CreateFixture(m_CircleShape, 5);

            BodyDef bodyDef1 = new BodyDef {BodyType = BodyType.DynamicBody};
            Body m_Body1 = this.parent.GetComponent<B2S_WorldComponent>().GetWorld().CreateBody(bodyDef1);
            CircleShape m_CircleShape1 = new CircleShape();
            m_CircleShape1.Radius = 5;
            m_Body1.CreateFixture(m_CircleShape1, 5);

            Log.Info("创建完成");
        }
    }
}