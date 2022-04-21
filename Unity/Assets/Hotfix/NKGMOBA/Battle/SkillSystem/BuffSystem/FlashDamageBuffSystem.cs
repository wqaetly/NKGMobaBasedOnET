//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:12:21
//------------------------------------------------------------

using System;

namespace ET
{
    /// <summary>
    /// 瞬时伤害
    /// </summary>
    public class FlashDamageBuffSystem : ABuffSystemBase<FlashDamageBuffData>
    {
#if SERVER
        public override void OnExecute(uint currentFrame)
        {
            FlashDamageBuffData flashDamageBuffData = this.GetBuffDataWithTType;

            DamageData damageData = ReferencePool.Acquire<DamageData>().InitData(flashDamageBuffData.BuffDamageTypes,
                BuffDataCalculateHelper.CalculateCurrentData(this), this.TheUnitFrom, this.TheUnitBelongto,
                flashDamageBuffData.CustomData);

            damageData.DamageValue *= flashDamageBuffData.DamageFix;

            this.TheUnitFrom.GetComponent<CastDamageComponent>().BaptismDamageData(damageData);

            float finalDamage =
                this.GetBuffTarget().GetComponent<ReceiveDamageComponent>().BaptismDamageData(damageData);
            
            if (finalDamage >= 0)
            {
                this.TheUnitBelongto.GetComponent<UnitAttributesDataComponent>().NumericComponent
                    .ApplyChange(NumericType.Hp, -finalDamage);
                //抛出伤害事件
                this.GetBuffTarget().BelongToRoom.GetComponent<BattleEventSystemComponent>().Run($"ExcuteDamage{this.TheUnitFrom.Id}", damageData);
                //抛出受伤事件
                this.GetBuffTarget().BelongToRoom.GetComponent<BattleEventSystemComponent>().Run($"TakeDamage{this.GetBuffTarget().Id}", damageData);
            }

            //TODO 从当前战斗Entity获取BattleEventSystem来Run事件
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    this.GetBuffTarget().BelongToRoom.GetComponent<BattleEventSystemComponent>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }
        }
#else
        public override void OnExecute(uint currentFrame)
        {

        }
#endif
    }
}