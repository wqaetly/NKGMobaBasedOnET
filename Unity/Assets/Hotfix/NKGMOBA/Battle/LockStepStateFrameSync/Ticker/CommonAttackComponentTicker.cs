namespace ET
{
    [LSF_Tickable(EntityType = typeof(CommonAttackComponent_Logic))]
    public class CommonAttackComponentTicker: ALSF_TickHandler<CommonAttackComponent_Logic>
    {
        public override bool OnLSF_CheckConsistency(CommonAttackComponent_Logic entity, uint frame, ALSF_Cmd stateToCompare)
        {
            if (stateToCompare is LSF_CommonAttackCmd commonAttackCmd)
            {
                commonAttackCmd.PassingConsistencyCheck = true;
            }

            return true;
        }

        public override void OnLSF_Tick(CommonAttackComponent_Logic entity, uint currentFrame, long deltaTime)
        {
            entity.FixedUpdate();
        }
    }
}