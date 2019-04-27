//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年4月27日 17:35:10
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUILoginStartSystem: StartSystem<FUILogin.FUILogin>
    {
        public override void Start(FUILogin.FUILogin self)
        {
            StartAsync(self).Coroutine();
        }

        private async ETVoid StartAsync(FUILogin.FUILogin self)
        {
            self.loginBtn.GObject.asButton.onClick.Add(()=>LoginBtnOnClick(self));
        }

        public static void LoginBtnOnClick(FUILogin.FUILogin self)
        {
            LoginHelper.OnLoginAsync(self.accountText.text).Coroutine();
        }
    }
}