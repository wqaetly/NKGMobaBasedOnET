//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:17:29
//------------------------------------------------------------

using System.Collections.Generic;
using Box2DSharp.Dynamics;
using ETMode;

namespace ETModel
{
    /// <summary>
    /// 一个碰撞体数据结点
    /// </summary>
    public class B2S_HeroColliderDataComponent:Component
    {
        /// <summary>
        /// 此数据结点ID
        /// </summary>
        public long ID;
        
        /// <summary>
        /// Box2D世界中的刚体
        /// </summary>
        public Body m_Body;

        /// <summary>
        /// 所归属的Unit
        /// </summary>
        public Unit m_Unit;

        /// <summary>
        /// 碰撞关系数据实例，依靠此实例来决定加载的碰撞体数据
        /// </summary>
        public B2S_CollisionInstance m_B2S_CollisionInstance;

        /// <summary>
        /// 碰撞体数据实例,因为一个Body可能有多个fixture
        /// </summary>
        public List<B2S_ColliderDataStructureBase> m_B2S_ColliderDataStructureBase;
    }
}