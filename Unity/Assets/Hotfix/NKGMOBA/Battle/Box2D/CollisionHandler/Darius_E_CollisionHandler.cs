using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class Darius_E_CollisionHandler : AB2S_CollisionHandler
    {
        public override void HandleCollisionStart(Unit a, Unit b)
        {
            B2S_RoleCastComponent aRole = a.GetComponent<B2S_RoleCastComponent>();
            B2S_ColliderComponent aColliderComponent = a.GetComponent<B2S_ColliderComponent>();

            B2S_RoleCastComponent bRole = b.GetComponent<B2S_RoleCastComponent>();
            B2S_ColliderComponent bColliderComponent = b.GetComponent<B2S_ColliderComponent>();

            RoleCast roleCast = aRole.GetRoleCastToTarget(b);
            RoleTag roleTag = bRole.RoleTag;
            Server_B2SCollisionRelationConfig serverB2SCollisionRelationConfig =
                Server_B2SCollisionRelationConfigCategory.Instance.Get(aColliderComponent
                    .B2S_CollisionRelationConfigId);

            switch (roleCast)
            {
                case RoleCast.Adverse:
                    switch (roleTag)
                    {
                        case RoleTag.Hero:
                            if (serverB2SCollisionRelationConfig.EnemyHero)
                            {
                                //获取目标SkillCanvas
                                List<NP_RuntimeTree> targetSkillCanvas = aColliderComponent.GetParent<Unit>()
                                    .GetComponent<SkillCanvasManagerComponent>()
                                    .GetSkillCanvas(
                                        SkillCanvasConfigCategory.Instance.Get(10007).BelongToSkillId);
                                
                                foreach (var skillCanvas in targetSkillCanvas)
                                {
                                    skillCanvas.GetBlackboard().Set("Darius_E_IsHitUnit", true);
                                    skillCanvas.GetBlackboard().Get<List<long>>("Darius_E_HitUnitIds")
                                        ?.Add(bColliderComponent.BelongToUnit.Id);
                                }
                            }

                            break;
                    }
                    break;
                case RoleCast.Friendly:
                    switch (roleTag)
                    {
                        case RoleTag.Hero:
                            if (serverB2SCollisionRelationConfig.FriendlyHero)
                            {
                            }

                            break;
                    }

                    break;
                case RoleCast.Neutral:
                    switch (roleTag)
                    {
                        case RoleTag.Creeps:
                            if (serverB2SCollisionRelationConfig.Creeps)
                            {
                            }

                            break;
                    }

                    break;
            }
        }

        public override void HandleCollisionSustain(Unit a, Unit b)
        {
        }

        public override void HandleCollisionEnd(Unit a, Unit b)
        {
        }
    }
}