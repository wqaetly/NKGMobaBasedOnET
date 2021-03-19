//此文件格式由工具自动生成

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ETModel
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
            Unit unit = UnitComponent.Instance.Get(this.BelongtoRuntimeTree.BelongToUnitId);

            List<NP_RuntimeTree> targetSkillCanvas = unit.GetComponent<SkillCanvasManagerComponent>()
                    .GetSkillCanvas(SkillIdWillbeNotified.GetBlackBoardValue<long>(this.BelongtoRuntimeTree.GetBlackboard()));

            foreach (var skillCanva in targetSkillCanvas)
            {
                skillCanva.GetBlackboard().Set("CastNormalAttack", true);
                skillCanva.GetBlackboard().Set("NormalAttackUnitIds",
                    CachedUnitIdsForAttack.GetBlackBoardValue<List<long>>(this.BelongtoRuntimeTree.GetBlackboard()));
            }

            CDComponent.Instance.TriggerCD(this.BelongtoRuntimeTree.BelongToUnitId, "CommonAttack");
            unit.GetComponent<CommonAttackComponent>().ReSetAttackReplaceInfo();  
#endif
        }
    }
}