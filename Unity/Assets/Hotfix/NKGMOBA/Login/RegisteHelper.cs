using System;

namespace ET
{
    public class RegisteHelper
    {
        public static async ETTask Register(Entity fuiComponent, string address, string account, string password)
        {
            try
            {
                Scene zoneScene = fuiComponent.DomainScene();

                if (account == "" || password == "")
                {
                    await Game.EventSystem.Publish(new EventType.LoginOrRegsiteFail()
                        {ErrorMessage = "账号名或密码不能为空", ZoneScene = zoneScene});
                    return;
                }

                R2C_Registe r2CRegiste;

                Session session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                {
                    FUI_LoadingComponent.ShowLoadingUI();
                    r2CRegiste = (R2C_Registe) await session.Call(new C2R_Registe()
                        {Account = account, Password = password});
                    FUI_LoadingComponent.HideLoadingUI();
                    if (r2CRegiste.Error == ErrorCode.ERR_AccountHasExist)
                    {
                        await Game.EventSystem.Publish(new EventType.LoginOrRegsiteFail()
                            {ErrorMessage = "账号已存在", ZoneScene = zoneScene});
                        session.Dispose();
                        return;
                    }
                }
                session.Dispose();
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