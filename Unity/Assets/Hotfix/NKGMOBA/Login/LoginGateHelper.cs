using System;
using UnityEngine;


namespace ET
{
    public static class LoginGateHelper
    {
        public static async ETTask Login(Entity fuiComponent, string address, string account, string password)
        {
            try
            {
                // 创建一个ET层的Session
                R2C_Login r2CLogin;
                Scene zoneScene = fuiComponent.DomainScene();
                Session session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                {
                    FUI_LoadingComponent.ShowLoadingUI();
                    r2CLogin = (R2C_Login) await session.Call(new C2R_Login() {Account = account, Password = password});
                    FUI_LoadingComponent.HideLoadingUI();
                    if (r2CLogin.Error == ErrorCode.ERR_LoginError)
                    {
                        await Game.EventSystem.Publish(new EventType.LoginOrRegsiteFail()
                            {ErrorMessage = "登陆失败，密码或用户名不正确。", ZoneScene = zoneScene});
                        session.Dispose();
                        return;
                    }
                }
                session.Dispose();

                PlayerPrefs.SetString("LoginAccount", account);
                PlayerPrefs.SetString("LoginPassWord", password);
                
                // 创建一个gate Session,并且保存到SessionComponent中
                Session gateSession = zoneScene.GetComponent<NetKcpComponent>()
                    .Create(NetworkHelper.ToIPEndPoint(r2CLogin.GateAddress));
                gateSession.AddComponent<PingComponent>();

                PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();
                playerComponent.GateSession = gateSession;

                G2C_LoginGate g2CLoginGate = (G2C_LoginGate) await gateSession.Call(
                    new C2G_LoginGate() {Key = r2CLogin.Key, GateId = r2CLogin.GateId});

                // 创建一个Lobby Session,并且保存到SessionComponent中
                Session lobbySession = zoneScene.GetComponent<NetKcpComponent>()
                    .Create(NetworkHelper.ToIPEndPoint(g2CLoginGate.LobbyAddress));
                
                playerComponent.LobbySession = lobbySession;
                playerComponent.PlayerId = g2CLoginGate.PlayerId;
                playerComponent.PlayerAccount = account;
                
                Log.Debug("登陆Gate成功!");

                await Game.EventSystem.Publish(new EventType.LoginGateFinish() {ZoneScene = zoneScene});
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                FUI_LoadingComponent.HideLoadingUI();
            }
        }
    }
}