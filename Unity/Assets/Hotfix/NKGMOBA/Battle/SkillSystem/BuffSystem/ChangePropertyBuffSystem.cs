//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 12:19:22
//------------------------------------------------------------

namespace ET
{
    public class ChangePropertyBuffSystem : ABuffSystemBase<ChangePropertyBuffData>
    {
#if SERVER
        /// <summary>
        /// 之所以要缓存一下是因为某些修改器比较特殊
        /// 比如狗头的枯萎
        /// 内瑟斯使目标英雄衰老，持续5秒，减少其35%移动速度，在持续期间减速效果逐渐提升至47%/59%/71%/83%/95%。该目标被减少的攻击速度为该数值的一半。
        /// </summary>
        private ADataModifier dataModifier;

        public override void OnExecute(uint currentFrame)
        {
            switch (this.BuffData.BuffWorkType)
            {
                case BuffWorkTypes.ChangeAttackValue:
                    ConstantModifier constantModifier_AttackValue = ReferencePool.Acquire<ConstantModifier>();
                    constantModifier_AttackValue.ChangeValue = BuffDataCalculateHelper.CalculateCurrentData(this);
                    dataModifier = constantModifier_AttackValue;

                    this.GetBuffTarget().GetComponent<DataModifierComponent>()
                        .AddDataModifier(NumericType.AttackAdd.ToString(), dataModifier, NumericType.AttackAdd);
                    break;
                case BuffWorkTypes.ChangeMagic:
                    PercentageModifier constantModifier_Magic = ReferencePool.Acquire<PercentageModifier>();
                    constantModifier_Magic.Percentage = BuffDataCalculateHelper.CalculateCurrentData(this);
                    this.dataModifier = constantModifier_Magic;

                    this.GetBuffTarget().GetComponent<DataModifierComponent>()
                        .AddDataModifier(NumericType.Mp.ToString(), this.dataModifier, NumericType.Mp);
                    break;
                case BuffWorkTypes.ChangeSpeed:
                    PercentageModifier percentageModifier_Speed = ReferencePool.Acquire<PercentageModifier>();
                    percentageModifier_Speed.Percentage = BuffDataCalculateHelper.CalculateCurrentData(this);
                    this.dataModifier = percentageModifier_Speed;

                    this.GetBuffTarget().GetComponent<DataModifierComponent>()
                        .AddDataModifier(NumericType.Speed.ToString(), this.dataModifier, NumericType.Speed);
                    break;
            }
        }

        public override void OnFinished(uint currentFrame)
        {
            switch (this.BuffData.BuffWorkType)
            {
                case BuffWorkTypes.ChangeAttackValue:
                    this.GetBuffTarget().GetComponent<DataModifierComponent>()
                        .RemoveDataModifier(NumericType.AttackAdd.ToString(), dataModifier, NumericType.AttackAdd);
                    break;
                case BuffWorkTypes.ChangeMagic:
                    this.GetBuffTarget().GetComponent<DataModifierComponent>()
                        .RemoveDataModifier(NumericType.Mp.ToString(), dataModifier, NumericType.Mp);
                    break;
                case BuffWorkTypes.ChangeSpeed:
                    this.GetBuffTarget().GetComponent<DataModifierComponent>()
                        .RemoveDataModifier(NumericType.Speed.ToString(), dataModifier, NumericType.Speed);
                    break;
            }

            if (this.dataModifier == null)
            {
                return;
            }

            ReferencePool.Release(dataModifier);
            dataModifier = null;
        }
#else
        public override void OnExecute(uint currentFrame)
        {
        }
#endif
    }
}