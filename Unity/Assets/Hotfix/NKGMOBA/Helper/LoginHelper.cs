using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class LoginHelper
    {
        private static bool isLogining;

        public static async ETVoid OnLoginAsync(string account, string password)
        {
            try
            {
                // 显示加载UI
                ETModel.Game.EventSystem.Run(ETModel.EventIdType.ShowLoadingUI);
                // 如果正在登录，就驳回登录请求，为了双重保险，点下登录按钮后，收到服务端响应之前将不能再点击
                if (isLogining)
                {
                    FinalRun();
                    return;
                }

                isLogining = true;

                if (account == "" || password == "")
                {
                    Game.EventSystem.Run(EventIdType.ShowLoginInfo, "账号或密码不能为空");
                    FinalRun();
                    return;
                }

                // 创建一个ETModel层的Session
                ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>()
                        .Create(GlobalConfigComponent.Instance.GlobalProto.Address);
                // 创建一个ETHotfix层的Session, ETHotfix的Session会通过ETModel层的Session发送消息
                Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);

                Game.EventSystem.Run(EventIdType.ShowLoginInfo, "登陆中。。。");
                // 发送登录请求，账号，密码均为传来的参数
                R2C_Login r2CLogin = (R2C_Login) await realmSession.Call(new C2R_Login() { Account = account, Password = password });

                if (r2CLogin.Error == ErrorCode.ERR_LoginError)
                {
                    Game.EventSystem.Run(EventIdType.ShowLoginInfo, "登录失败，账号或密码错误");
                    FinalRun();
                    return;
                }

                //释放realmSession
                realmSession.Dispose();

                // 创建一个ETModel层的Session,并且保存到ETModel.SessionComponent中
                ETModel.Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(r2CLogin.Address);
                ETModel.Game.Scene.AddComponent<ETModel.SessionComponent>().Session = gateSession;

                // 创建一个ETHotfix层的Session, 并且保存到ETHotfix.SessionComponent中
                Game.Scene.AddComponent<SessionComponent>().Session = ComponentFactory.Create<Session, ETModel.Session>(gateSession);

                // 增加客户端断线处理组件
                Game.Scene.GetComponent<SessionComponent>().Session.AddComponent<SessionOfflineComponent>();

                // 增加心跳组件
                ETModel.Game.Scene.GetComponent<ETModel.SessionComponent>().Session.AddComponent<HeartBeatComponent>();

                await SessionComponent.Instance.Session.Call(new C2G_LoginGate() { Key = r2CLogin.Key });

                Game.EventSystem.Run(EventIdType.ShowLoginInfo, "登录成功");

                // 创建Player(抽象化的玩家),这里的id是数据库里的账号id
                Player player = ETModel.ComponentFactory.CreateWithId<Player>(r2CLogin.PlayerId);
                PlayerComponent playerComponent = ETModel.Game.Scene.GetComponent<PlayerComponent>();
                playerComponent.MyPlayer = player;

                // 登录完成
                Game.EventSystem.Run(EventIdType.LoginFinish);

                // 测试消息有成员是class类型
                // G2C_PlayerInfo g2CPlayerInfo = (G2C_PlayerInfo) await SessionComponent.Instance.Session.Call(new C2G_PlayerInfo());
                // Debug.Log("测试玩家信息为" + g2CPlayerInfo.Message);
                FinalRun();
            }
            catch (Exception e)
            {
                Log.Error(e);
                FinalRun();
            }
        }

        private static void FinalRun()
        {
            //设置登录处理完成状态
            isLogining = false;
            if (((FUILogin) Game.Scene.GetComponent<FUIComponent>().Get(FUILogin.UIPackageName)) != null)
                ((FUILogin) Game.Scene.GetComponent<FUIComponent>().Get(FUILogin.UIPackageName)).Btn_Login.GObject.asButton
                        .visible =
                        true;
            // 关闭加载UI
            ETModel.Game.EventSystem.Run(ETModel.EventIdType.CloseLoadingUI);
        }
    }
}