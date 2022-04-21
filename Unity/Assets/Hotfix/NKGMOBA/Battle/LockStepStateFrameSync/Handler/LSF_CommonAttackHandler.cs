namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_CommonAttackCmd.CmdType)]
    public class LSF_CommonAttackHandler : ALockStepStateFrameSyncMessageHandler<LSF_CommonAttackCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_CommonAttackCmd cmd)
        {
            unit.GetComponent<CommonAttackComponent_Logic>()
                .SetAttackTarget(unit.BelongToRoom.GetComponent<UnitComponent>().Get(cmd.TargetUnitId));

#if SERVER
            // 对于客户端发来的每一条指令，都要进行一次广播，因为多人模式需要进行同步，
            LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();
            lsfComponent.AddCmdToSendQueue(cmd);
#endif
            
            await ETTask.CompletedTask;
        }
    }
}