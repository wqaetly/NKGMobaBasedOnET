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
    /// 一个碰撞体Component,包含一个碰撞实例所有信息，直接挂载到碰撞体Unit上
    /// 比如诺手Q技能碰撞体UnitA，那么这个B2S_ColliderComponent的Entity就是UnitA，而其中的BelongToUnit就是诺手
    /// </summary>
    public class B2S_ColliderComponent: Component
    {
        /// <summary>
        /// 此数据结点ID(在碰撞关系数据载体中的)
        /// </summary>
        public long NodeDataId;

        /// <summary>
        /// Box2D世界中的刚体
        /// </summary>
        public Body Body;

        /// <summary>
        /// 所归属的Unit，也就是产出碰撞体的Unit，
        /// 比如诺克放一个Q，那么BelongUnit就是诺克
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
            Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().DestroyBody(this.Body);
            this.B2S_CollisionInstance = null;
            this.B2S_ColliderDataStructureBase.Clear();
        }
    }
}