namespace ET
{
    public class LSF_CmdHandlerComponentAwakeSystem: AwakeSystem<LSF_CmdDispatcherComponent>
    {
        public override void Awake(LSF_CmdDispatcherComponent self)
        {
            self.Load();
            LSF_CmdDispatcherComponent.Instance = self;
        }
    }
    
    public class LSF_CmdHandlerComponentDestroySystem: DestroySystem<LSF_CmdDispatcherComponent>
    {
        public override void Destroy(LSF_CmdDispatcherComponent self)
        {
            LSF_CmdDispatcherComponent.Instance = null;
        }
    }
}