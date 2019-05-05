//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 19:42:21
//------------------------------------------------------------

using System;
using System.Net;
using ETModel;

namespace ETHotfix.Helper
{
    public static class RealmHelper
    {
        /// <summary>
        /// 将玩家踢下线
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public static async ETTask KickOutPlayer(string playerAccount)
        {
            //验证账号是否在线，在线则踢下线
            int gateAppId = Game.Scene.GetComponent<OnlineComponent>().GetGateAppId(playerAccount);
            if (gateAppId != 0)
            {
                long playerId = Game.Scene.GetComponent<OnlineComponent>().GetPlayerId(playerAccount);
                StartConfig playerGateConfig = Game.Scene.GetComponent<StartConfigComponent>().Get(gateAppId);
                IPEndPoint playerGateIPEndPoint = playerGateConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session playerGateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(playerGateIPEndPoint);

                await playerGateSession.Call(new R2G_PlayerKickOut() { playerAccount = playerAccount, PlayerId = playerId });

                Console.WriteLine($"玩家{playerId}已被踢下线");
            }
        }
    }
}