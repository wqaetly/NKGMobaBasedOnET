//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 21:21:33
//------------------------------------------------------------

using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class R2G_PlayerKickOut_ReqHandler: AMRpcHandler<R2G_PlayerKickOut, G2R_PlayerKickOut>
    {
        protected override async ETTask Run(Session session, R2G_PlayerKickOut message, G2R_PlayerKickOut response, Action reply)
        {
            //向登录服务器发送玩家离线消息
            StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
            IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
            Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);
            // 发送玩家离线消息
            await realmSession.Call(new G2R_PlayerOffline() { playerAccount = message.PlayerAccount });

            Player player = Game.Scene.GetComponent<PlayerComponent>().Get(message.PlayerId);

            //服务端主动断开客户端连接
            long playerSessionId = player.GetComponent<UnitGateComponent>().GateSessionActorId;
            Game.Scene.GetComponent<NetOuterComponent>().Remove(playerSessionId);

            reply();
            await ETTask.CompletedTask;
        }
    }
}