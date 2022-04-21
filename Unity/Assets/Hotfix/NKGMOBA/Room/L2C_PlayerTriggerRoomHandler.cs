using ET.EventType;

namespace ET
{
    public class L2C_PlayerTriggerRoomHandler : AMHandler<L2C_PlayerTriggerRoom>
    {
        protected override async ETVoid Run(Session session, L2C_PlayerTriggerRoom message)
        {
            Room room = session.DomainScene().GetComponent<RoomManagerComponent>()
                .GetLobbyRoom(message.playerInfoRoom.RoomId);
            // 其他玩家操作房间 true为加入房间，false为离开房间，
            if (message.JoinOrLeave)
            {
                room.PlayerCount++;
                Game.EventSystem.Publish(new CreatePlayerCard()
                {
                    DomainScene = session.DomainScene(), PlayerId = message.playerInfoRoom.playerid,
                    PlayerAccount = message.playerInfoRoom.Account, RoomId = message.playerInfoRoom.RoomId
                }).Coroutine();
            }
            else
            {
                room.PlayerCount--;
                Game.EventSystem.Publish(new LeaveRoom()
                {
                    DomainScene = session.DomainScene(), PlayerId = message.playerInfoRoom.playerid,
                    PlayerAccount = message.playerInfoRoom.Account, RoomId = message.playerInfoRoom.RoomId
                }).Coroutine();
            }

            await ETTask.CompletedTask;
        }
    }
}