//此文件格式由工具自动生成
using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("替换攻击流程Action",TitleAlignment = TitleAlignments.Centered)]
    public class NP_RegisteReplaceAttackAction:NP_ClassForStoreAction
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
#if SERVER
            BelongToUnit.GetComponent<CommonAttackComponent_Logic>().SetAttackReplaceInfo(this.BelongtoRuntimeTree.Id, AttackReplaceInfo);
            BelongToUnit.GetComponent<CommonAttackComponent_Logic>().SetCancelAttackReplaceInfo(this.BelongtoRuntimeTree.Id, CancelReplaceInfo);  
#endif
        }
    }
}
