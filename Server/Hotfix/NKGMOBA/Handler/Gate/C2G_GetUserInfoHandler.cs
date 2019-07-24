//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月13日 20:20:49
//------------------------------------------------------------

using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_GetUserInfoHandler: AMRpcHandler<C2G_GetUserInfo, G2C_GetUserInfo>
    {
        protected override async ETTask Run(Session session, C2G_GetUserInfo message, G2C_GetUserInfo response, Action reply)
        {
            //查询用户信息
            DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            UserInfo userInfo = await dbProxyComponent.Query<UserInfo>(message.PlayerId);

            response.UserName = userInfo.NickName;
            response.Level = userInfo.Level;
            response.Point = userInfo.points;
            response.Goldens = userInfo.Goldens;
            response.Diamods = userInfo.Diamods;

            reply();
        }
    }
}