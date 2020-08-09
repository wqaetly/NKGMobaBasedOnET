//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年4月27日 17:35:10
//------------------------------------------------------------

using ETHotfix;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUILoginStartSystem: StartSystem<FUILogin>
    {
        public override void Start(FUILogin self)
        {
            self.Tex_LoginInfo.alpha = 0;
            self.Btn_Login.self.onClick.Add(() => LoginBtnOnClick(self));
            self.Btn_Registe.self.onClick.Add(() => RegisterBtnOnClick(self));
            self.ToTestSceneBtn.self.onClick.Add(() => ToTestSceneBtnBtnOnClick(self));
        }

        private void RegisterBtnOnClick(FUILogin self)
        {
            self.Btn_Registe.self.visible = false;
            RegisterHelper.OnRegisterAsync(self.accountText.text, self.passwordText.text).Coroutine();
        }

        public void LoginBtnOnClick(FUILogin self)
        {
            self.Btn_Login.self.visible = false;
            LoginHelper.OnLoginAsync(self.accountText.text, self.passwordText.text).Coroutine();
        }

        /// <summary>
        /// 前往训练营
        /// </summary>
        /// <param name="self"></param>
        public void ToTestSceneBtnBtnOnClick(FUILogin self)
        {
            self.Btn_Login.self.visible = false;
            LoginHelper.OnLoginAsync("Test123", "Test123").Coroutine();
        }
    }
    
}