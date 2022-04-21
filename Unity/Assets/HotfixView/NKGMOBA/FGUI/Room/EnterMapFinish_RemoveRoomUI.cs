using UnityEngine;

namespace ET
{
	public class EnterMapFinish_RemoveRoomUI: AEvent<EventType.PrepareEnterMap>
	{
		protected override async ETTask Run(EventType.PrepareEnterMap args)
		{
			Scene scene = args.ZoneScene;

			FUIManagerComponent fuiManagerComponent = scene.GetComponent<FUIManagerComponent>();

			FUI_RoomComponent fuiRoomComponent = fuiManagerComponent.GetFUIComponent<FUI_RoomComponent>(FUI_RoomComponent.FUIRoomName);
			
			scene.GetComponent<RoomManagerComponent>().RemoveAllLobbyRooms();
			
			FUI_RoomUtilities.RefreshRoomListBaseOnRoomData(fuiRoomComponent);
			
			scene.GetComponent<FUIManagerComponent>().Remove(FUI_RoomComponent.FUIRoomName);

			await ETTask.CompletedTask;
		}
	}
}
