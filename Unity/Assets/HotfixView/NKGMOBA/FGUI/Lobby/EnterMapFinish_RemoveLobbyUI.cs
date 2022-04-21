using UnityEngine;

namespace ET
{
	public class EnterMapFinish_RemoveLobbyUI: AEvent<EventType.PrepareEnterMap>
	{
		protected override async ETTask Run(EventType.PrepareEnterMap args)
		{
			Scene scene = args.ZoneScene;
			
			scene.GetComponent<FUIManagerComponent>().Remove(FUIPackage.Lobby);

			await ETTask.CompletedTask;
		}
	}
}
