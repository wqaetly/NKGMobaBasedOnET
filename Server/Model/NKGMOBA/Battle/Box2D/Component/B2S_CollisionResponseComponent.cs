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
    /// 挂载到B2S_HeroColliderData上
    /// </summary>
    public class B2S_CollisionResponseComponent: Component
    {
        /// <summary>
        /// 碰撞开始事件
        /// </summary>
        public event Action<Entity> OnCollideStartAction;

        /// <summary>
        /// 碰撞结束事件
        /// </summary>
        public event Action<Entity> OnCollideFinishAction;

        /// <summary>
        /// 碰撞持续事件
        /// </summary>,
        public event Action<Entity> OnCollideSustainAction;

        /// <summary>
        /// 碰撞开始
        /// </summary>
        /// <param name="b2SCollider"></param>
        public void OnCollideStart(Entity b2SCollider)
        {
            this.OnCollideStartAction?.Invoke(b2SCollider);
        }

        /// <summary>
        /// 碰撞结束
        /// </summary>
        /// <param name="b2SCollider"></param>
        public void OnCollideFinish(Entity b2SCollider)
        {
            this.OnCollideFinishAction?.Invoke(b2SCollider);
        }

        /// <summary>
        /// 碰撞持续
        /// </summary>
        /// <param name="b2SCollider"></param>
        public void OnCollideSustain(Entity b2SCollider)
        {
            this.OnCollideSustainAction?.Invoke(b2SCollider);
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
                OnCollideStartAction -= VARIABLE as Action<Entity>; 
            }
            
            foreach (var VARIABLE in this.OnCollideFinishAction.GetInvocationList())
            {
                OnCollideFinishAction -= VARIABLE as Action<Entity>; 
            }

            foreach (var VARIABLE in this.OnCollideSustainAction.GetInvocationList())
            {
                OnCollideSustainAction -= VARIABLE as Action<Entity>; 
            }

        }
    }
}