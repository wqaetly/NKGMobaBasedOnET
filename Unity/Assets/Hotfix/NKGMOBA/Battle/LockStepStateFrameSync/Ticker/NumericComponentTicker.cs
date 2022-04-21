using System.Collections.Generic;

namespace ET
{
    [LSF_Tickable(EntityType = typeof(NumericComponent))]
    public class NumericComponentTicker : ALSF_TickHandler<NumericComponent>
    {
        public override bool OnLSF_CheckConsistency(NumericComponent entity, uint frame, ALSF_Cmd stateToCompare)
        {
            // 默认本地不进行属性计算，所以每一条属性同步信息都属于不一致的数据
            if (stateToCompare is LSF_SyncAttributeCmd)
            {
                return false;
            }
            
            return true;
        }


        public override void OnLSF_TickStart(NumericComponent entity, uint frame, long deltaTime)
        {
#if SERVER
            entity.AttributeChangeFrameSnap[frame] = new Dictionary<int, float>();
            entity.AttributeReusltFrameSnap[frame] = new Dictionary<int, float>();
#endif
        }

        public override void OnLSF_Tick(NumericComponent entity, uint currentFrame, long deltaTime)
        {
        }

        public override void OnLSF_TickEnd(NumericComponent entity, uint frame, long deltaTime)
        {
#if SERVER
            if (entity.AttributeChangeFrameSnap[frame].Count > 0 || entity.AttributeReusltFrameSnap[frame].Count > 0)
            {
                LSF_SyncAttributeCmd syncAttributeCmd =
                    ReferencePool.Acquire<LSF_SyncAttributeCmd>().Init(entity.GetParent<Unit>().Id) as LSF_SyncAttributeCmd;
                
                syncAttributeCmd.Frame = frame;
                syncAttributeCmd.SyncAttributesChanged = entity.AttributeChangeFrameSnap[frame];
                syncAttributeCmd.SyncAttributesResult = entity.AttributeReusltFrameSnap[frame];

                entity.GetParent<Unit>().BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(syncAttributeCmd);
            }
#endif
        }
    }
}