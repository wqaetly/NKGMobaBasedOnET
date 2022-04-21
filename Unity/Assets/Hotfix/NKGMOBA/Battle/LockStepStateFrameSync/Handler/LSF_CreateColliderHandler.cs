using UnityEngine;

namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_CreateColliderCmd.CmdType)]
    public class LSF_CreateColliderHandler : ALockStepStateFrameSyncMessageHandler<LSF_CreateColliderCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_CreateColliderCmd cmd)
        {
#if !SERVER
            UnitFactory.CreateSpecialColliderUnit(unit.BelongToRoom, cmd.BelongtoUnitId, cmd.SelfId,
                cmd.CollisionsRelationSupportIdInExcel, cmd.ColliderNPBehaveTreeIdInExcel, cmd.FollowUnitPos,
                cmd.FollowUnitRot, new Vector3(cmd.OffsetX, 0, cmd.OffsetZ),
                new Vector3(cmd.TargetPosX, 0, cmd.TargetPosZ), cmd.Angle);
            Log.Info($"------------创建了Id为：{cmd.SelfId}的碰撞体");
#endif

            await ETTask.CompletedTask;
        }
    }
}