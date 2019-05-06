//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月4日 16:30:09
//------------------------------------------------------------

using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class G2R_PlayerOffline_ReqHandler: AMRpcHandler<G2R_PlayerOffline, R2G_PlayerOffline>
    {
        protected override void Run(Session session, G2R_PlayerOffline message, Action<R2G_PlayerOffline> reply)
        {
            R2G_PlayerOffline response = new R2G_PlayerOffline();
            try
            {
                //玩家下线
                Game.Scene.GetComponent<OnlineComponent>().Remove(message.playerAccount);
                Console.WriteLine($"玩家{message.playerAccount}下线");

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}