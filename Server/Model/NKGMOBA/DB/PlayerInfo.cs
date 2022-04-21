using System.Collections.Generic;

namespace ET
{
    public class PlayerInfo: Entity
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name;

        /// <summary>
        /// 等级
        /// </summary>
        public int Level;

        /// <summary>
        /// 拥有的精灵
        /// </summary>
        public List<Entity> Pets;
    }
}