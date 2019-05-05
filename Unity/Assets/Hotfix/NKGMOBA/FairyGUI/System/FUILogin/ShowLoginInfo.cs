//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 20:33:55
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 显示登录信息
    /// </summary>
    [Event(EventIdType.ShowLoginInfo)]
    public class ShowLoginInfo: AEvent<string>
    {
        public override void Run(string info)
        {
            ((FUILogin.FUILogin) Game.Scene.GetComponent<FUIComponent>().Get(FUILogin.FUILogin.UIPackageName)).loginInfo.text = info;
            ((FUILogin.FUILogin) Game.Scene.GetComponent<FUIComponent>().Get(FUILogin.FUILogin.UIPackageName)).t1.Play();
        }
    }
}