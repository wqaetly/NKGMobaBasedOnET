using System.Collections.Generic;
using NPBehave;

namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_ChangeBBValueCmd.CmdType)]
    public class LSF_ChangeBBValueHandler : ALockStepStateFrameSyncMessageHandler<LSF_ChangeBBValueCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_ChangeBBValueCmd cmd)
        {
            NP_RuntimeTreeManager npRuntimeTreeManager = unit.GetComponent<NP_RuntimeTreeManager>();
            NP_RuntimeTreeBBSnap cmdNPRuntimeTreeBbSnap = cmd.NP_RuntimeTreeBBSnap;

#if !SERVER
            bool isLocalPlayer = unit == unit.BelongToRoom.GetComponent<UnitComponent>().MyUnit;
#else
            bool isLocalPlayer = true;
#endif

            // 服务器发来的脏数据就是变更的黑板数据，直接原样设置到黑板中即可
            if (npRuntimeTreeManager.RuntimeTrees.TryGetValue(cmd.TargetNPBehaveTreeId, out var npRuntimeTree))
            {
                Blackboard blackboard = npRuntimeTree.GetBlackboard();

                foreach (var toBeChangedBBValues in cmd.NP_RuntimeTreeBBSnap.NP_FrameBBValues)
                {
                    if (cmdNPRuntimeTreeBbSnap.NP_FrameBBValueOperations.TryGetValue(toBeChangedBBValues.Key,
                        out var operationType))
                    {
                        switch (operationType)
                        {
                            case NP_RuntimeTreeBBOperationType.ADD:
                            case NP_RuntimeTreeBBOperationType.CHANGE:
                                NP_BBValueHelper.SetTargetBlackboardUseANP_BBValue(toBeChangedBBValues.Value,
                                    blackboard,
                                    toBeChangedBBValues.Key, isLocalPlayer);
                                break;
                            case NP_RuntimeTreeBBOperationType.REMOVE:
                                blackboard.Unset(toBeChangedBBValues.Key);
                                break;
                        }
                    }
                }
            }

            await ETTask.CompletedTask;
        }
    }
}