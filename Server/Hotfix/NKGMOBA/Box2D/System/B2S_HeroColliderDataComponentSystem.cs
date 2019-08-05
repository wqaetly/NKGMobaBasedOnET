//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:31:14
//------------------------------------------------------------

using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ETMode;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class B2S_HeroColliderDataComponentAwakeSystem: AwakeSystem<B2S_HeroColliderDataComponent, long>
    {
        public override void Awake(B2S_HeroColliderDataComponent self, long id)
        {
            self.ID = id;
            self.m_Unit = (Unit) self.Entity;
            LoadDependenceRes(self);
        }

        /// <summary>
        /// 加载依赖数据
        /// </summary>
        /// <param name="self"></param>
        private void LoadDependenceRes(B2S_HeroColliderDataComponent self)
        {
            B2S_ColliderDataRepositoryComponent b2SColliderDataRepositoryComponent =
                    Game.Scene.GetComponent<B2S_ColliderDataRepositoryComponent>();

            foreach (var VARIABLE in self.m_B2S_CollisionInstance.collisionId)
            {
                self.m_B2S_ColliderDataStructureBase.Add(b2SColliderDataRepositoryComponent.GetDataById(VARIABLE));
            }

            BodyDef bodyDef = new BodyDef();
            self.m_Body = Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().CreateBody(bodyDef);

            foreach (var VARIABLE in self.m_B2S_ColliderDataStructureBase)
            {
                switch (VARIABLE.b2SColliderType)
                {
                    case B2S_ColliderType.BoxColllider:
                        PolygonShape m_BoxShape = new PolygonShape();
                        m_BoxShape.SetAsBox(((B2S_BoxColliderDataStructure) VARIABLE).hx, ((B2S_BoxColliderDataStructure) VARIABLE).hy);
                        self.m_Body.CreateFixture(m_BoxShape, 0);
                        break;
                    case B2S_ColliderType.CircleCollider:
                        CircleShape m_CircleShape = new CircleShape();
                        m_CircleShape.Radius = ((B2S_CircleColliderDataStructure) VARIABLE).radius;
                        self.m_Body.CreateFixture(m_CircleShape, 0);
                        break;
                    case B2S_ColliderType.PolygonCollider:
                        foreach (var VARIABLE1 in ((B2S_PolygonColliderDataStructure) VARIABLE).finalPoints)
                        {
                            PolygonShape m_PolygonShape = new PolygonShape();
                            m_PolygonShape.Set(VARIABLE1.ToArray());
                            self.m_Body.CreateFixture(m_PolygonShape, 0);
                        }
                        break;
                }
            }
        }
    }
}