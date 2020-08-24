//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:17:29
//------------------------------------------------------------

using System.Collections.Generic;
using Box2DSharp.Dynamics;
using ETModel;

namespace ETModel
{
    /// <summary>
    /// 一个碰撞体,为一个实体
    /// </summary>
    public class B2S_ColliderEntity: Entity
    {
        /// <summary>
        /// 此数据结点ID
        /// </summary>
        public long ID;

        /// <summary>
        /// 在碰撞体池中的标识ID
        /// </summary>
        public int flagID;
        
        /// <summary>
        /// Box2D世界中的刚体
        /// </summary>
        public Body m_Body;

        /// <summary>
        /// 此刚体的Unit，一般而言，与Body同进退，共生死，可以看成ET下的Body，
        /// 比如诺克放一个Q，那么m_Unit就是这个Q技能
        /// </summary>
        public Unit m_Unit;
        
        /// <summary>
        /// 所归属的Unit，就是挂有此碰撞体的目标单位，
        /// 比如诺克放一个Q，那么m_BelongUnit就是诺克
        /// </summary>
        public Unit m_BelongUnit;

        /// <summary>
        /// 碰撞关系数据实例，依靠此实例来决定加载的碰撞体数据
        /// </summary>
        public B2S_CollisionInstance m_B2S_CollisionInstance;

        /// <summary>
        /// 碰撞体数据实例,因为一个Body可能有多个fixture
        /// </summary>
        public List<B2S_ColliderDataStructureBase> m_B2S_ColliderDataStructureBase = new List<B2S_ColliderDataStructureBase>();

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            this.m_Body.Dispose();
            m_B2S_CollisionInstance = null;
            this.m_B2S_ColliderDataStructureBase.Clear();
        }
    }
}