//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月20日 18:44:35
//------------------------------------------------------------

using System.Numerics;
using Box2DSharp.Dynamics;

namespace ETModel
{
    public class B2S_WorldUtility
    {
        /// <summary>
        /// 创造一个物理世界
        /// </summary>
        /// <param name="gravity">重力加速度</param>
        /// <returns></returns>
        public static World CreateWorld(Vector2 gravity)
        {
            return new World(gravity);
        }
    }
}