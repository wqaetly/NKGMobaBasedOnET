namespace ET
{
    [LSF_Tickable(EntityType = typeof(LSF_TimerComponent))]
    public class LSF_TimerComponentTicker : ALSF_TickHandler<LSF_TimerComponent>
    {
        public override void OnLSF_Tick(LSF_TimerComponent entity, uint currentFrame, long deltaTime)
        {
            entity.FixedUpdate();
        }
    }
}