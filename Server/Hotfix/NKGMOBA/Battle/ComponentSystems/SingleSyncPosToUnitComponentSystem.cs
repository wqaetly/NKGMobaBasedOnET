namespace ET
{
    public class SingleSyncPosToUnitComponentAwakeSystem: AwakeSystem<SingleSyncPosToUnitComponent, Unit>
    {
        public override void Awake(SingleSyncPosToUnitComponent self, Unit a)
        {
            self.UnitToSyncPos = a;
        }
    }
    
    public class SingleSyncPosToUnitComponentFixedUpdateSystem: FixedUpdateSystem<SingleSyncPosToUnitComponent>
    {
        public override void FixedUpdate(SingleSyncPosToUnitComponent self)
        {
            self.GetParent<Unit>().Position = self.UnitToSyncPos.Position;
        }
    }
    
    public class SingleSyncPosToUnitComponentDestroySystem: DestroySystem<SingleSyncPosToUnitComponent>
    {
        public override void Destroy(SingleSyncPosToUnitComponent self)
        {
            self.UnitToSyncPos = null;
        }
    }


    public class SingleSyncPosToUnitComponentSystem
    {
        
    }
}