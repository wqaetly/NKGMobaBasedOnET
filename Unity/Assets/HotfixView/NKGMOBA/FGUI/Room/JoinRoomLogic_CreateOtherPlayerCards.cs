namespace ET
{
    public class JoinRoomLogic_CreateOtherPlayerCards : AEvent<EventType.JoinRoom>
    {
        protected override async ETTask Run(EventType.JoinRoom a)
        {
            FUIManagerComponent fuiManagerComponent = a.DomainScene.GetComponent<FUIManagerComponent>();

            FUI_RoomComponent fuiRoomComponent =
                fuiManagerComponent.GetFUIComponent<FUI_RoomComponent>(FUI_RoomComponent.FUIRoomName);

            fuiRoomComponent.FuiRoomList.Visible = false;
            fuiRoomComponent.FuiRoom.Visible = true;

            foreach (var playerInfoRoom in a.PlayerInfoRooms)
            {
                FUI_RoomUtilities.AddPlayerCard(fuiRoomComponent, playerInfoRoom.playerid, playerInfoRoom.Account,
                    a.Camp);
            }

            await ETTask.CompletedTask;
        }
    }
}