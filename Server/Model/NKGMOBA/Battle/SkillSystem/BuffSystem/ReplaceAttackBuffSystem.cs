//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月24日 14:35:15
//------------------------------------------------------------

using NPBehave;

namespace ETModel
{
    public class ReplaceAttackBuffSystem: ABuffSystemBase
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
            ReplaceAttackBuffData replaceAttackBuffData = this.BuffData as ReplaceAttackBuffData;
            
            Unit unit = UnitComponent.Instance.Get(this.GetBuffTarget().Id);
            unit.GetComponent<CommonAttackComponent>().SetAttackReplaceInfo(this.BelongtoRuntimeTree.Id, replaceAttackBuffData.AttackReplaceInfo);
            unit.GetComponent<CommonAttackComponent>()
                    .SetCancelAttackReplaceInfo(this.BelongtoRuntimeTree.Id, replaceAttackBuffData.CancelReplaceInfo);
            //TODO 从当前战斗Entity获取BattleEventSystem来Run事件
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystem>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }

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
            ReplaceAttackBuffData replaceAttackBuffData = this.BuffData as ReplaceAttackBuffData;
            
            Unit unit = UnitComponent.Instance.Get(this.GetBuffTarget().Id);
            unit.GetComponent<CommonAttackComponent>().ReSetAttackReplaceInfo();
            unit.GetComponent<CommonAttackComponent>().ReSetCancelAttackReplaceInfo();

            Blackboard blackboard = unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(this.BelongtoRuntimeTree.Id).GetBlackboard();

            blackboard.Set(replaceAttackBuffData.AttackReplaceInfo.BBKey, false);
            blackboard.Set(replaceAttackBuffData.CancelReplaceInfo.BBKey, false);
        }
    }
}