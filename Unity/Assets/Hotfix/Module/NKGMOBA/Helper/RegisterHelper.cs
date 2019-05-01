//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 17:56:07
//------------------------------------------------------------

using System;
using ETModel;

namespace ETHotfix
{
    public static class RegisterHelper
    {
        private static bool isRegistering;

        public static async ETVoid OnRegisterAsync(string account, string password)
        {
            Session realmSession = null;
            try
            {
                // 如果正在注册，就驳回登录请求，为了双重保险，点下登录按钮后，收到服务端响应之前将不能再点击
                if (isRegistering) return;

                if (account == "" || password == "")
                {
                    Game.EventSystem.Run(EventIdType.ShowLoginInfo, "账号或密码不能为空");
                    return;
                }

                isRegistering = true;

                // 创建一个ETModel层的Session
                ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>()
                        .Create(GlobalConfigComponent.Instance.GlobalProto.Address);
                // 创建一个ETHotfix层的Session, ETHotfix的Session会通过ETModel层的Session发送消息
                realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);

                Game.EventSystem.Run(EventIdType.ShowLoginInfo, "正在注册");

                // 发送登录请求，账号，密码均为传来的参数
                R2C_Register r2CRegister = (R2C_Register) await realmSession.Call(new C2R_Register() { Account = account, Password = password });

                if (r2CRegister.Error == ErrorCode.ERR_AccountAlreadyRegister)
                {
                    Game.EventSystem.Run(EventIdType.ShowLoginInfo, "注册失败，账号已被注册");
                    return;
                }

                LoginHelper.OnLoginAsync(account, password).Coroutine();
            }
            catch (Exception e)
            {
                Game.EventSystem.Run(EventIdType.ShowLoginInfo, "注册异常");
            }
            finally
            {
                //释放realmSession
                realmSession?.Dispose();
                //设置注册处理完成状态
                isRegistering = false;
                ((FUILogin.FUILogin) Game.Scene.GetComponent<FUIComponent>().Get(FUILogin.FUILogin.UIPackageName)).registBtn.GObject.asButton
                        .visible =
                        true;
            }
        }
    }
}