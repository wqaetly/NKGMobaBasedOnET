//此文件格式由工具自动生成
using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("取消普攻(重置普攻CD)",TitleAlignment = TitleAlignments.Centered)]
    public class NP_CancelAttackAction:NP_ClassForStoreAction
    {        
        public override Action GetActionToBeDone()
        {
            this.Action = this.CancelAttackAction;
            return this.Action;
        }

        public void CancelAttackAction()
        {
#if SERVER
            this.BelongToUnit.GetComponent<CommonAttackComponent_Logic>().CancelCommonAttackWithOutResetTarget_ResetAttackCD(); 
#endif
        }
    }
}
