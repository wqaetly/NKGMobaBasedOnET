namespace ET
{
    /// <summary>
    /// 初始化战斗UI
    /// </summary>
    public class EnterMapFinish_Init : AEvent<EventType.PrepareEnterMap>
    {
        protected override async ETTask Run(EventType.PrepareEnterMap args)
        {
            Scene scene = args.ZoneScene;

            await scene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.BattleMain);
            FUI_Battle_Main fuiUIPanelBattle = await FUI_Battle_Main.CreateInstanceAsync(args.ZoneScene);
            fuiUIPanelBattle.self.MakeFullScreen();

            FUIManagerComponent fuiManagerComponent = scene.GetComponent<FUIManagerComponent>();
            FUI_BattleComponent fuiBattleComponent =
                fuiManagerComponent.AddChild<FUI_BattleComponent, FUI_Battle_Main>(fuiUIPanelBattle, true);

            scene.GetComponent<FUIManagerComponent>().Add(FUIPackage.BattleMain, fuiUIPanelBattle, fuiBattleComponent);

            Game.Scene.GetComponent<CameraComponent>().SetTargetUnit(scene.GetComponent<RoomManagerComponent>()
                .GetBattleRoom().GetComponent<UnitComponent>().MyUnit);

            await scene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.FlyFont);
        }
    }
}