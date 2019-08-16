using System;
using System.Collections.Generic;
using System.Net;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2R_LoginHandler: AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply)
        {
            //数据库操作对象
            DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

            //验证账号密码是否正确
            List<ComponentWithId> result =
                    await dbProxy.Query<AccountInfo>(_account => _account.Account == request.Account && _account.Password == request.Password);
            if (result.Count == 0)
            {
                response.Error = ErrorCode.ERR_LoginError;
                reply();
                return;
            }

            // 随机分配一个Gate(内部)
            StartConfig config = Game.Scene.GetComponent<RealmGateAddressComponent>().GetAddress();
            IPEndPoint innerAddress = config.GetComponent<InnerConfig>().IPEndPoint;
            Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(innerAddress);

            // 向gate请求一个key,客户端可以拿着这个key连接gate,20秒失效
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey) await gateSession.Call(new R2G_GetLoginKey() { playerID = result[0].Id });

            string outerAddress = config.GetComponent<OuterConfig>().Address2;

            response.Address = outerAddress;
            response.Key = g2RGetLoginKey.Key;
            response.PlayerId = result[0].Id;
            reply();
        }
    }
}