//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月24日 17:52:12
//------------------------------------------------------------

using System;

namespace ETModel
{
    /// <summary>
    /// 碰撞响应组件，用来响应碰撞
    /// </summary>
    public class B2S_CollisionResponseComponent: Component
    {
        /// <summary>
        /// 碰撞开始事件
        /// </summary>
        public event Action<B2S_FixtureUserData> OnCollideStartAction;

        /// <summary>
        /// 碰撞结束事件
        /// </summary>
        public event Action<B2S_FixtureUserData> OnCollideFinishAction;

        /// <summary>
        /// 碰撞持续事件
        /// </summary>
        public event Action<B2S_FixtureUserData> OnCollideSustainAction;

        /// <summary>
        /// 碰撞开始
        /// </summary>
        /// <param name="b2SFixtureUserData"></param>
        public void OnCollideStart(B2S_FixtureUserData b2SFixtureUserData)
        {
            this.OnCollideStartAction?.Invoke(b2SFixtureUserData);
        }

        /// <summary>
        /// 碰撞结束
        /// </summary>
        /// <param name="b2SFixtureUserData"></param>
        public void OnCollideFinish(B2S_FixtureUserData b2SFixtureUserData)
        {
            this.OnCollideFinishAction?.Invoke(b2SFixtureUserData);
        }

        /// <summary>
        /// 碰撞持续
        /// </summary>
        /// <param name="b2SFixtureUserData"></param>
        public void OnCollideSustain(B2S_FixtureUserData b2SFixtureUserData)
        {
            this.OnCollideSustainAction?.Invoke(b2SFixtureUserData);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.IsDisposed)
            {
                return;
            }

            //清理已注册的委托
            foreach (var VARIABLE in this.OnCollideStartAction.GetInvocationList())
            {
                OnCollideStartAction -= VARIABLE as Action<B2S_FixtureUserData>; 
            }
            
            foreach (var VARIABLE in this.OnCollideFinishAction.GetInvocationList())
            {
                OnCollideFinishAction -= VARIABLE as Action<B2S_FixtureUserData>; 
            }

            foreach (var VARIABLE in this.OnCollideSustainAction.GetInvocationList())
            {
                OnCollideSustainAction -= VARIABLE as Action<B2S_FixtureUserData>; 
            }

        }
    }
}