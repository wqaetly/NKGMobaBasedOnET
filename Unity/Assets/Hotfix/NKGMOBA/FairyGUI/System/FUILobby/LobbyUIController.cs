//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月2日 17:11:17
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 登录完成，创建大厅UI
    /// </summary>
    [Event(EventIdType.LoginFinish)]
    public class LoginSuccess_CreateLobbyUI: AEvent
    {
        public override void Run()
        {
            var hotfixui = FUILobby.FUILobby.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = FUIPackage.FUILobby;
            hotfixui.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }
    
    [Event(EventIdType.CloseLobbyUI)]
    [Event(EventIdType.EnterMapFinish)]
    public class CloseLobbyUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Remove(FUIPackage.FUILobby);
        }
    }
}