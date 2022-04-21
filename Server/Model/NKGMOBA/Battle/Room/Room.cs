using System;
using System.Collections.Generic;

namespace ET
{
    public class RoomAwakeSystem : AwakeSystem<Room>
    {
        public override void Awake(Room self)
        {
            
        }
    }
    
    public class  RoomDestorySystem : DestroySystem<Room>
    {
        public override void Destroy(Room self)
        {
            self.enterNum = 0;
            self.RoomHolder = null;
            self.startGameNum = 0;
            self.ContainsPlayers.Clear();
            self.PlayersCamp.Clear();
        }
    }

    /// <summary>
    /// 代表一个房间
    /// </summary>
    public class Room : Entity
    {
        /// <summary>
        /// 房主
        /// </summary>
        public Player RoomHolder;

        public string RoomName;

        public int enterNum;

        public int startGameNum;
    
        /// <summary>
        /// 这个房间当前包含的玩家，包括房主
        /// </summary>
        public Dictionary<long, Player> ContainsPlayers = new Dictionary<long, Player>();

        /// <summary>
        /// 房间玩家对应的位置，也就是阵营
        /// </summary>
        public Dictionary<int, long> PlayersCamp = new Dictionary<int, long>();
    }
}