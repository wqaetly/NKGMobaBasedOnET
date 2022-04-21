namespace ET
{
    public class B2S_CollisionHandlerAttribute : BaseAttribute
    {
    }

    [B2S_CollisionHandler]
    public abstract class AB2S_CollisionHandler
    {
        /// <summary>
        /// a是碰撞者自身，b是碰撞到的目标
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public abstract void HandleCollisionStart(Unit a, Unit b);

        /// <summary>
        /// a是碰撞者自身，b是碰撞到的目标
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public abstract void HandleCollisionSustain(Unit a, Unit b);

        /// <summary>
        /// a是碰撞者自身，b是碰撞到的目标
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public abstract void HandleCollisionEnd(Unit a, Unit b);
    }
}