namespace ET
{
    /// <summary>
    /// 添加此组件代表Unit已死亡，一些其他组件逻辑将暂时失灵
    /// 相应的，移除此组件，其他组件的逻辑将恢复正常
    /// </summary>
    public class DeadComponent : Entity
    {
        /// <summary>
        /// 复活时长，在经过这个时间之后DeadComponent将会被移除, 当前默认10s
        /// </summary>
        public long ResurrectionTime = 10000;

        public long DeadTimerId;
    }
}