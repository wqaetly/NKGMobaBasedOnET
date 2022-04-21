//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月1日 16:00:56
//------------------------------------------------------------

using System;

namespace ET
{
    /// <summary>
    /// 持续伤害，一般描述为X秒内造成Y伤害，或者每X秒造成Y伤害
    /// </summary>
    public class SustainDamageBuffSystem : ABuffSystemBase<SustainDamageBuffData>
    {
#if SERVER
        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private uint m_SelfNextExcuteFrame = 0;

        public override void OnExecute(uint currentFrame)
        {
            ExcuteDamage(currentFrame);
            //Log.Info($"作用间隔为{selfNextimer - TimeHelper.Now()},持续时间为{temp.SustainTime},持续到{this.selfNextimer}");
        }

        public override void OnUpdate(uint currentFrame)
        {
            if (currentFrame >= this.m_SelfNextExcuteFrame)
            {
                ExcuteDamage(currentFrame);
            }
        }

        private void ExcuteDamage(uint currentFrame)
        {
            //强制类型转换为伤害Buff数据 
            SustainDamageBuffData temp = this.GetBuffDataWithTType;

            DamageData damageData = ReferencePool.Acquire<DamageData>().InitData(temp.BuffDamageTypes,
                BuffDataCalculateHelper.CalculateCurrentData(this), this.TheUnitFrom, this.TheUnitBelongto);
            
            damageData.DamageValue *= temp.DamageFix;

            this.TheUnitFrom.GetComponent<CastDamageComponent>().BaptismDamageData(damageData);

            float finalDamage =
                this.TheUnitBelongto.GetComponent<ReceiveDamageComponent>().BaptismDamageData(damageData);

            if (finalDamage >= 0)
            {
                this.TheUnitBelongto.GetComponent<UnitAttributesDataComponent>().NumericComponent
                    .ApplyChange(NumericType.Hp, -finalDamage);
                //抛出伤害事件
                this.GetBuffTarget().BelongToRoom.GetComponent<BattleEventSystemComponent>()
                    .Run($"ExcuteDamage{this.TheUnitFrom.Id}", damageData);
                //抛出受伤事件
                this.GetBuffTarget().BelongToRoom.GetComponent<BattleEventSystemComponent>()
                    .Run($"TakeDamage{this.GetBuffTarget().Id}", damageData);
            }

            //设置下一个时间点
            this.m_SelfNextExcuteFrame = currentFrame + TimeAndFrameConverter.Frame_Long2Frame(temp.WorkInternal);
        }
#else
        public override void OnExecute(uint currentFrame)
        {
            
        }
#endif
    }
}