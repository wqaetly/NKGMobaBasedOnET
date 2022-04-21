namespace ET
{
    [LSF_Tickable(EntityType = typeof(NP_SyncComponent))]
    public class NP_SyncContextComponentTicker: ALSF_TickHandler<NP_SyncComponent>
    {
        public override void OnLSF_TickStart(NP_SyncComponent entity, uint frame, long deltaTime)
        {

        }

        public override void OnLSF_Tick(NP_SyncComponent entity, uint currentFrame, long deltaTime)
        {
// #if !SERVER
//             if (entity.GetParent<Unit>() == entity.GetParent<Unit>().BelongToRoom.GetComponent<UnitComponent>().MyUnit)
//             {
//                 Log.Error($"本地玩家{entity.GetParent<Unit>().Id}在 {currentFrame} tick");
//             }
//             else
//             {
//                 Log.Error($"远程玩家{entity.GetParent<Unit>().Id}在 {currentFrame} tick");
//             }
//
// #endif

            entity.FixedUpdate(currentFrame);
        }
    }
}