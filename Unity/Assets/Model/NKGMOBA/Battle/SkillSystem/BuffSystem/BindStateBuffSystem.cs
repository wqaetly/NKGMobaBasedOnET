//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月1日 21:22:19
//------------------------------------------------------------

using ETModel.NKGMOBA.Battle.State;

namespace ETModel
{
    /// <summary>
    /// 绑定一个状态
    /// </summary>
    public class BindStateBuffSystem: ABuffSystemBase
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
            ExcuteInternal();
            this.BuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if (this.BuffData.SustainTime + 1 > 0)
            {
                if (TimeHelper.Now() > this.MaxLimitTime)
                {
                    this.BuffState = BuffState.Finished;
                }
            }
        }

        public override void OnFinished()
        {
            BindStateBuffData tempData = this.BuffData as BindStateBuffData;
            if (tempData.OriState != null)
            {
                this.GetBuffTarget().GetComponent<StackFsmComponent>().RemoveState(tempData.OriState.StateName);
            }
        }

        public override void OnRefresh()
        {
            ExcuteInternal();
        }

        private void ExcuteInternal()
        {
            BindStateBuffData tempData = this.BuffData as BindStateBuffData;

            foreach (var buffData in tempData.OriBuff)
            {
                buffData.AutoAddBuff(this.BuffData.BelongToBuffDataSupportorId, buffData.BuffNodeId.Value,
                    this.TheUnitFrom, this.TheUnitBelongto, this.BelongtoRuntimeTree);
            }

            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystem>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }

            if (tempData.OriState != null)
            {
                this.GetBuffTarget().GetComponent<StackFsmComponent>().ChangeState(tempData.OriState);
            }
        }
    }
}