//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年3月25日 15:52:25
//------------------------------------------------------------

using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 用于管理一个物理世界中所有的碰撞实体
    /// </summary>
    public class B2S_ColliderEntityManagerComponent: Component
    {
        /// <summary>
        /// 用于管理碰撞实体
        /// </summary>
        public Dictionary<long, B2S_ColliderEntity> AllColliderEntitys = new Dictionary<long, B2S_ColliderEntity>();

        public void AddColliderEntity(B2S_ColliderEntity b2SColliderEntity)
        {
            this.AllColliderEntitys.Add(b2SColliderEntity.Id, b2SColliderEntity);
        }

        public void RemoveColliderEntity(long id)
        {
            if (this.AllColliderEntitys.ContainsKey(id))
            {
                this.AllColliderEntitys.Remove(id);
            }
            else
            {
                Log.Error($"所要移除的碰撞实体ID：{id}不存在");
            }
        }

        public B2S_ColliderEntity GetColliderEntity(long id)
        {
            if (this.AllColliderEntitys.TryGetValue(id, out var b2SColliderEntity))
            {
                return b2SColliderEntity;
            }
            else
            {
                Log.Error($"所请求的碰撞实体ID：{id}不存在");
                return null;
            }
        }
    }
}