namespace ET
{
    public class L2C_PrepareToEnterBattleHandler: AMHandler<L2C_PrepareToEnterBattle>
    {
        protected override async ETVoid Run(Session session, L2C_PrepareToEnterBattle message)
        {
            //TODO 单独的加载界面UI
            FUI_LoadingComponent.ShowLoadingUI();
            await XAssetLoader.LoadSceneAsync(XAssetPathUtilities.GetScenePath("Map"));
            await ETTask.CompletedTask;
        }
    }
}