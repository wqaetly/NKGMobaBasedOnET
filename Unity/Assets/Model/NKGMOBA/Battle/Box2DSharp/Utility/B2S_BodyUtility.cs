//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月20日 18:47:57
//------------------------------------------------------------

using Box2DSharp.Dynamics;

namespace ET
{
    /// <summary>
    /// Box2D刚体实用函数集
    /// </summary>
    public static class B2S_BodyUtility
    {
        /// <summary>
        /// 创造一个动态刚体
        /// </summary>
        public static Body CreateDynamicBody(this B2S_WorldComponent self)
        {
            // 一定不准睡哦
            return self.GetWorld().CreateBody(new BodyDef() { BodyType = BodyType.DynamicBody , AllowSleep = false});
        }
        
        /// <summary>
        /// 创造一个静态刚体
        /// </summary>
        public static Body CreateStaticBody(this B2S_WorldComponent self)
        {
            return self.GetWorld().CreateBody(new BodyDef() { BodyType = BodyType.StaticBody });
        }
    }
}