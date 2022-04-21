using System;
using ET.EventType;

namespace ET
{
    public static class CreateRoomHelper
    {
        public static async ETTask CreateRoom(Entity fuiComponent)
        {
            try
            {
                Scene zoneScene = fuiComponent.DomainScene();

                PlayerComponent playerComponent = Game.Scene
                    .GetComponent<PlayerComponent>();
                
                L2C_CreateNewRoomLobby l2CCreateNewRoomLobby = (L2C_CreateNewRoomLobby)await playerComponent.LobbySession
                    .Call(new C2L_CreateNewRoomLobby()
                        {PlayerId = playerComponent.PlayerId});

                Room room = zoneScene.GetComponent<RoomManagerComponent>().CreateLobbyRoom(l2CCreateNewRoomLobby.RoomId);
                
                room.RoomHolderPlayerId = playerComponent.PlayerId;
                room.RoomName = l2CCreateNewRoomLobby.Message;
                room.PlayerCount = 1;

                playerComponent.BelongToRoom = room;
                
                Game.EventSystem.Publish(new CreateRoom(){DomainScene = zoneScene}).Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}