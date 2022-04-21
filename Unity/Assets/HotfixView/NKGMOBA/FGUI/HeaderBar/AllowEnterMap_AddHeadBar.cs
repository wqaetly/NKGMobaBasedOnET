using ET.EventType;

namespace ET
{
    public class AllowEnterMap_AddHeadBar : AEvent<EventType.PrepareEnterMap>
    {
        protected override async ETTask Run(PrepareEnterMap a)
        {
            await a.ZoneScene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.HeadBar);

            UnitComponent unitComponent = a.ZoneScene.GetComponent<RoomManagerComponent>().BattleRoom.GetComponent<UnitComponent>();
            foreach (var heroUnit in unitComponent.GetAll())
            {
                FUI_HeadBar headBar = FUI_HeadBar.CreateInstance(a.ZoneScene);
                headBar.MakeFullScreen();

                a.ZoneScene.GetComponent<FUIManagerComponent>().Add($"{heroUnit.Id}_HeadBar", headBar, headBar);
                
                heroUnit.AddComponent<HeroHeadBarComponent, FUI_HeadBar>(headBar);
            }
            
            a.ZoneScene.AddComponent<OutLineComponent>();
            
            await ETTask.CompletedTask;
        }
    }
}