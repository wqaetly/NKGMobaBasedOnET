//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020/12/26 13:45:26
// Description: 此代码switch case与System部分由工具生成，请勿进行增减操作
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [Event(EventIdType_Collision.B2S_Darius_E_CRS)]
    public class AddB2S_Darius_E_CRSSystem: AEvent<Entity>
    {
        public override void Run(Entity a)
        {
            a.AddComponent<B2S_Darius_E_CRS>();
        }
    }

    [ObjectSystem]
    public class B2S_Darius_E_CRSAwakeSystem: AwakeSystem<B2S_Darius_E_CRS>
    {
        public override void Awake(B2S_Darius_E_CRS self)
        {
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideStartAction += self.OnCollideStart;
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideSustainAction += self.OnCollideSustain;
            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideFinishAction += self.OnCollideFinish;
        }
    }

    public class B2S_Darius_E_CRS: Component
    {
        public void OnCollideStart(Entity b2SCollider)
        {
            //技能碰撞体自身Unit的B2S_ColliderComponent
            B2S_ColliderComponent selfColliderComponent = Entity.GetComponent<B2S_ColliderComponent>();
            //碰撞到的Unit的B2S_ColliderComponent
            B2S_ColliderComponent targetColliderComponent = b2SCollider.GetComponent<B2S_ColliderComponent>();
            
            //自身Collider Unit所归属的Unit
            Unit selfBelongToUnit = selfColliderComponent.BelongToUnit;
            //碰撞到的Collider Unit所归属的Unit
            Unit collisionBelongToUnit = targetColliderComponent.BelongToUnit;
            
            //Log.Info("诺克Q技能打到了东西");
            switch (targetColliderComponent.B2S_CollisionInstance.nodeDataId)
            {
                case 10006: //诺克：自身
                    //TODO:这一步需要在结点编辑器配好支持自动升成
                    if (selfBelongToUnit.GetComponent<B2S_RoleCastComponent>()
                                .GetRoleCastToTarget(collisionBelongToUnit) !=
                        RoleCast.Adverse) return;

                    //获取目标SkillCanvas
                    List<NP_RuntimeTree> targetSkillCanvas = this.Entity.GetComponent<SkillCanvasManagerComponent>()
                            .GetSkillCanvas(Game.Scene.GetComponent<ConfigComponent>().Get<Server_SkillCanvasConfig>(10006).BelongToSkillId);

                    foreach (var skillCanvas in targetSkillCanvas)
                    {
                        skillCanvas.GetBlackboard().Set("Darius_E_IsHitUnit", true);
                        skillCanvas.GetBlackboard().Get<List<long>>("Darius_E_HitUnitIds")?.Add(collisionBelongToUnit.Id);
                    }

                    break;
            }
        }

        public void OnCollideSustain(Entity b2SCollider)
        {
        }

        public void OnCollideFinish(Entity b2SCollider)
        {
        }
    }
}