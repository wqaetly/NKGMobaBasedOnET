//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:17:29
//------------------------------------------------------------

using System.Collections.Generic;
using Box2DSharp.Dynamics;

namespace ETModel
{
    /// <summary>
    /// 一个碰撞体,为一个实体
    /// </summary>
    public class B2S_ColliderEntity: Entity
    {
        /// <summary>
        /// 此数据结点ID(在碰撞关系数据载体中的)
        /// </summary>
        public long NodeDataId;

        /// <summary>
        /// 在碰撞体池中的标识ID
        /// </summary>
        public int FlagId;
        
        /// <summary>
        /// Box2D世界中的刚体
        /// </summary>
        public Body Body;

        /// <summary>
        /// 此刚体的Unit，一般而言，与Body同进退，共生死，可以看成ET下的Body，
        /// 比如诺克放一个Q，那么Unit就是这个Q技能
        /// </summary>
        public Unit SelfUnit;
        
        /// <summary>
        /// 所归属的Unit，就是挂有此碰撞体的目标单位，
        /// 比如诺克放一个Q，那么m_BelongUnit就是诺克
        /// </summary>
        public Unit BelongToUnit;

        /// <summary>
        /// 碰撞关系数据实例，同时依靠此实例来决定加载的碰撞体数据
        /// </summary>
        public B2S_CollisionInstance B2S_CollisionInstance;

        /// <summary>
        /// 碰撞体数据实例,因为一个Body可能有多个fixture
        /// </summary>
        public List<B2S_ColliderDataStructureBase> B2S_ColliderDataStructureBase = new List<B2S_ColliderDataStructureBase>();

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            this.Body.Dispose();
            this.B2S_CollisionInstance = null;
            this.B2S_ColliderDataStructureBase.Clear();
        }
    }
}