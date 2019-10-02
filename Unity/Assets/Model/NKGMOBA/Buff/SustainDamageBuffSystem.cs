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
            
            SustainDamageBuffSystem tempSystem =
                    (SustainDamageBuffSystem) theUnitBelongto.GetComponent<BuffManagerComponent>().GetBuffByFlagID(BuffDataBase.FlagId);

            if (tempSystem != null)
            {
                SustainDamageBuffData tempData = tempSystem.MSkillBuffDataBase as SustainDamageBuffData;
                //可以叠加，并且当前层数未达到最高层
                if (tempData.CanOverlay &&
                    tempSystem.CurrentOverlay < tempData.MaxOverlay)
                {
                    //如果是有限时长的
                    if (tempData.SustainTime + 1 > 0)
                    {
                        tempSystem.maxTime += tempData.SustainTime;
                    }

                    tempSystem.CurrentOverlay++;
                }

                this.MBuffState = BuffState.Finished;
            }
            else
            {
                //如果是有限时长的
                if (this.MSkillBuffDataBase.SustainTime + 1 > 0)
                {
                    maxTime = TimeHelper.ClientNow() + ((SustainDamageBuffData) this.MSkillBuffDataBase).SustainTime;
                }

                this.CurrentOverlay++;

                this.MBuffState = BuffState.Waiting;
            }

            maxTime = TimeHelper.ClientNow() + this.MSkillBuffDataBase.SustainTime;
            this.MBuffState = BuffState.Waiting;
        }

        public override void OnExecute()
        {
            currentDamageValue = BuffDataCalculateHelper.CalculateCurrentData(this, this.MSkillBuffDataBase);
            //强制类型转换为伤害Buff数据
            SustainDamageBuffData temp = (SustainDamageBuffData) MSkillBuffDataBase;
            
            //TODO 对受方的伤害结算，此时finalDamageValue为最终值
            
            this.theUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue -= this.currentDamageValue;
            Log.Info($"来自持续伤害的数据:{this.currentDamageValue}");
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
                currentDamageValue = BuffDataCalculateHelper.CalculateCurrentData(this, this.MSkillBuffDataBase);
                //强制类型转换为伤害Buff数据
                SustainDamageBuffData temp = (SustainDamageBuffData) MSkillBuffDataBase;
            
                //TODO 对受方的伤害结算，此时finalDamageValue为最终值
            
                this.theUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue -= this.currentDamageValue;
                Log.Info($"来自持续伤害的数据:{this.currentDamageValue}");
                //设置下一个时间点
                this.selfNextimer = TimeHelper.Now() + temp.WorkInternal;
                this.MBuffState = BuffState.Running;
            }
        }

        public override void OnFinished()
        {
        }
    }
}