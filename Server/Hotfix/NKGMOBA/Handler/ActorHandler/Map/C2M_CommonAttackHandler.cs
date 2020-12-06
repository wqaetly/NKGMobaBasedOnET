//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月21日 15:25:33
//------------------------------------------------------------

using System;
using ETModel;
using ETModel.NKGMOBA.Battle.Fsm;
using ETModel.NKGMOBA.Battle.State;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class C2M_CommonAttackHandler: AMActorLocationHandler<Unit, C2M_CommonAttack>
    {
        protected override async ETTask Run(Unit unit, C2M_CommonAttack request)
        {
            unit.GetComponent<CommonAttackComponent>().SetAttackTarget(UnitComponent.Instance.Get(request.TargetUnitId));
            await ETTask.CompletedTask;
        }
    }
}