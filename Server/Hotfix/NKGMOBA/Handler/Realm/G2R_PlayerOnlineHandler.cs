//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月4日 16:31:26
//------------------------------------------------------------

using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class G2R_PlayerOnlineHandler: AMRpcHandler<G2R_PlayerOnline, R2G_PlayerOnline>
    {
        protected override async void Run(Session session, G2R_PlayerOnline message, Action<R2G_PlayerOnline> reply)
        {
            R2G_PlayerOnline response = new R2G_PlayerOnline();
            try
            {
                OnlineComponent onlineComponent = Game.Scene.GetComponent<OnlineComponent>();

                //将已在线玩家踢下线
                await RealmHelper.KickOutPlayer(message.playerAccount, PlayerOfflineTypes.SamePlayerLogin);

                //玩家上线
                onlineComponent.Add(message.playerAccount, message.PlayerId, message.GateAppID);
                Log.Info($"玩家{message.playerAccount}上线");

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}