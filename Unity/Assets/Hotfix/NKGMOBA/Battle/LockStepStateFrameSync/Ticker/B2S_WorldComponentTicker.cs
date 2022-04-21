namespace ET
{
    [LSF_Tickable(EntityType = typeof(B2S_WorldComponent))]
    public class B2S_WorldComponentTicker: ALSF_TickHandler<B2S_WorldComponent>
    {
        public override void OnLSF_Tick(B2S_WorldComponent entity, uint currentFrame, long deltaTime)
        {
            entity.FixedUpdate();
        }
    }
}