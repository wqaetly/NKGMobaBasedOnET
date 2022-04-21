using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 类似AIDispatcher，是全局的，整个进程只有一个，因为其本身就是一个无状态函数封装
    /// </summary>
    public class B2S_CollisionDispatcherComponent : Entity
    {
        public static B2S_CollisionDispatcherComponent Instance;

        public Dictionary<string, AB2S_CollisionHandler> B2SCollisionHandlers =
            new Dictionary<string, AB2S_CollisionHandler>();

        /// <summary>
        /// 处理碰撞开始，a碰到了b
        /// </summary>
        public void HandleCollisionStart(Unit a, Unit b)
        {
            if (B2SCollisionHandlers.TryGetValue(a.GetComponent<B2S_ColliderComponent>().CollisionHandlerName,
                out var collisionHandler))
            {
                collisionHandler.HandleCollisionStart(a, b);
            }
        }

        /// <summary>
        /// 处理碰撞持续
        /// </summary>
        public void HandleCollisionSustain(Unit a, Unit b)
        {
            if (B2SCollisionHandlers.TryGetValue(a.GetComponent<B2S_ColliderComponent>().CollisionHandlerName,
                out var collisionHandler))
            {
                collisionHandler.HandleCollisionSustain(a, b);
            }
        }

        /// <summary>
        /// 处理碰撞结束
        /// </summary>
        public void HandleCollsionEnd(Unit a, Unit b)
        {
            if (B2SCollisionHandlers.TryGetValue(a.GetComponent<B2S_ColliderComponent>().CollisionHandlerName,
                out var collisionHandler))
            {
                collisionHandler.HandleCollisionEnd(a, b);
            }
        }
    }
}