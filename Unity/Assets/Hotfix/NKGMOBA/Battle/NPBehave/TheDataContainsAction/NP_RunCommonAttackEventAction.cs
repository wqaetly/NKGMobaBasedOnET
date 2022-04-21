//此文件格式由工具自动生成

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("分发平A事件", TitleAlignment = TitleAlignments.Centered)]
    public class NP_RunCommonAttackEvent: NP_ClassForStoreAction
    {
        [LabelText("要通知的技能Id")]
        public NP_BlackBoardRelationData SkillIdWillbeNotified = new NP_BlackBoardRelationData();

        [LabelText("受到攻击的UnitId")]
        public NP_BlackBoardRelationData CachedUnitIdsForAttack = new NP_BlackBoardRelationData();

        public override Action GetActionToBeDone()
        {
            this.Action = this.RunCommonAttackEvent;
            return this.Action;
        }

        public void RunCommonAttackEvent()
        {
#if SERVER
            List<NP_RuntimeTree> targetSkillCanvas = BelongToUnit.GetComponent<SkillCanvasManagerComponent>()
                    .GetSkillCanvas(SkillIdWillbeNotified.GetBlackBoardValue<long>(this.BelongtoRuntimeTree.GetBlackboard()));

            //注意这里一定不能引用本黑板中的Id数据，因为行为树的事件是下一帧才响应的，但是在当前帧的不久后的将来，本黑板相关数据可能会被重置！
            //List<long> attackIds = CachedUnitIdsForAttack.GetBlackBoardValue<List<long>>(this.BelongtoRuntimeTree.GetBlackboard());
            List<long> attackIds = new List<long>();
            foreach (var id in CachedUnitIdsForAttack.GetBlackBoardValue<List<long>>(this.BelongtoRuntimeTree.GetBlackboard()))
            {
                attackIds.Add(id);
            }
            foreach (var skillCanva in targetSkillCanvas)
            {
                skillCanva.GetBlackboard().Set("CastNormalAttack", true);
                skillCanva.GetBlackboard().Set("NormalAttackUnitIds", attackIds);
            }

            CDComponent.Instance.TriggerCD(this.BelongToUnit.Id, "CommonAttack");
#endif
        }
    }
}