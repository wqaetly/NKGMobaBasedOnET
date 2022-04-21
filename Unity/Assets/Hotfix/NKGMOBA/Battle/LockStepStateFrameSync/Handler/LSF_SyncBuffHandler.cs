namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_SyncBuffCmd.CmdType)]
    public class LSF_SyncBuffHandler : ALockStepStateFrameSyncMessageHandler<LSF_SyncBuffCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_SyncBuffCmd cmd)
        {
            foreach (var snapInfo in cmd.BuffSnapInfoCollection.FrameBuffChangeSnap)
            {
                switch (snapInfo.Value.OperationType)
                {
                    case BuffSnapInfo.BuffOperationType.ADD:
                        // 有可能同步的Buff信息来自远程玩家，即本地可能已经对其进行了Buff添加操作
                        IBuffSystem tobeAddedBuffSystem =
                            unit.GetComponent<BuffManagerComponent>().GetBuffById(snapInfo.Value.BuffId);
                        if (tobeAddedBuffSystem == null)
                        {
                            tobeAddedBuffSystem = BuffFactory.AcquireBuff(unit.BelongToRoom,
                                snapInfo.Value.NP_SupportId, snapInfo.Value.BuffNodeId,
                                snapInfo.Value.FromUnitId, snapInfo.Value.BelongtoUnitId,
                                snapInfo.Value.BelongtoNP_RuntimeTreeId);
                        }

                        tobeAddedBuffSystem.CurrentOverlay = snapInfo.Value.BuffLayer;
                        if (tobeAddedBuffSystem.CurrentOverlay != 1)
                        {
                            tobeAddedBuffSystem.Refresh(cmd.Frame);
                        }

                        break;
                    case BuffSnapInfo.BuffOperationType.CHANGE:
                        IBuffSystem changedBuffSystem =
                            unit.GetComponent<BuffManagerComponent>().GetBuffById(snapInfo.Value.BuffId);
                        if (changedBuffSystem.CurrentOverlay != snapInfo.Value.BuffLayer || changedBuffSystem.MaxLimitFrame != snapInfo.Value.BuffMaxLimitFrame)
                        {
                            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(changedBuffSystem, cmd.Frame,
                                snapInfo.Value.BuffLayer);
                        }
                        break;
                    case BuffSnapInfo.BuffOperationType.REMOVE:
                        unit.BelongToRoom.GetComponent<UnitComponent>().Get(snapInfo.Value.BelongtoUnitId)
                            .GetComponent<BuffManagerComponent>().RemoveBuff(snapInfo.Value.BuffNodeId);
                        break;
                    default:
                        Log.Error("BuffOperationType 为 空，请检查逻辑");
                        break;
                }
            }

            await ETTask.CompletedTask;
        }
    }
}