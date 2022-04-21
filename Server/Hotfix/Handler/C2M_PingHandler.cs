using System;

namespace ET
{
    public class C2M_PingHandler : AMActorLocationRpcHandler<Unit, C2M_Ping, M2C_Ping>
    {
        protected override async ETTask Run(Unit unit, C2M_Ping request, M2C_Ping response, Action reply)
        {
            response.TimePoint = TimeHelper.ServerNow();
            response.ServerFrame = unit.BelongToRoom.GetComponent<LSF_Component>().CurrentFrame;
            reply();
            await ETTask.CompletedTask;
        }
    }
}