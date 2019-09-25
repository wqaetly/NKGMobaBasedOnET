//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月20日 18:47:57
//------------------------------------------------------------

using Box2DSharp.Dynamics;

namespace ETModel
{
    /// <summary>
    /// Box2D刚体实用函数集
    /// </summary>
    public class B2S_BodyUtility
    {
        /// <summary>
        /// 创造一个动态刚体
        /// </summary>
        public static Body CreateDynamicBody()
        {
            return Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().CreateBody(new BodyDef() { BodyType = BodyType.DynamicBody });
        }
        
        /// <summary>
        /// 创造一个静态刚体
        /// </summary>
        public static Body CreateStaticBody()
        {
            return Game.Scene.GetComponent<B2S_WorldComponent>().GetWorld().CreateBody(new BodyDef() { BodyType = BodyType.DynamicBody });
        }
    }
}