using ETModel;
using ETModel.NKGMOBA.Battle.Fsm;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class Frame_ClickMapHandler: AMActorLocationHandler<Unit, Frame_ClickMap>
    {
        protected override async ETTask Run(Unit unit, Frame_ClickMap message)
        {
            Vector3 target = new Vector3(message.X, message.Y, message.Z);
            unit.GetComponent<UnitPathComponent>().CommonNavigate(target);

            await ETTask.CompletedTask;
        }
    }
}