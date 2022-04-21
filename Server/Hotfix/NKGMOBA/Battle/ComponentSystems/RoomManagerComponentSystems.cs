namespace ET
{
    public static class RoomManagerComponentSystems
    {
        public static Room CreateBattleRoom(this RoomManagerComponent self, long id)
        {
            Room room = self.AddChildWithId<Room>(id);

            room.AddComponent<UnitComponent>();
            room.AddComponent<LSF_Component>();
            room.AddComponent<LSF_TimerComponent>();
            room.AddComponent<RecastNavMeshManagerComponent>();
            room.AddComponent<CDComponent>();

            room.AddComponent<BattleEventSystemComponent>();
            room.AddComponent<B2S_WorldComponent>();
            room.AddComponent<B2S_WorldColliderManagerComponent>();
            room.AddComponent<B2S_CollisionListenerComponent>();

            room.AddComponent<LSF_TickComponent>();

            self.Rooms.Add(room.Id, room);
            return room;
        }

        public static Room CreateLobbyRoom(this RoomManagerComponent self, long id, int startGameNum)
        {
            Room room = self.AddChildWithId<Room>(id);
            room.startGameNum = startGameNum;
            room.enterNum = 0;
            room.ContainsPlayers.Clear();
            room.PlayersCamp.Clear();

            self.Rooms.Add(room.Id, room);
            return room;
        }
    }
}