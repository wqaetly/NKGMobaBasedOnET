namespace ET
{
    [LSF_Tickable(EntityType = typeof(CDComponent))]
    public class CDComponentTicker: ALSF_TickHandler<CDComponent>
    {
        public override void OnLSF_Tick(CDComponent entity, uint currentFrame, long deltaTime)
        {
            entity.FixedUpdate(currentFrame);
        }
    }
}