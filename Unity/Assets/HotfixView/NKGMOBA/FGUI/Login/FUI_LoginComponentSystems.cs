//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月29日 9:41:17
//------------------------------------------------------------

using UnityEngine;

namespace ET
{
    public class FUILoginComponentAwakeSystem : AwakeSystem<FUI_LoginComponent, FUI_Login>
    {
        public override void Awake(FUI_LoginComponent self, FUI_Login fuiLogin)
        {
            string recordAccount = PlayerPrefs.GetString("LoginAccount");
            string recordPassWord = PlayerPrefs.GetString("LoginPassWord");
            if (!string.IsNullOrEmpty(recordAccount) && !string.IsNullOrEmpty(recordPassWord))
            {
                fuiLogin.m_accountText.text = recordAccount;
                fuiLogin.m_passwordText.text = recordPassWord;
            }
            
            fuiLogin.m_Btn_Login.self.onClick.Add(() => { FUI_LoginUtilities.OnLogin(self); });
            fuiLogin.m_Btn_Registe.self.onClick.Add(() => { FUI_LoginUtilities.OnRegister(self); });
            
            self.FuiUIPanelLogin = fuiLogin;
            self.FuiUIPanelLogin.m_Tex_LoginInfo.visible = false;
            self.FuiUIPanelLogin.m_Tween_LoginPanelFlyIn.Play();
        }
    }

    public class FUILoginComponentUpdateSystem : UpdateSystem<FUI_LoginComponent>
    {
        public override void Update(FUI_LoginComponent self)
        {
        }
    }

    public class FUILoginComponentFixedUpdateSystem : FixedUpdateSystem<FUI_LoginComponent>
    {
        public override void FixedUpdate(FUI_LoginComponent self)
        {
            //Log.Info($"定序执行 ： {Time.time}");
        }
    }
    
    public class FUILoginComponentDestroySystem : DestroySystem<FUI_LoginComponent>
    {
        public override void Destroy(FUI_LoginComponent self)
        {
            self.FuiUIPanelLogin = null;
        }
    }
}