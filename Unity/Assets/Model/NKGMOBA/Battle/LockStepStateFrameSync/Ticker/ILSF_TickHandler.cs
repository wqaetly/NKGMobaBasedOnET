namespace ET
{
    public interface ILSF_TickHandler
    {
        /// <summary>
        /// 当前帧Tick开始，可用于做数据准备工作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        void LSF_TickStart(Entity entity, uint frame, long deltaTime);
        
        /// <summary>
        /// 正常Tick
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        void LSF_Tick(Entity entity, uint currentFrame, long deltaTime);
        
        /// <summary>
        /// 当前帧所有Tick都结束了，可用于做数据收集工作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        void LSF_TickEnd(Entity entity, uint frame, long deltaTime);

#if !SERVER
        /// <summary>
        /// 检测结果一致性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="alsfCmd"></param>
        bool LSF_CheckConsistency(Entity entity, uint frame, ALSF_Cmd stateToCompare);

        /// <summary>
        /// 视图层Tick
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="alsfCmd"></param>
        void LSF_ViewTick(Entity entity, long deltaTime);

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        void LSF_RollBackTick(Entity entity, uint frame, ALSF_Cmd stateToCompare);
#endif
    }
}