using Vector3 = UnityEngine.Vector3;

namespace ET
{
    [MessageHandler]
    public class M2C_CreateHeroUnitsHandler : AMHandler<M2C_CreateUnits>
    {
        protected override async ETVoid Run(Session session, M2C_CreateUnits message)
        {
            RoomManagerComponent roomManagerComponent = session.DomainScene().GetComponent<RoomManagerComponent>();
            Room lobbyRoom = roomManagerComponent.GetLobbyRoom(message.RoomId);
            
            //战斗房间，代表一场战斗
            Room battleRoom = roomManagerComponent.GetOrCreateBattleRoom();
            
            UnitComponent unitComponent = battleRoom.GetComponent<UnitComponent>();
            
            PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();

            foreach (UnitInfo unitInfo in message.Units)
            {
                if (unitComponent.Get(unitInfo.UnitId) != null)
                {
                    continue;
                }

                Unit unit = UnitFactory.CreateHero(battleRoom, unitInfo);

                playerComponent.HasCompletedLoadCount++;
                Log.Debug("playerComponent.HasCompletedLoadCount:" + playerComponent.HasCompletedLoadCount);
                if ((playerComponent.HasCompletedLoadCount == 1 && lobbyRoom.PlayerCount == 1) ||
                    (playerComponent.HasCompletedLoadCount == 2 && lobbyRoom.PlayerCount == 2 )||(playerComponent.HasCompletedLoadCount == 4 && lobbyRoom.PlayerCount == 4)||
                    (playerComponent.HasCompletedLoadCount == 6 && lobbyRoom.PlayerCount == 6)) //playerComponent.BelongToRoom.PlayerMaxCount)
                {
                    // 要取LobbySession才行，因为M2C的session为Gate
                    Session lobbySession = playerComponent.LobbySession;
                    lobbySession.Send(new C2L_PreparedToEnterBattle() {PlayerId = playerComponent.PlayerId});
                }
            }

            await ETTask.CompletedTask;
        }
    }
}