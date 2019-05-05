//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月2日 17:18:42
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.LoginFinish)]
    public class LoginSuccess_CloseLoginUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Remove(FUILogin.FUILogin.UIPackageName);
            //Game.Scene.GetComponent<FUIStackComponent>().Pop();
        }
    }
}