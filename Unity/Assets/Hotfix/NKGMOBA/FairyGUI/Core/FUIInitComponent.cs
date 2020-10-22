using ETModel;

namespace ETHotfix
{
    public class FUIInitComponent: Component
    {
        public void Init()
        {
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIPackage.FUILogin);
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIPackage.FUIHeadBar);
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIPackage.FUILobby);
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackage(FUIPackage.FUIDialog);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().RemovePackage(FUIPackage.FUILogin);
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().RemovePackage(FUIPackage.FUIHeadBar);
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().RemovePackage(FUIPackage.FUILobby);
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().RemovePackage(FUIPackage.FUIDialog);
        }
    }
}