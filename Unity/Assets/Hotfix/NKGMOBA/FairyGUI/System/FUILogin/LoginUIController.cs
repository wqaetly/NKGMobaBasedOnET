using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.ShowLoginUI)]
    public class ShowLoginUI_CreateLoginUI: AEvent
    {
        public override void Run()
        {
            var m_loginui = FUILogin.FUILogin.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            m_loginui.Name = FUIPackage.FUILogin;
            m_loginui.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(m_loginui, true);
        }
    }
    
    [Event(EventIdType.LoginFinish)]
    public class LoginSuccess_CloseLoginUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Remove(FUIPackage.FUILogin);
        }
    }
}