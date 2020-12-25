//此文件格式由工具自动生成

using System;
using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("替换攻击流程Action", TitleAlignment = TitleAlignments.Centered)]
    public class NP_RegisteReplaceAttackAction: NP_ClassForStoreAction
    {
        [LabelText("替换攻击")]
        public NP_BlackBoardRelationData AttackReplaceInfo = new NP_BlackBoardRelationData();

        [LabelText("替换取消攻击")]
        public NP_BlackBoardRelationData CancelReplaceInfo = new NP_BlackBoardRelationData();

        public override Action GetActionToBeDone()
        {
            this.Action = this.RegisteReplaceAttackAction;
            return this.Action;
        }

        public void RegisteReplaceAttackAction()
        {
            Unit unit = UnitComponent.Instance.Get(this.Unitid);
            unit.GetComponent<CommonAttackComponent>().SetAttackReplaceInfo(this.BelongtoRuntimeTree.Id, AttackReplaceInfo);
            unit.GetComponent<CommonAttackComponent>().SetCancelAttackReplaceInfo(this.BelongtoRuntimeTree.Id, CancelReplaceInfo);
        }
    }
}