namespace ET
{
    public class AfterHeroSpilingCreate_CreateGO : AEvent<EventType.AfterHeroSpilingCreate_CreateGO>
    {
        protected override async ETTask Run(EventType.AfterHeroSpilingCreate_CreateGO args)
        {
            Client_UnitConfig clientUnitConfig = Client_UnitConfigCategory.Instance.Get(args.HeroSpilingConfigId);
            GameObjectComponent gameObjectComponent =
                args.Unit.AddComponent<GameObjectComponent, string>(clientUnitConfig.UnitName);

            gameObjectComponent.GameObject.transform.position =
                args.Unit.Position;

            args.Unit.AddComponent<AnimationComponent>();
            args.Unit.AddComponent<UnitTransformComponent>();
            args.Unit.AddComponent<TurnComponent>();
            args.Unit.AddComponent<EffectComponent>();
            args.Unit.AddComponent<CommonAttackComponent_View>();
            args.Unit.AddComponent<FallingFontComponent>();
            
            gameObjectComponent.GameObject.GetComponent<MonoBridge>().BelongToUnitId = args.Unit.Id;
            //英雄属性组件
            args.Unit.AddComponent<UnitAttributesDataComponent, long>(clientUnitConfig.UnitAttributesDataId);
            
            FUI_HeadBar headBar = FUI_HeadBar.CreateInstance(args.Unit.DomainScene());
            headBar.MakeFullScreen();
            args.Unit.DomainScene().GetComponent<FUIManagerComponent>().Add($"{args.Unit.Id}_HeadBar", headBar, headBar);
            args.Unit.AddComponent<HeroHeadBarComponent, FUI_HeadBar>(headBar);
            
            await ETTask.CompletedTask;
        }
    }
}