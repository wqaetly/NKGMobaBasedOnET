using UnityEngine;

namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_MoveCmd.CmdType)]
    public class LSF_MoveCmdHandler : ALockStepStateFrameSyncMessageHandler<LSF_MoveCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_MoveCmd cmd)
        {
#if !SERVER
            Vector3 pos = new Vector3(cmd.PosX, cmd.PosY, cmd.PosZ);
            Quaternion rotation = new Quaternion(cmd.RotA, cmd.RotB, cmd.RotC, cmd.RotW);
            unit.Position = pos;
            unit.Rotation = rotation;
#endif
            
            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            if (cmd.IsMoveStartCmd)
            {
                Vector3 target = new Vector3(cmd.TargetPosX, cmd.TargetPosY, cmd.TargetPosZ);
                IdleState idleState = ReferencePool.Acquire<IdleState>();
                idleState.SetData(StateTypes.Idle, "Idle", 1);
                unit.NavigateTodoSomething(target, 0, idleState).Coroutine();
                moveComponent.StartMoveCurrentFrame = true;
            }
            
            if (cmd.IsStopped)
            {
                moveComponent.Stop(true);
                Game.EventSystem.Publish(new EventType.MoveStop() {Unit = unit}).Coroutine();
            }

            await ETTask.CompletedTask;
        }
    }
}