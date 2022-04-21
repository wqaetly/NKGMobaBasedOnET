using System;

namespace ET
{
    public class LeaveRoomHelper
    {
        public static async ETTask LeaveRoom(Entity fuiComponent)
        {
            try
            {
                Scene zoneScene = fuiComponent.DomainScene();

                PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();

                L2C_LeaveRoomLobby l2CLeaveRoomLobby = (L2C_LeaveRoomLobby) await playerComponent.LobbySession
                    .Call(new C2L_LeaveRoomLobby() {PlayerId = playerComponent.PlayerId});

                if (l2CLeaveRoomLobby.isDestory)
                {
                    zoneScene.GetComponent<RoomManagerComponent>().RemoveLobbyRoom(l2CLeaveRoomLobby.RoomId);
                }

                playerComponent.BelongToRoom = null;

                // 自己离开房间要清空本地所有玩家卡片
                Game.EventSystem
                    .Publish(new EventType.LeaveRoom()
                    {
                        DomainScene = fuiComponent.DomainScene(), PlayerId = l2CLeaveRoomLobby.PlayerId,
                        Camp = l2CLeaveRoomLobby.camp, RoomId = l2CLeaveRoomLobby.RoomId
                    })
                    .Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}