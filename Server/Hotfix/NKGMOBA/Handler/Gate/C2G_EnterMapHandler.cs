using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_EnterMapHandler: AMRpcHandler<C2G_EnterMap, G2C_EnterMap>
    {
        protected override async ETTask Run(Session session, C2G_EnterMap request, G2C_EnterMap response, Action reply)
        {
            //获取Player对象引用
            Player player = session.GetComponent<SessionPlayerComponent>().Player;
            
            // 在map服务器上创建战斗Unit
            IPEndPoint mapAddress = StartConfigComponent.Instance.MapConfigs[0].GetComponent<InnerConfig>().IPEndPoint;
            Session mapSession = Game.Scene.GetComponent<NetInnerComponent>().Get(mapAddress);
            
            //由gate服务器向map服务器发送创建战斗单位请求,这里的id是数据库里玩家账号id
            M2G_CreateUnit createUnit =
                    (M2G_CreateUnit) await mapSession.Call(new G2M_CreateUnit() { PlayerId = player.PlayerID, GateSessionId = session.Id });
            player.UnitId = createUnit.UnitId;
            response.UnitId = createUnit.UnitId;

            reply();
        }
    }
}