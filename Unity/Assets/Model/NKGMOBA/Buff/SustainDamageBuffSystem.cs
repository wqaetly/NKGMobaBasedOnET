//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月1日 16:00:56
//------------------------------------------------------------

using System;

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
        private long selfNextimer;

        public override void OnInit(BuffDataBase BuffDataBase, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.theUnitFrom = theUnitFrom;
            this.theUnitBelongto = theUnitBelongto;
            this.MSkillBuffDataBase = BuffDataBase;

            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, this.MSkillBuffDataBase);
            //Log.Info("持续伤害Buff初始化完成");
        }

        public override void OnExecute()
        {
            try
            {
                //Log.Info("进入持续伤害的Execute");
                currentDamageValue = BuffDataCalculateHelper.CalculateCurrentData(this, this.MSkillBuffDataBase);
                //强制类型转换为伤害Buff数据
                SustainDamageBuffData temp = (SustainDamageBuffData) MSkillBuffDataBase;

                //TODO 对受方的伤害结算，此时finalDamageValue为最终值

                this.theUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue -= this.currentDamageValue;
                Log.Info($"来自持续伤害ExeCute的数据:{this.currentDamageValue}");
                //设置下一个时间点
                this.selfNextimer = TimeHelper.Now() + temp.WorkInternal;
                //Log.Info($"作用间隔为{selfNextimer - TimeHelper.Now()},持续时间为{temp.SustainTime},持续到{this.selfNextimer}");
                this.MBuffState = BuffState.Running;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if (this.MSkillBuffDataBase.SustainTime + 1 > 0)
            {
                //Log.Info($"执行持续伤害的Update,当前时间是{TimeHelper.Now()}");
                if (TimeHelper.Now() > MaxLimitTime)
                {
                    this.MBuffState = BuffState.Finished;
                    Log.Info("持续伤害结束了");
                }
                else if (TimeHelper.Now() > this.selfNextimer)
                {
                    try
                    {
                        currentDamageValue = BuffDataCalculateHelper.CalculateCurrentData(this, this.MSkillBuffDataBase);
                        //强制类型转换为伤害Buff数据
                        SustainDamageBuffData temp = (SustainDamageBuffData) MSkillBuffDataBase;

                        //TODO 对受方的伤害结算，此时finalDamageValue为最终值

                        this.theUnitBelongto.GetComponent<HeroDataComponent>().CurrentLifeValue -= this.currentDamageValue;
                        Log.Info($"来自持续伤害Update的数据:{this.currentDamageValue},结束时间为{MaxLimitTime},当前层数为{this.CurrentOverlay}");
                        //设置下一个时间点
                        this.selfNextimer = TimeHelper.Now() + temp.WorkInternal;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }

        public override void OnFinished()
        {
        }
    }
}