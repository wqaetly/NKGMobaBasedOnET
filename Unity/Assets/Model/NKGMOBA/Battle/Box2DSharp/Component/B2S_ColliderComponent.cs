//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:17:29
//------------------------------------------------------------

using System.Collections.Generic;
using Box2DSharp.Dynamics;
using ET;

namespace ET
{
    /// <summary>
    /// 一个碰撞体Component,包含一个碰撞实例所有信息，直接挂载到碰撞体Unit上
    /// 比如诺手Q技能碰撞体UnitA，那么这个B2S_ColliderComponent的Entity就是UnitA，而其中的BelongToUnit就是诺手
    /// </summary>
    public class B2S_ColliderComponent: Entity
    {
        public B2S_WorldComponent WorldComponent;

        /// <summary>
        /// 碰撞关系表中的Id (Excel中的Id)
        /// </summary>
        public int B2S_CollisionRelationConfigId;
        
        /// <summary>
        /// 碰撞体数据表中的Id (Excel中的Id)
        /// </summary>
        public int B2S_ColliderDataConfigId;

        /// <summary>
        /// 碰撞处理者名称
        /// </summary>
        public string CollisionHandlerName;
        
        /// <summary>
        /// Box2D世界中的刚体
        /// </summary>
        public Body Body;

        /// <summary>
        /// 所归属的Unit，也就是产出碰撞体的Unit，
        /// 比如诺克放一个Q，那么BelongUnit就是诺克
        /// 需要注意的是，如果这个碰撞体需要同步位置，同步目标是Parent，而不是这个BelongToUnit
        /// </summary>
        public Unit BelongToUnit;

        /// <summary>
        /// 是否同步归属的UnitPos
        /// </summary>
        public bool SyncPosToBelongUnit;
        
        /// <summary>
        /// 是否同步归属的UnitRot
        /// </summary>
        public bool SyncRotToBelongUnit;

        /// <summary>
        /// 碰撞体数据实例
        /// </summary>
        public B2S_ColliderDataStructureBase B2S_ColliderDataStructureBase = new B2S_ColliderDataStructureBase();
    }
}