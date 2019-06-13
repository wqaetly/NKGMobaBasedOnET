//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 18:34:18
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2R_RegisterHandler: AMRpcHandler<C2R_Register, R2C_Register>
    {
        protected override async void Run(Session session, C2R_Register message, Action<R2C_Register> reply)
        {
            R2C_Register response = new R2C_Register();
            try
            {
                //数据库操作对象
                DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();

                //查询账号是否存在
                List<ComponentWithId> result = await dbProxyComponent.Query<AccountInfo>(_account => _account.Account == message.Account);

                if (result.Count > 0)
                {
                    response.Error = ErrorCode.ERR_AccountAlreadyRegister;
                    reply(response);
                    return;
                }

                //新建账号
                AccountInfo newAccount = ComponentFactory.CreateWithId<AccountInfo>(IdGenerater.GenerateId());
                newAccount.Account = message.Account;
                newAccount.Password = message.Password;

                //新建用户信息
                UserInfo newUser = ComponentFactory.CreateWithId<UserInfo>(newAccount.Id);
                newUser.NickName = $"召唤师{message.Account}";
                newUser.Level = 1;
                newUser.points = 10000;
                newUser.Diamods = 10000;
                newUser.Goldens = 10000;
                newUser._1v1Wins = 0;
                newUser._1v1Loses = 0;
                newUser._5v5Wins = 0;
                newUser._5v5Loses = 0;

                // 保存用户数据到数据库
                await dbProxyComponent.Save(newAccount);
                await dbProxyComponent.Save(newUser);

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}