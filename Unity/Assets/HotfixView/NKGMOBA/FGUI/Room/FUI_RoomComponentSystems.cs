namespace ET
{
    public class FUI_RoomComponentAwakeSystem : AwakeSystem<FUI_RoomComponent, FUI_RoomList, FUI_Room>
    {
        public override void Awake(FUI_RoomComponent self, FUI_RoomList fuiRoomList, FUI_Room fuiRoom)
        {
            fuiRoom.Visible = false;
            fuiRoomList.Visible = true;

            fuiRoomList.m_CreateButton.self.onClick.Add(() => { CreateRoomHelper.CreateRoom(self).Coroutine(); });
            fuiRoomList.m_RefreshButton.self.onClick.Add(() => { });
            fuiRoomList.m_QutiButton.self.onClick.Add(() => { });

            fuiRoom.m_Btn_StartGame.self.onClick.Add(() => { StartGameHelper.StartGame(self); });
            fuiRoom.m_Btn_QuitRoom.self.onClick.Add(() =>
            {
                LeaveRoomHelper.LeaveRoom(self);
            });

            self.FuiRoomList = fuiRoomList;
            self.FuiRoom = fuiRoom;
            
            FUI_RoomUtilities.RefreshRoomListBaseOnRoomData(self);
        }
    }

    public class FUI_RoomComponentDestroySystem : DestroySystem<FUI_RoomComponent>
    {
        public override void Destroy(FUI_RoomComponent self)
        {
            Scene scene = self.DomainScene();
            FUIManagerComponent fuiManagerComponent = scene.GetComponent<FUIManagerComponent>();
            RoomManagerComponent roomManagerComponent = self.DomainScene().GetComponent<RoomManagerComponent>();

            roomManagerComponent.RemoveAllLobbyRooms();

            fuiManagerComponent.Remove(FUI_RoomComponent.FUIRoomName);
            fuiManagerComponent.Remove(FUI_RoomComponent.FUIRoomListName);
        }
    }
}