namespace ET
{
    public class LoginGateFinish_CreateLobbyUI : AEvent<EventType.LoginGateFinish>
    {
        protected override async ETTask Run(EventType.LoginGateFinish args)
        {
            Scene scene = args.ZoneScene;
            
            await scene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.Lobby);
            FUI_Lobby fuiLobby = await FUI_Lobby.CreateInstanceAsync(args.ZoneScene);
            fuiLobby.self.MakeFullScreen();
            
            FUIManagerComponent fuiManagerComponent = scene.GetComponent<FUIManagerComponent>();
            FUI_LobbyComponent fuiLobbyComponent = fuiManagerComponent.AddChild<FUI_LobbyComponent, FUI_Lobby>(fuiLobby);
            
            scene.GetComponent<FUIManagerComponent>().Add(FUIPackage.Lobby, fuiLobby, fuiLobbyComponent);

            await ETTask.CompletedTask;
        }
    }
}