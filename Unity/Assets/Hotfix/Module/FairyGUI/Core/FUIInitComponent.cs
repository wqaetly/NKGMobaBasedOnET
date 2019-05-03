using ETModel;
using System.Threading.Tasks;

namespace ETHotfix
{
	public class FUIInitComponent : Component
    {
        public async ETTask Init()
        {
            await ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackageAsync(FUIPackage.FUILogin);
            await ETModel.Game.Scene.GetComponent<FUIPackageComponent>().AddPackageAsync(FUIPackage.FUILobby);
        }

        public override void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			base.Dispose();

            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().RemovePackage(FUIPackage.FUILogin);
            ETModel.Game.Scene.GetComponent<FUIPackageComponent>().RemovePackage(FUIPackage.FUILobby);
        }
    }
}