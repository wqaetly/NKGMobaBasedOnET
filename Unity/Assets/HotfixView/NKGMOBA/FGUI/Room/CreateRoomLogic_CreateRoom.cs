using ET.EventType;

namespace ET
{
    public class CreateRoomLogic_CreateRoom : AEvent<EventType.CreateRoom>
    {
        protected override async ETTask Run(CreateRoom a)
        {
            FUIManagerComponent fuiManagerComponent = a.DomainScene.GetComponent<FUIManagerComponent>();

            FUI_RoomComponent fuiRoomComponent =
                fuiManagerComponent.GetFUIComponent<FUI_RoomComponent>(FUI_RoomComponent.FUIRoomListName);

            FUI_RoomUtilities.RefreshRoomListBaseOnRoomData(fuiRoomComponent);

            PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();

            FUI_RoomUtilities.AddPlayerCard(fuiRoomComponent, playerComponent.PlayerId, playerComponent.PlayerAccount,
                0);

            fuiRoomComponent.FuiRoomList.Visible = false;
            fuiRoomComponent.FuiRoom.Visible = true;

            await ETTask.CompletedTask;
        }
    }
}