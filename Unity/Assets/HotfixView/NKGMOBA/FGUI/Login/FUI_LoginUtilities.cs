//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月29日 10:34:57
//------------------------------------------------------------

namespace ET
{
    public static class FUI_LoginUtilities
    {
        public static void OnLogin(FUI_LoginComponent self)
        {
            FUI_Login fuiLogin = self.FuiUIPanelLogin;
            LoginGateHelper.Login(self, GlobalDefine.GetLoginAddress(), fuiLogin.m_accountText.text,
                fuiLogin.m_passwordText.text).Coroutine();
        }
        
        public static void OnRegister(FUI_LoginComponent self)
        {
            FUI_Login fuiLogin = self.FuiUIPanelLogin;
            RegisteHelper.Register(self, GlobalDefine.GetLoginAddress(), fuiLogin.m_accountText.text,
                fuiLogin.m_passwordText.text).Coroutine();
        }
    }
}