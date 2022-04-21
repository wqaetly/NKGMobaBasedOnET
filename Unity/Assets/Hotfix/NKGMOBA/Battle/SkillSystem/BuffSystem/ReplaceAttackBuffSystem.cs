//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月24日 14:35:15
//------------------------------------------------------------

using NPBehave;

namespace ET
{
    public class ReplaceAttackBuffSystem : ABuffSystemBase<ReplaceAttackBuffData>
    {
        public override void OnExecute(uint currentFrame)
        {
            ReplaceAttackBuffData replaceAttackBuffData = this.GetBuffDataWithTType;

            Unit unit = this.GetBuffTarget();
            
            // 默认重置一次普攻
            unit.GetComponent<CommonAttackComponent_Logic>().CancelCommonAttackWithOutResetTarget_ResetAttackCD();
            
            unit.GetComponent<CommonAttackComponent_Logic>().SetAttackReplaceInfo(this.BelongtoRuntimeTree.Id,
                replaceAttackBuffData.AttackReplaceInfo);
            unit.GetComponent<CommonAttackComponent_Logic>()
                .SetCancelAttackReplaceInfo(this.BelongtoRuntimeTree.Id, replaceAttackBuffData.CancelReplaceInfo);

            Blackboard blackboard =
                unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(this.BelongtoRuntimeTree.Id)
                    .GetBlackboard();
            blackboard.Set(replaceAttackBuffData.AttackReplaceInfo.BBKey, false);
            blackboard.Set(replaceAttackBuffData.CancelReplaceInfo.BBKey, false);

            //TODO 从当前战斗Entity获取BattleEventSystem来Run事件
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    this.GetBuffTarget().BelongToRoom.GetComponent<BattleEventSystemComponent>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }
        }

        public override void OnFinished(uint currentFrame)
        {
            ReplaceAttackBuffData replaceAttackBuffData = this.GetBuffDataWithTType;

            Unit unit = this.GetBuffTarget();
            unit.GetComponent<CommonAttackComponent_Logic>().ReSetAttackReplaceInfo();
            unit.GetComponent<CommonAttackComponent_Logic>().ReSetCancelAttackReplaceInfo();

            Blackboard blackboard =
                unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(this.BelongtoRuntimeTree.Id)
                    .GetBlackboard();

            blackboard.Set(replaceAttackBuffData.AttackReplaceInfo.BBKey, false);
            blackboard.Set(replaceAttackBuffData.CancelReplaceInfo.BBKey, false);
        }
    }
}