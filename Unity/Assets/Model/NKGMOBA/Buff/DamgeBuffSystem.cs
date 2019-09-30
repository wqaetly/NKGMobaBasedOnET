//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:12:21
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    public class DamageBuffSystem: BuffSystemBase
    {
        /// <summary>
        /// 最终伤害值
        /// </summary>
        private float finalDamageValue;

        public override void OnInit(BuffDataBase skillBuffDataBase, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.theUnitFrom = theUnitFrom;
            this.theUnitBelongto = theUnitBelongto;
            this.MSkillBuffDataBase = skillBuffDataBase;
            //强制类型转换为伤害Buff数据
            DamageBuffData temp = (DamageBuffData) MSkillBuffDataBase;
            //取得归属Unit的Hero数据，用以计算伤害数据
            HeroDataComponent theUnitFromHeroData = this.theUnitFrom.GetComponent<HeroDataComponent>();

            //依据基础数值的加成方式来获取对应伤害数据
            switch (temp.Base_BuffEffectedTypes)
            {
                case BuffEffectedTypes.FromHeroLevel:
                    finalDamageValue = temp.ValueToBeChanged[theUnitFromHeroData.CurrentLevel];
                    break;
                case BuffEffectedTypes.FromSkillLevel:
                    finalDamageValue = temp.ValueToBeChanged[theUnitFromHeroData.GetSkillLevel(temp.BelongSkillId)];
                    break;
            }

            //依据加成方式对伤害进行加成
            foreach (var VARIABLE in temp.additionValue)
            {
                switch (VARIABLE.Key)
                {
                    case BuffAdditionTypes.Percentage_Physical:
                        this.finalDamageValue = finalDamageValue * VARIABLE.Value *
                                theUnitFromHeroData.CurrentAttackValue;
                        break;
                    case BuffAdditionTypes.Percentage_Magic:
                        this.finalDamageValue = finalDamageValue * VARIABLE.Value *
                                theUnitFromHeroData.CurrentSpellpower;
                        break;
                }
            }

            //对伤害进行修正
            this.finalDamageValue *= temp.damageFix;

            //TODO 对受方的伤害结算，此时finalDamageValue为最终值

            //设置Buff状态为就绪
            this.MBuffState = BuffState.Waiting;
        }

        public override void OnExecute()
        {
            this.theUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue -= this.finalDamageValue;
            //抛出Buff奏效事件
            //单独写一个事件系统，避免不必要的性能浪费，只关心本局战斗即可
            //TODO 从当前战斗Entity获取BattleEventSystem来Run事件
            //Game.EventSystem.Run(EventIdType.BuffCallBack, this);
        }

        public override void OnFinished()
        {
        }
    }
}