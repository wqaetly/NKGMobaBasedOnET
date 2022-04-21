namespace ET
{
    [LSF_Tickable(EntityType = typeof(UnitComponent))]
    public class UnitComponentTicker : ALSF_TickHandler<UnitComponent>
    {
        public override void OnLSF_TickStart(UnitComponent entity, uint frame, long deltaTime)
        {
            foreach (var allUnit in entity.idUnits)
            {
                LSF_TickDispatcherComponent.Instance.HandleLSF_TickStart(allUnit.Value, frame, deltaTime);
            }
        }

        public override void OnLSF_Tick(UnitComponent entity, uint currentFrame, long deltaTime)
        {
#if !SERVER
            if (entity.GetParent<Room>().GetComponent<LSF_Component>().IsInChaseFrameState)
            {
                LSF_TickDispatcherComponent.Instance.HandleLSF_Tick(entity.MyUnit, currentFrame, deltaTime);
                return;
            }
#endif

            using (ListComponent<Unit> unitsToTick = new ListComponent<Unit>())
            {
                foreach (var allUnit in entity.idUnits)
                {
                    unitsToTick.List.Add(allUnit.Value);
                }

                foreach (var unitToTick in unitsToTick.List)
                {
                    if (entity.idUnits.TryGetValue(unitToTick.Id, out var unit))
                    {
                        LSF_TickDispatcherComponent.Instance.HandleLSF_Tick(unit, currentFrame, deltaTime);
                    }
                }
            }
        }

        public override void OnLSF_TickEnd(UnitComponent entity, uint frame, long deltaTime)
        {
            foreach (var allUnit in entity.idUnits)
            {
                LSF_TickDispatcherComponent.Instance.HandleLSF_TickEnd(allUnit.Value, frame, deltaTime);
            }
        }

#if !SERVER
        public override void OnLSF_ViewTick(UnitComponent entity, long deltaTime)
        {
            foreach (var allUnit in entity.idUnits)
            {
                LSF_TickDispatcherComponent.Instance.HandleLSF_ViewTick(allUnit.Value, deltaTime);
            }
        }

        public override void OnLSF_RollBackTick(UnitComponent entity, uint frame, ALSF_Cmd stateToCompare)
        {
            LSF_TickDispatcherComponent.Instance.HandleLSF_RollBack(entity.MyUnit, frame,
                stateToCompare);
        }

        public override bool OnLSF_CheckConsistency(UnitComponent entity, uint frame, ALSF_Cmd stateToCompare)
        {
            return LSF_TickDispatcherComponent.Instance.HandleLSF_CheckConsistency(entity.MyUnit, frame,
                stateToCompare);
        }
#endif
    }
}