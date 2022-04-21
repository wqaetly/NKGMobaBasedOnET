namespace ET
{
    [LSF_Tickable(EntityType = typeof(B2S_CollisionListenerComponent))]
    public class B2S_CollisionListenerComponentTicker: ALSF_TickHandler<B2S_CollisionListenerComponent>
    {
        public override void OnLSF_Tick(B2S_CollisionListenerComponent entity, uint currentFrame, long deltaTime)
        {
            entity.FixedUpdate();
        }
    }
}