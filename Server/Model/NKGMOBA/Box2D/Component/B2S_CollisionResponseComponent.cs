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
        public event Action<B2S_HeroColliderData> OnCollideStartAction;

        /// <summary>
        /// 碰撞结束事件
        /// </summary>
        public event Action<B2S_HeroColliderData> OnCollideFinishAction;

        /// <summary>
        /// 碰撞持续事件
        /// </summary>,
        public event Action<B2S_HeroColliderData> OnCollideSustainAction;

        /// <summary>
        /// 碰撞开始
        /// </summary>
        /// <param name="b2SFixtureUserData"></param>
        public void OnCollideStart(B2S_HeroColliderData b2SHeroColliderData)
        {
            this.OnCollideStartAction?.Invoke(b2SHeroColliderData);
        }

        /// <summary>
        /// 碰撞结束
        /// </summary>
        /// <param name="b2SFixtureUserData"></param>
        public void OnCollideFinish(B2S_HeroColliderData b2SHeroColliderData)
        {
            this.OnCollideFinishAction?.Invoke(b2SHeroColliderData);
        }

        /// <summary>
        /// 碰撞持续
        /// </summary>
        /// <param name="b2SFixtureUserData"></param>
        public void OnCollideSustain(B2S_HeroColliderData b2SHeroColliderData)
        {
            this.OnCollideSustainAction?.Invoke(b2SHeroColliderData);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            
            //清理已注册的委托
            foreach (var VARIABLE in this.OnCollideStartAction.GetInvocationList())
            {
                OnCollideStartAction -= VARIABLE as Action<B2S_HeroColliderData>; 
            }
            
            foreach (var VARIABLE in this.OnCollideFinishAction.GetInvocationList())
            {
                OnCollideFinishAction -= VARIABLE as Action<B2S_HeroColliderData>; 
            }

            foreach (var VARIABLE in this.OnCollideSustainAction.GetInvocationList())
            {
                OnCollideSustainAction -= VARIABLE as Action<B2S_HeroColliderData>; 
            }

        }
    }
}