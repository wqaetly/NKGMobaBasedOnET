using ET.EventType;

namespace ET
{
    public static class ReceiveDamageComponentSystems
    {
        #region 公有成员

        /// <summary>
        /// 洗礼这个伤害值
        /// </summary>
        /// <param name="damageData">伤害数据</param>
        /// <returns></returns>
        public static float BaptismDamageData(this ReceiveDamageComponent self, DamageData damageData)
        {
            Unit damageTaker = self.GetParent<Unit>();
            damageData.OperateTaker = damageTaker;
            damageData.DamageValue = damageData.DamageValue * self.DamagePrefix;
            return damageData.DamageValue < 0 ? 0 : damageData.DamageValue;
        }

        /// <summary>
        /// 接受伤害
        /// </summary>
        /// <param name="damageData"></param>
        /// <returns></returns>
        public static void ReceiveDamage(this ReceiveDamageComponent self, DamageData damageData)
        {
            //如果已经死亡就不能继续受到伤害
            if (self.GetParent<Unit>().GetComponent<DeadComponent>() != null)
            {
                ReferencePool.Release(damageData);
                return;
            }

            damageData.OperateTaker = self.GetParent<Unit>();
            self.BaptismDamageData(damageData);

            float currentHp = self.GetParent<Unit>().GetComponent<NumericComponent>()[NumericType.Hp];
            float finalHp = currentHp - damageData.DamageValue;

            Unit unit = self.GetParent<Unit>();

            // if (finalHp <= 0)
            // {
            //     finalHp = 0;
            //     self.GetParent<Unit>().GetComponent<NumericComponent>()[NumericType.Hp] = finalHp;
            //
            //     MessageHelper.BroadcastToRoom(unit.BelongToRoom,
            //         new M2C_ReceiveDamage() {FinalValue = damageData.DamageValue, UnitId = unit.Id});
            //     Game.EventSystem.Publish(new EventType.SpriteDead()
            //         {KillerSprite = damageData.OperateCaster, DeadSprite = damageData.OperateTaker}).Coroutine();
            // }
            // else
            // {
            //     self.GetParent<Unit>().GetComponent<NumericComponent>()[NumericType.Hp] = finalHp;
            //     MessageHelper.BroadcastToRoom(unit.BelongToRoom,
            //         new M2C_ReceiveDamage() {FinalValue = damageData.DamageValue, UnitId = unit.Id});
            // }
            //
            ReferencePool.Release(damageData);
        }

        #endregion
    }
}