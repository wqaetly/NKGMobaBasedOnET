namespace ET
{
    public class B2S_ParabolicCollisionHandler: AB2S_CollisionHandler
    {
        public override void HandleCollisionStart(Unit a, Unit b)
        {
            B2S_RoleCastComponent aRole = a.GetComponent<B2S_RoleCastComponent>();
            B2S_RoleCastComponent bRole = b.GetComponent<B2S_RoleCastComponent>();
            // 只有敌对或者中立碰撞才会销毁子弹
            if (aRole.GetRoleCastToTarget(b) ==
                RoleCast.Adverse && bRole.RoleTag == RoleTag.Sprite)
            {
                Unit hurtCaster = a.GetComponent<B2S_ColliderComponent>().BelongToUnit;
                Unit beHurtedUnit = b.GetComponent<B2S_ColliderComponent>().BelongToUnit;

                //如果已死亡就不做处理
                if (beHurtedUnit.GetComponent<DeadComponent>() != null)
                    return;

                DamageData damageData = ReferencePool.Acquire<DamageData>();
                hurtCaster.GetComponent<CastDamageComponent>().BaptismDamageData(damageData);
                beHurtedUnit.GetComponent<ReceiveDamageComponent>().ReceiveDamage(damageData);
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