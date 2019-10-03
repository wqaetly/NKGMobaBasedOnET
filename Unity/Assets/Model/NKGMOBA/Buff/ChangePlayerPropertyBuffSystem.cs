//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 12:19:22
//------------------------------------------------------------

namespace ETModel
{
    public class ChangePlayerPropertyBuffSystem: BuffSystemBase
    {
        public override void OnInit(BuffDataBase BuffDataBase, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.theUnitFrom = theUnitFrom;
            this.theUnitBelongto = theUnitBelongto;
            this.MSkillBuffDataBase = BuffDataBase;
            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, this.MSkillBuffDataBase);
        }

        public override void OnExecute()
        {
            Log.Info("自身添加了血怒Buff!!!!!!!!!!!!!!!!!!!!!");
            HeroDataComponent tempHeroDataComponent = this.theUnitBelongto.GetComponent<HeroDataComponent>();
            ChangePlayerPropertyBuffData tempChangePlayerPropertyBuffData = this.MSkillBuffDataBase as ChangePlayerPropertyBuffData;
            switch (this.MSkillBuffDataBase.BuffWorkType)
            {
                case BuffWorkTypes.ChangeAttackValue:
                    tempHeroDataComponent.CurrentAttackValue += tempChangePlayerPropertyBuffData.theValueWillBeAdded;
                    break;
            }

            this.MBuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if (this.MSkillBuffDataBase.SustainTime + 1 > 0)
            {
                if (TimeHelper.Now() >= this.MaxLimitTime)
                {
                    HeroDataComponent tempHeroDataComponent = this.theUnitBelongto.GetComponent<HeroDataComponent>();
                    ChangePlayerPropertyBuffData tempChangePlayerPropertyBuffData = this.MSkillBuffDataBase as ChangePlayerPropertyBuffData;
                    switch (this.MSkillBuffDataBase.BuffWorkType)
                    {
                        case BuffWorkTypes.ChangeAttackValue:
                            tempHeroDataComponent.CurrentAttackValue -= tempChangePlayerPropertyBuffData.theValueWillBeAdded;
                            break;
                    }

                    this.MBuffState = BuffState.Finished;
                }
            }
        }

        public override void OnFinished()
        {
            
        }
    }
}