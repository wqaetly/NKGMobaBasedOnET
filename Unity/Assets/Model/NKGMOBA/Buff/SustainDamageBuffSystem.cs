//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月1日 16:00:56
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// 持续伤害，一般描述为X秒内造成Y伤害，或者每X秒造成Y伤害
    /// </summary>
    public class SustainDamageBuffSystem: BuffSystemBase
    {
        /// <summary>
        /// 本次伤害值
        /// </summary>
        private float currentDamageValue;

        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private float selfNextimer;

        /// <summary>
        /// 最大时间限制（持续几秒）
        /// </summary>
        private float maxTime;

        public override void OnInit(BuffDataBase BuffDataBase, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.theUnitFrom = theUnitFrom;
            this.theUnitBelongto = theUnitBelongto;
            this.MSkillBuffDataBase = BuffDataBase;

            maxTime = TimeHelper.ClientNow() + ((SustainDamageBuffData) this.MSkillBuffDataBase).SustainTime;

            this.CalculateCurrentDamage();

            this.MBuffState = BuffState.Waiting;
        }

        public override void OnExecute()
        {
            //强制类型转换为伤害Buff数据
            SustainDamageBuffData temp = (SustainDamageBuffData) MSkillBuffDataBase;
            this.theUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue -= this.currentDamageValue;
            //设置下一个时间点
            this.selfNextimer = TimeHelper.Now() + temp.WorkInternal;
            this.MBuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            if (TimeHelper.ClientNow() > maxTime)
            {
                this.MBuffState = BuffState.Finished;
            }
            
            if (TimeHelper.Now() > this.selfNextimer)
            {
                this.CalculateCurrentDamage();
            }
        }

        public override void OnFinished()
        {
        }

        /// <summary>
        /// 计算本次伤害
        /// </summary>
        private void CalculateCurrentDamage()
        {
            //强制类型转换为伤害Buff数据
            SustainDamageBuffData temp = (SustainDamageBuffData) MSkillBuffDataBase;
            //取得归属Unit的Hero数据，用以计算伤害数据
            HeroDataComponent theUnitFromHeroData = this.theUnitFrom.GetComponent<HeroDataComponent>();

            //依据基础数值的加成方式来获取对应伤害数据
            switch (temp.Base_BuffEffectedTypes)
            {
                case BuffEffectedTypes.FromHeroLevel:
                    this.currentDamageValue = temp.ValueToBeChanged[theUnitFromHeroData.CurrentLevel];
                    break;
                case BuffEffectedTypes.FromSkillLevel:
                    this.currentDamageValue = temp.ValueToBeChanged[theUnitFromHeroData.GetSkillLevel(temp.BelongSkillId)];
                    break;
            }

            //依据加成方式对伤害进行加成
            foreach (var VARIABLE in temp.additionValue)
            {
                switch (VARIABLE.Key)
                {
                    case BuffAdditionTypes.Percentage_Physical:
                        this.currentDamageValue += VARIABLE.Value *
                                theUnitFromHeroData.CurrentAttackValue;
                        break;
                    case BuffAdditionTypes.Percentage_Magic:
                        this.currentDamageValue += VARIABLE.Value *
                                theUnitFromHeroData.CurrentSpellpower;
                        break;
                }
            }
            
            Log.Info($"来自持续伤害：本次伤害为{currentDamageValue}");
        }
    }
}