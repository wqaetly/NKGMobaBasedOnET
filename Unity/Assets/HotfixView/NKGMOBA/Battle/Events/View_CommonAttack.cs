using ET.EventType;

namespace ET
{
    public class View_CommonAttack: AEvent<EventType.CommonAttack>
    {
        protected override async ETTask Run(CommonAttack a)
        {
            a.AttackCast.GetComponent<CommonAttackComponent_View>().CommonAttackStart(a.AttackTarget);
            await ETTask.CompletedTask;
        }
    }
    
    public class View_CancelCommonAttack: AEvent<EventType.CancelCommonAttack>
    {
        protected override async ETTask Run(CancelCommonAttack a)
        {
            a.AttackCast.GetComponent<CommonAttackComponent_View>().CancelCommonAttack();
            await ETTask.CompletedTask;
        }
    }
}