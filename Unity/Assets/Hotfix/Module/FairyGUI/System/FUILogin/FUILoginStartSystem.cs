//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年4月27日 17:35:10
//------------------------------------------------------------

using System;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUILoginStartSystem: StartSystem<FUILogin.FUILogin>
    {
        public override void Start(FUILogin.FUILogin self)
        {
            self.loginInfo.alpha = 0;
            StartAsync(self).Coroutine();
        }

        private async ETVoid StartAsync(FUILogin.FUILogin self)
        {
            self.loginBtn.self.onClick.Add(() => LoginBtnOnClick(self));
            self.registBtn.self.onClick.Add(() => RegisterBtnOnClick(self));
        }

        private void RegisterBtnOnClick(FUILogin.FUILogin self)
        {
            self.registBtn.self.visible = false;
            RegisterHelper.OnRegisterAsync(self.accountText.text, self.passwordText.text).Coroutine();
        }

        public void LoginBtnOnClick(FUILogin.FUILogin self)
        {
            self.loginBtn.self.visible = false;
            LoginHelper.OnLoginAsync(self.accountText.text, self.passwordText.text).Coroutine();
        }
    }
}