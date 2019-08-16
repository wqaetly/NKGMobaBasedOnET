//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月16日 16:41:08
//------------------------------------------------------------

using System;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class C2M_GetHeroDataRequestHandler: AMActorLocationRpcHandler<Unit, C2M_GetHeroDataRequest, M2C_GetHeroDataResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_GetHeroDataRequest message, M2C_GetHeroDataResponse response, Action reply)
        {
            response.HeroDataID = unit.GetComponent<HeroDataComponent>().NodeDataForHero.HeroID;
            reply();
            await ETTask.CompletedTask;
        }
    }
}