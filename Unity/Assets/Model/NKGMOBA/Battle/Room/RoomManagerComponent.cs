using System.Collections.Generic;

namespace ET
{
    public class RoomManagerComponentAwakeSystem : AwakeSystem<RoomManagerComponent>
    {
        public override void Awake(RoomManagerComponent self)
        {
        }
    }

    public class RoomManagerComponentDestroySystem : DestroySystem<RoomManagerComponent>
    {
        public override void Destroy(RoomManagerComponent self)
        {
            self.LobbyRooms.Clear();
            
            self.BattleRoom?.Dispose();
        }
    }

    public class RoomManagerComponent : Entity
    {
        public Dictionary<long, Room> LobbyRooms = new Dictionary<long, Room>();
        
        /// <summary>
        /// 战斗房间
        /// </summary>
        public Room BattleRoom;

        public Room CreateLobbyRoom(long id)
        {
            Room room = this.AddChildWithId<Room>(id);
            LobbyRooms.Add(room.Id, room);
            return room;
        }
        
        public Room GetBattleRoom()
        {
            return BattleRoom;
        }

        public Room GetOrCreateBattleRoom()
        {
            if (BattleRoom == null)
            {
                BattleRoom = this.AddChild<Room>();

                BattleRoom.AddComponent<UnitComponent>();
                BattleRoom.AddComponent<LSF_Component>();
                BattleRoom.AddComponent<LSF_TimerComponent>();
                BattleRoom.AddComponent<MouseTargetSelectorComponent>();
                BattleRoom.AddComponent<MapClickCompoent>();
                BattleRoom.AddComponent<LSF_TickComponent>();
                BattleRoom.AddComponent<BattleEventSystemComponent>();
                BattleRoom.AddComponent<CDComponent>();
                BattleRoom.AddComponent<B2S_WorldComponent>();
            }

            return BattleRoom;
        }
        
        public Room GetLobbyRoom(long id)
        {
            if (LobbyRooms.TryGetValue(id, out var room))
            {
                return room;
            }
            else
            {
                Log.Warning($"请求的Room Id不存在 ： {id}");
                return null;
            }
        }

        /// <summary>
        /// 根据PlayerId获取其作为房主的房间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Room GetLobbyRoomByPlayerId(long playerId)
        {
            foreach (var room in LobbyRooms)
            {
                if (room.Value.RoomHolderPlayerId == playerId)
                {
                    return room.Value;
                }
            }

            Log.Error($"playerId作为房主的房间不存在 ： {playerId}");
            return null;
        }

        public void RemoveLobbyRoom(long id)
        {
            if (LobbyRooms.TryGetValue(id, out var room))
            {
                room.Dispose();
                LobbyRooms.Remove(id);
            }
        }

        public void RemoveAllLobbyRooms()
        {
            foreach (var room in LobbyRooms)
            {
                room.Value.Dispose();
            }

            LobbyRooms.Clear();
        }
    }
}