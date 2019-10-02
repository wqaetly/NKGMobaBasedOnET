//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:12:21
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// 瞬时伤害
    /// </summary>
    public class FlashDamageBuffSystem: BuffSystemBase
    {
        /// <summary>
        /// 最终伤害值
        /// </summary>
        private float finalDamageValue;

        public override void OnInit(BuffDataBase BuffDataBase, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.theUnitFrom = theUnitFrom;
            this.theUnitBelongto = theUnitBelongto;
            this.MSkillBuffDataBase = BuffDataBase;

            //设置Buff状态为就绪
            this.MBuffState = BuffState.Waiting;
        }

        public override void OnExecute()
        {
            float tempFinalData = BuffDataCalculateHelper.CalculateCurrentData(this, this.MSkillBuffDataBase);

            tempFinalData *= (MSkillBuffDataBase as FlashDamageBuffData).damageFix;

            Log.Info($"预计造成{tempFinalData}伤害");
            
            //TODO 对受方的伤害结算，此时finalDamageValue为最终值
            
            this.finalDamageValue = tempFinalData;
            this.theUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue -= this.finalDamageValue;

            //抛出Buff奏效事件
            //TODO 从当前战斗Entity获取BattleEventSystem来Run事件
            Game.Scene.GetComponent<BattleEventSystem>().Run(this.MSkillBuffDataBase.theEventID, this);
            Log.Info($"抛出了EventID为{this.MSkillBuffDataBase.theEventID}的事件");
            
            this.MBuffState = BuffState.Finished;
        }

        public override void OnFinished()
        {
        }
    }
}