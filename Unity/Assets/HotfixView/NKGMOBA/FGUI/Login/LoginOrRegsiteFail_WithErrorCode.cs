namespace ET
{
    public class LoginOrRegsiteFail_WithErrorCode : AEvent<EventType.LoginOrRegsiteFail>
    {
        protected override async ETTask Run(EventType.LoginOrRegsiteFail args)
        {
            FUI_Login fuiLogin = args.ZoneScene.GetComponent<FUIManagerComponent>()
                .GetFUIComponent<FUI_LoginComponent>(FUIPackage.Login).FuiUIPanelLogin;
            if (fuiLogin != null)
            {
                fuiLogin.m_Tex_LoginInfo.text = args.ErrorMessage;
                fuiLogin.m_Tex_LoginInfo.visible = true;
                fuiLogin.m_Tween_LoginInfoFadeIn.Play();
            }

            await ETTask.CompletedTask;
        }
    }
}