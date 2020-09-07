//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 12:19:22
//------------------------------------------------------------

namespace ETModel
{
    public class ChangePropertyABuffSystem: ABuffSystemBase
    {
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
            //Log.Info("自身添加了血怒Buff!!!!!!!!!!!!!!!!!!!!!");
            HeroDataComponent tempHeroDataComponent = this.TheUnitBelongto.GetComponent<HeroDataComponent>();
            ChangePropertyBuffData tempChangePropertyBuffData = this.BuffData as ChangePropertyBuffData;
            switch (this.BuffData.BuffWorkType)
            {
                case BuffWorkTypes.ChangeAttackValue:
                    tempHeroDataComponent.CurrentAttackValue += tempChangePropertyBuffData.TheValueWillBeAdded;
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
                    HeroDataComponent tempHeroDataComponent = this.TheUnitBelongto.GetComponent<HeroDataComponent>();
                    ChangePropertyBuffData tempChangePropertyBuffData = this.BuffData as ChangePropertyBuffData;
                    switch (this.BuffData.BuffWorkType)
                    {
                        case BuffWorkTypes.ChangeAttackValue:
                            tempHeroDataComponent.CurrentAttackValue -= tempChangePropertyBuffData.TheValueWillBeAdded;
                            break;
                    }

                    this.BuffState = BuffState.Finished;
                }
            }
        }

        public override void OnFinished()
        {
            
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            //Log.Info("刷新了血怒Buff!!!!!!!!!!!!!!!!!!!!!");
        }
    }
}