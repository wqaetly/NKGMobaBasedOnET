//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月2日 17:09:27
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUILobbyStartSystem: StartSystem<FUILobby.FUILobby>
    {
        public override void Start(FUILobby.FUILobby self)
        {
            GetUserInfo().Coroutine();
            self.normalPVPBtn.self.onClick.Add(() => this.EnterMapAsync());
        }

        private async ETVoid GetUserInfo()
        {
            G2C_GetUserInfo g2CGetUserInfo = (G2C_GetUserInfo) await Game.Scene.GetComponent<SessionComponent>().Session
                    .Call(new C2G_GetUserInfo() { PlayerId = ETModel.Game.Scene.GetComponent<PlayerComponent>().MyPlayer.Id });

            FUILobby.FUILobby fuiLobby = (FUILobby.FUILobby) Game.Scene.GetComponent<FUIComponent>().Get(FUIPackage.FUILobby);

            fuiLobby.userName.text = g2CGetUserInfo.UserName;
            fuiLobby.UserLevel.text = "Lv " + g2CGetUserInfo.Level;
            fuiLobby.m_goldenInfo.text = g2CGetUserInfo.Goldens.ToString();
            fuiLobby.m_pointInfo.text = g2CGetUserInfo.Point.ToString();
            fuiLobby.m_gemInfo.text = g2CGetUserInfo.Diamods.ToString();

            Game.EventSystem.Run(EventIdType.LobbyUIAllDataLoadComplete);
        }

        private void EnterMapAsync()
        {
            ETModel.Game.EventSystem.Run(ETModel.EventIdType.ShowLoadingUI);
            MapHelper.EnterMapAsync().Coroutine();
        }
    }
}