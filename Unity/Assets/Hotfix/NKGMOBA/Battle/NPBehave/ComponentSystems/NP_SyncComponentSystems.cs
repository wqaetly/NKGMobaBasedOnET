using NPBehave;

namespace ET
{
    public class SyncComponentAwakeSystem: AwakeSystem<NP_SyncComponent>
    {
        public override void Awake(NP_SyncComponent self)
        {
            self.SyncContext = new SyncContext(self);
        }
    }
    
    public class SyncComponentDestroy: DestroySystem<NP_SyncComponent>
    {
        public override void Destroy(NP_SyncComponent self)
        {
            self.SyncContext = null;
        }
    }
    
    public static class NP_SyncComponentUtilities
    {
        public static void FixedUpdate(this NP_SyncComponent self, uint currentFrame)
        {
            self.SyncContext.Update();
        }
    }
}