//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年3月28日 21:58:45
//------------------------------------------------------------

namespace ETModel
{
    public class RefreshTargetBuffTimeBuffSystem: BuffSystemBase
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
            RefreshTargetBuffTimeBuffData refreshTargetBuffTimeBuffData = this.MSkillBuffDataBase as RefreshTargetBuffTimeBuffData;
            BuffManagerComponent buffManagerComponent;
            if (MSkillBuffDataBase.BuffTargetTypes == BuffTargetTypes.Self)
            {
                buffManagerComponent = this.theUnitFrom.GetComponent<BuffManagerComponent>();
            }
            else
            {
                buffManagerComponent = theUnitBelongto.GetComponent<BuffManagerComponent>();
            }

            foreach (var buffId in refreshTargetBuffTimeBuffData.TheBuffsIDToBeRefreshed)
            {
                //Log.Info($"准备刷新指定Buff——{buffId}持续时间");
                BuffSystemBase buffSystemBase = buffManagerComponent.GetBuffByFlagID(buffId);
                if (buffSystemBase != null && buffSystemBase.MSkillBuffDataBase.SustainTime + 1 > 0)
                {
                    // Log.Info(
                    //     $"刷新了指定Buff——{buffId}持续时间{TimeHelper.Now() + buffSystemBase.MSkillBuffDataBase.SustainTime},原本为{buffSystemBase.MaxLimitTime}");
                    buffSystemBase.MaxLimitTime = TimeHelper.Now() + buffSystemBase.MSkillBuffDataBase.SustainTime;
                }
            }

            this.MBuffState = BuffState.Finished;
        }

        public override void OnFinished()
        {
        }
    }
}