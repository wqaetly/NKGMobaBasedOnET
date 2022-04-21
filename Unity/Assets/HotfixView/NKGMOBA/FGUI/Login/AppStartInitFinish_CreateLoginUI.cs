namespace ET
{
    public class AppStartInitFinish_CreateLoginUI : AEvent<EventType.AppStartInitFinish>
    {
        protected override async ETTask Run(EventType.AppStartInitFinish args)
        {
            Scene scene = args.ZoneScene;
            await scene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.Login);
            FUI_Login fuiLogin = await FUI_Login.CreateInstanceAsync(args.ZoneScene);
            
            // 自动适配
            fuiLogin.self.MakeFullScreen();
            
            FUIManagerComponent fuiManagerComponent = scene.GetComponent<FUIManagerComponent>();
            
            FUI_LoginComponent fuiLoginComponent =
                fuiManagerComponent.AddChild<FUI_LoginComponent, FUI_Login>(fuiLogin);
            scene.GetComponent<FUIManagerComponent>().Add(FUIPackage.Login, fuiLogin, fuiLoginComponent);

            await ETTask.CompletedTask;
        }
    }
}