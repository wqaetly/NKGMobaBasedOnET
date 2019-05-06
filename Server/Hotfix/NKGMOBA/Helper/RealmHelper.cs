//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 19:42:21
//------------------------------------------------------------

using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
    public static class RealmHelper
    {
        /// <summary>
        /// 将玩家踢下线
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public static async ETTask KickOutPlayer(string playerAccount, PlayerOfflineTypes playerOfflineType)
        {
            //验证账号是否在线，在线则踢下线
            int gateAppId = Game.Scene.GetComponent<OnlineComponent>().GetGateAppId(playerAccount);
            if (gateAppId != 0)
            {
                // 获取内网gate，向realm发送离线信息
                StartConfig playerGateConfig = Game.Scene.GetComponent<StartConfigComponent>().Get(gateAppId);
                IPEndPoint playerGateIPEndPoint = playerGateConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session playerGateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(playerGateIPEndPoint);
                
                // 发送断线信息
                long playerId = Game.Scene.GetComponent<OnlineComponent>().GetPlayerId(playerAccount);
                Player player = Game.Scene.GetComponent<PlayerComponent>().Get(playerId);
                long playerSessionId = player.GetComponent<UnitGateComponent>().GateSessionActorId;
                Session lastGateSession = Game.Scene.GetComponent<NetOuterComponent>().Get(playerSessionId);
                
                switch (playerOfflineType)
                {
                    case PlayerOfflineTypes.NoPlayForLongTime:
                        // 因长时间未操作而强制下线
                        lastGateSession.Send(new G2C_PlayerOffline() { MPlayerOfflineType = 1 });
                        break;
                    case PlayerOfflineTypes.SamePlayerLogin:
                        // 因账号冲突而强制下线
                        lastGateSession.Send(new G2C_PlayerOffline() { MPlayerOfflineType = 2 });
                        break;
                }

                TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();
                await timerComponent.WaitAsync(5000);

                //服务端主动断开客户端连接
                await playerGateSession.Call(new R2G_PlayerKickOut() { PlayerAccount = playerAccount, PlayerId = playerId });

                Console.WriteLine($"玩家{playerId}已被踢下线");
            }
        }
    }
}