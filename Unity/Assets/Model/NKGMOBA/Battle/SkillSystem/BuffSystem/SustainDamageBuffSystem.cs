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
    public class SustainDamageBuffSystem: ABuffSystemBase
    {
        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private long m_SelfNextimer;

        public override void OnInit(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.TheUnitFrom = theUnitFrom;
            this.TheUnitBelongto = theUnitBelongto;
            this.BuffData = buffData;

            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, this.BuffData);
            //Log.Info("持续伤害Buff初始化完成");
        }

        public override void OnExecute()
        {
            ExcuteDamage();
            //Log.Info($"作用间隔为{selfNextimer - TimeHelper.Now()},持续时间为{temp.SustainTime},持续到{this.selfNextimer}");
            this.BuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if (this.BuffData.SustainTime + 1 > 0)
            {
                //Log.Info($"执行持续伤害的Update,当前时间是{TimeHelper.Now()}");
                if (TimeHelper.Now() > MaxLimitTime)
                {
                    this.BuffState = BuffState.Finished;
                    //Log.Info("持续伤害结束了");
                }
                else if (TimeHelper.Now() > this.m_SelfNextimer)
                {
                    ExcuteDamage();
                }
            }
        }

        public override void OnFinished()
        {
        }

        private void ExcuteDamage()
        {
            //强制类型转换为伤害Buff数据 
            SustainDamageBuffData temp = (SustainDamageBuffData) this.BuffData;

            DamageData damageData = ReferencePool.Acquire<DamageData>().InitData(temp.BuffDamageTypes,
                BuffDataCalculateHelper.CalculateCurrentData(this, this.BuffData), this.TheUnitFrom, this.TheUnitBelongto);

            damageData.DamageValue *= temp.DamageFix;

            this.TheUnitFrom.GetComponent<CastDamageComponent>().BaptismDamageData(damageData);

            float finalDamage = this.TheUnitBelongto.GetComponent<ReceiveDamageComponent>().BaptismDamageData(damageData);

            if (finalDamage >= 0)
            {
                this.TheUnitBelongto.GetComponent<HeroDataComponent>().NumericComponent[NumericType.Hp] += -finalDamage;
                //抛出伤害事件，需要监听伤害的buff（比如吸血buff）需要监听此事件
                Game.Scene.GetComponent<BattleEventSystem>().Run($"{EventIdType.ExcuteDamage}{this.TheUnitFrom.Id}", damageData);
            }

            //设置下一个时间点
            this.m_SelfNextimer = TimeHelper.Now() + temp.WorkInternal;
        }
    }
}