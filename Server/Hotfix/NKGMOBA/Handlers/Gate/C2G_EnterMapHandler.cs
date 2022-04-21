using System;


namespace ET
{
    [MessageHandler]
    public class C2G_EnterMapHandler : AMRpcHandler<C2G_EnterMap, G2C_EnterMap>
    {
        protected override async ETTask Run(Session session, C2G_EnterMap request, G2C_EnterMap response, Action reply)
        {
            //优化开局流程，废弃
            
            // Player player = session.GetComponent<SessionPlayerComponent>().Player;
            // long mapInstanceId = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "Map")
            //     .InstanceId;
            //
            // M2G_CreateHeroUnit createUnit = (M2G_CreateHeroUnit) await ActorMessageSenderComponent.Instance.Call(
            //     mapInstanceId, new G2M_CreateHeroUnit() {PlayerId = player.Id, GateSessionId = session.InstanceId,Roomid = player.RoomId});
            //
            // player.UnitId = createUnit.UnitId;
            // response.UnitId = createUnit.UnitId;
            // reply();
            await ETTask.CompletedTask;
        }
    }
}