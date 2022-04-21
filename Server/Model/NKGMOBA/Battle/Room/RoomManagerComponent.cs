using System.Collections.Generic;

namespace ET
{
    public class RoomManagerComponentAwakeSystem : AwakeSystem<RoomManagerComponent>
    {
        public override void Awake(RoomManagerComponent self)
        {
            self.RoomIdNum = 0;
        }
    }

    public class RoomManagerComponentDestroySystem : DestroySystem<RoomManagerComponent>
    {
        public override void Destroy(RoomManagerComponent self)
        {
            self.Rooms.Clear();
        }
    }

    public class RoomManagerComponent : Entity
    {
        public Dictionary<long, Room> Rooms = new Dictionary<long, Room>();
        public int RoomIdNum;

        public Room GetRoom(long id)
        {
            if (Rooms.TryGetValue(id, out var room))
            {
                return room;
            }
            else
            {
                //Log.Warning($"请求的Room Id不存在 ： {id}");  干掉报错
                return null;
            }
        }

        public void RemoveRoom(long id)
        {
            if (Rooms.TryGetValue(id, out var room))
            {
                room.Dispose();
                Rooms.Remove(id);
            }
        }
    }
}