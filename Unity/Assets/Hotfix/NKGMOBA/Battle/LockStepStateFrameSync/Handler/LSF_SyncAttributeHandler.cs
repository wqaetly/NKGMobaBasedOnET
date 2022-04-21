namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_SyncAttributeCmd.CmdType)]
    public class LSF_SyncAttributeHandler : ALockStepStateFrameSyncMessageHandler<LSF_SyncAttributeCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_SyncAttributeCmd cmd)
        {
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            foreach (var attributeToSync in cmd.SyncAttributesResult)
            {
                numericComponent[(NumericType) attributeToSync.Key] = attributeToSync.Value;
            }

            foreach (var changedAttributeInfo in cmd.SyncAttributesChanged)
            {
                Game.EventSystem.Publish(new EventType.NumericApplyChangeValue()
                {
                    ChangedValue = changedAttributeInfo.Value, NumericType = (NumericType) changedAttributeInfo.Key,
                    Unit = unit
                }).Coroutine();
            }

#if SERVER
            // 对于客户端发来的每一条指令，都要进行一次广播，因为多人模式需要进行同步，
            LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();
            lsfComponent.AddCmdToSendQueue(cmd);
#endif

            await ETTask.CompletedTask;
        }
    }
}