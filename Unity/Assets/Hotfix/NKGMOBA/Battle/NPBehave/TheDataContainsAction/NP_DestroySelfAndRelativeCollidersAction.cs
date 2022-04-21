//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月17日 9:13:36
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ET
{
    [Title("销毁自己和相关联的Collider", TitleAlignment = TitleAlignments.Centered)]
    public class NP_DestroySelfAndRelativeCollidersAction: NP_ClassForStoreAction
    {
        public override System.Action GetActionToBeDone()
        {
            this.Action = this.DestroySelfAndRelativeCollider;
            return this.Action;
        }

        public void DestroySelfAndRelativeCollider()
        {
            UnitComponent unitComponent = BelongToUnit.BelongToRoom
                .GetComponent<UnitComponent>();
            unitComponent.Remove(this.BelongtoRuntimeTree.BelongToUnit.Id);
        }
    }
}