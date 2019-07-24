//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月24日 17:52:12
//------------------------------------------------------------

namespace ETModel
{
    /// <summary>
    /// 碰撞响应组件，用来响应碰撞
    /// </summary>
    public class B2S_CollisionResponseComponent: Component
    {
        /// <summary>
        /// 碰撞开始
        /// </summary>
        /// <param name="b2SFixtureUserData"></param>
        public void OnCollideStart(B2S_FixtureUserData b2SFixtureUserData)
        {
            
        }
        
        /// <summary>
        /// 碰撞结束
        /// </summary>
        /// <param name="b2SFixtureUserData"></param>
        public void OnCollideFinish(B2S_FixtureUserData b2SFixtureUserData)
        {
            
        }

        /// <summary>
        /// 碰撞持续
        /// </summary>
        /// <param name="b2SFixtureUserData"></param>
        public void OnCollideSustain(B2S_FixtureUserData b2SFixtureUserData)
        {
            
        }
    }
}