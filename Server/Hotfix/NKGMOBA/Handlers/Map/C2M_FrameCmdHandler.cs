namespace ET
{
    public class C2M_FrameCmdHandler : AMActorLocationHandler<Unit, C2M_FrameCmd>
    {
        protected override async ETTask Run(Unit entity, C2M_FrameCmd message)
        {
            LSF_Component lockStepStateFrameSyncComponent =
                entity.BelongToRoom.GetComponent<LSF_Component>();

            lockStepStateFrameSyncComponent.AddCmdToHandleQueue(message.CmdContent);
            await ETTask.CompletedTask;
        }
    }
}