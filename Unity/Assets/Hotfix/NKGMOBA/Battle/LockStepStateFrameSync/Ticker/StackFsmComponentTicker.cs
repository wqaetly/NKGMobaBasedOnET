namespace ET
{
    public class StackFsmComponentTicker: ALSF_TickHandler<StackFsmComponent>
    {
        public override void OnLSF_Tick(StackFsmComponent entity, uint currentFrame, long deltaTime)
        {
            
        }

        /// <summary>
        /// 每帧末尾搜集脏数据进行同步
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="frame"></param>
        /// <param name="deltaTime"></param>
        public override void OnLSF_TickEnd(StackFsmComponent entity, uint frame, long deltaTime)
        {
            base.OnLSF_TickEnd(entity, frame, deltaTime);
        }
    }
}