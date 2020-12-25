//此文件格式由工具自动生成

using System;
using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("重置普攻CD", TitleAlignment = TitleAlignments.Centered)]
    public class NP_ResetAttackCDAction: NP_ClassForStoreAction
    {
        public override Action GetActionToBeDone()
        {
            this.Action = this.ResetAttackCDAction;
            return this.Action;
        }

        public void ResetAttackCDAction()
        {
            CDComponent.Instance.ResetCD(this.BelongtoRuntimeTree.BelongToUnitId, "CommonAttack");
        }
    }
}