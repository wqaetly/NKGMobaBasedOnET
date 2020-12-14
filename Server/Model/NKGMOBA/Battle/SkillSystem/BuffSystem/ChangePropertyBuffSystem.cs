//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 12:19:22
//------------------------------------------------------------

namespace ETModel
{
    public class ChangePropertyBuffSystem: ABuffSystemBase
    {
        /// <summary>
        /// 之所以要缓存一下是因为某些修改器比较特殊
        /// 比如狗头的枯萎
        /// 内瑟斯使目标英雄衰老，持续5秒，减少其35%移动速度，在持续期间减速效果逐渐提升至47%/59%/71%/83%/95%。该目标被减少的攻击速度为该数值的一半。
        /// </summary>
        private ADataModifier dataModifier;

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
            switch (this.BuffData.BuffWorkType)
            {
                case BuffWorkTypes.ChangeAttackValue:
                    ConstantModifier constantModifier = ReferencePool.Acquire<ConstantModifier>();
                    constantModifier.ChangeValue = BuffDataCalculateHelper.CalculateCurrentData(this,this.BuffData);
                    dataModifier = constantModifier;

                    this.GetBuffTarget().GetComponent<DataModifierComponent>()
                            .AddDataModifier(NumericType.AttackAdd.ToString(), dataModifier, NumericType.AttackAdd);
                    break;
            }

            this.BuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if (this.BuffData.SustainTime + 1 > 0)
            {
                if (TimeHelper.Now() >= this.MaxLimitTime)
                {
                    switch (this.BuffData.BuffWorkType)
                    {
                        case BuffWorkTypes.ChangeAttackValue:
                            this.GetBuffTarget().GetComponent<DataModifierComponent>()
                                    .RemoveDataModifier(NumericType.AttackAdd.ToString(), dataModifier, NumericType.AttackAdd);
                            break;
                    }

                    this.BuffState = BuffState.Finished;
                }
            }
        }

        public override void OnFinished()
        {
            ReferencePool.Release(dataModifier);
            dataModifier = null;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            //Log.Info("刷新了血怒Buff!!!!!!!!!!!!!!!!!!!!!");
        }
    }
}