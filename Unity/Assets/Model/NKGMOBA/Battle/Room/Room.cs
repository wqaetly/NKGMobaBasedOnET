using System.Collections.Generic;

namespace ET
{
    public class RoomAwakeSystem : AwakeSystem<Room>
    {
        public override void Awake(Room self)
        {
            
        }
    }
    
    public class RoomDestroySystem : DestroySystem<Room>
    {

        public override void Destroy(Room self)
        {
            self.RoomHolderPlayerId = 0;
            self.PlayerCount = 0;
            self.RoomName = "";
        }
    }

    /// <summary>
    /// 代表一个房间
    /// </summary>
    public class Room : Entity
    {
        /// <summary>
        /// 房主Id
        /// </summary>
        public long RoomHolderPlayerId;

        /// <summary>
        /// 房间人数
        /// </summary>
        public int PlayerCount;

        /// <summary>
        /// 房间名
        /// </summary>
        public string RoomName;

        public int PlayerMaxCount = 6;
    }
}