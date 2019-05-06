//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月6日 13:23:13
//------------------------------------------------------------

using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_HeartBeatHandler: AMRpcHandler<C2G_HeartBeat, G2C_HeartBeat>
    {
        protected override void Run(Session session, C2G_HeartBeat message, Action<G2C_HeartBeat> reply)
        {
            if (session.GetComponent<HeartBeatComponent>() != null)
            {
                session.GetComponent<HeartBeatComponent>().CurrentTime = TimeHelper.ClientNowSeconds();
            }
            reply(new G2C_HeartBeat());
        }
    }
}