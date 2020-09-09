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
        /// <summary>
        /// 最终伤害值
        /// </summary>
        private float finalDamageValue;

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
            float tempFinalData = BuffDataCalculateHelper.CalculateCurrentData(this, this.BuffData);

            tempFinalData *= (this.BuffData as FlashDamageBuffData).DamageFix;

            Log.Info($"瞬时预计造成{tempFinalData}伤害");

            //TODO 对受方的伤害结算，此时finalDamageValue为最终值

            this.finalDamageValue = tempFinalData;

            this.TheUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue -= this.finalDamageValue;

            Game.EventSystem.Run(EventIdType.ChangeHP, this.TheUnitBelongto.Id, -this.finalDamageValue);

            //抛出Buff奏效事件
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