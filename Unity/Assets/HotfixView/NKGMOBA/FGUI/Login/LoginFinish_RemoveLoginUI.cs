

namespace ET
{
	public class LoginFinish_RemoveLoginUI: AEvent<EventType.LoginGateFinish>
	{
		protected override async ETTask Run(EventType.LoginGateFinish args)
		{
			args.ZoneScene.GetComponent<FUIManagerComponent>().Remove(FUIPackage.Login);
			await ETTask.CompletedTask;
		}
	}
}
