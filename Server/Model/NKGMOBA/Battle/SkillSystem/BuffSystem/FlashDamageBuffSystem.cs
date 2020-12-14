//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:12:21
//------------------------------------------------------------

using System;

namespace ETModel
{
    /// <summary>
    /// 瞬时伤害
    /// </summary>
    public class FlashDamageBuffSystem: ABuffSystemBase
    {
        public override void OnInit(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.TheUnitFrom = theUnitFrom;
            this.TheUnitBelongto = theUnitBelongto;
            this.BuffData = buffData;

            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, this.BuffData);
        }

        public override void OnExecute()
        {
            DamageData damageData = ReferencePool.Acquire<DamageData>().InitData((this.BuffData as FlashDamageBuffData).BuffDamageTypes,
                BuffDataCalculateHelper.CalculateCurrentData(this, this.BuffData), this.TheUnitFrom, this.TheUnitBelongto);

            damageData.DamageValue *= (this.BuffData as FlashDamageBuffData).DamageFix;

            this.TheUnitFrom.GetComponent<CastDamageComponent>().BaptismDamageData(damageData);

            float finalDamage = this.TheUnitBelongto.GetComponent<ReceiveDamageComponent>().BaptismDamageData(damageData);

            if (finalDamage >= 0)
            {
                this.TheUnitBelongto.GetComponent<HeroDataComponent>().NumericComponent.ApplyChange(NumericType.Hp, -finalDamage);
                //抛出伤害事件，需要监听伤害的buff（比如吸血buff）需要监听此事件
                Game.Scene.GetComponent<BattleEventSystem>().Run($"{EventIdType.ExcuteDamage}{this.TheUnitFrom.Id}", damageData);
            }
            
            //TODO 从当前战斗Entity获取BattleEventSystem来Run事件
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystem>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }

            this.BuffState = BuffState.Finished;
            //Log.Info($"设置瞬时伤害Buff：{this.MSkillBuffDataBase.FlagId}状态为Finshed");
        }

        public override void OnFinished()
        {
        }
    }
}