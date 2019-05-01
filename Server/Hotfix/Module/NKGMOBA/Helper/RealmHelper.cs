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
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async ETTask KickOutPlayer(long userId)
        {
            //验证账号是否在线，在线则踢下线
            int gateAppId = Game.Scene.GetComponent<OnlineComponent>().Get(userId);
            if (gateAppId != 0)
            {
                StartConfig userGateConfig = Game.Scene.GetComponent<StartConfigComponent>().Get(gateAppId);
                IPEndPoint userGateIPEndPoint = userGateConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session userGateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(userGateIPEndPoint);
                await userGateSession.Call(new R2G_PlayerKickOut() { UserID = userId });

                Console.WriteLine($"玩家{userId}已被踢下线");
            }
        }
    }
}