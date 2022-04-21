using System.Diagnostics;
using ET.EventType;
using UnityEngine;

namespace ET
{
    public class MoveStop_LogicHandler : AEvent<EventType.MoveStop>
    {
        protected override async ETTask Run(MoveStop a)
        {
            a.Unit.GetComponent<StackFsmComponent>().RemoveState(StateTypes.Run);
            await ETTask.CompletedTask;
        }
    }

    [LSF_Tickable(EntityType = typeof(MoveComponent))]
    public class MoveComponentTicker : ALSF_TickHandler<MoveComponent>
    {
        public override bool OnLSF_CheckConsistency(MoveComponent entity, uint frame, ALSF_Cmd stateToCompare)
        {
            LSF_MoveCmd serverMoveState = stateToCompare as LSF_MoveCmd;

            if (serverMoveState == null)
            {
                return true;
            }
            
            if (entity.HistroyMoveStates.TryGetValue(frame, out var histroyState))
            {
                bool result = serverMoveState.CheckConsistency(histroyState);
                stateToCompare.PassingConsistencyCheck = result;
                if (!result)
                {
                    // Log.Error(
                    //      $"---来自MoveComponent的不一致：服务端 {MongoHelper.ToJson(serverMoveState)}\n客户端：{MongoHelper.ToJson(histroyState)}");
                }
                else
                {
                    // Log.Error(
                    //     $"√√√来自MoveComponent的一致：服务端 {serverMoveState.Frame} X：{serverMoveState.PosX} Y: {serverMoveState.PosY} Z: {serverMoveState.PosZ}\n客户端：{frame} X：{histroyState.PosX} Y: {histroyState.PosY} Z: {histroyState.PosZ}");
                }

                return result;
            }
            
            return false;
        }

        public override void OnLSF_TickEnd(MoveComponent entity, uint frame, long deltaTime)
        {
            Unit unit = entity.GetParent<Unit>();

            LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();

            LSF_MoveCmd lsfMoveCmd = ReferencePool.Acquire<LSF_MoveCmd>().Init(unit.Id) as LSF_MoveCmd;

            lsfMoveCmd.Speed = entity.Speed;
            lsfMoveCmd.PosX = unit.Position.x;
            lsfMoveCmd.PosY = unit.Position.y;
            lsfMoveCmd.PosZ = unit.Position.z;

            lsfMoveCmd.RotA = unit.Rotation.x;
            lsfMoveCmd.RotB = unit.Rotation.y;
            lsfMoveCmd.RotC = unit.Rotation.z;
            lsfMoveCmd.RotW = unit.Rotation.w;

            lsfMoveCmd.IsMoveStartCmd = false;
            lsfMoveCmd.IsStopped = !entity.ShouldMove;
            lsfMoveCmd.Frame = lsfComponent.CurrentFrame;

            entity.HistroyMoveStates[lsfComponent.CurrentFrame] = lsfMoveCmd;

            if (entity.StartMoveCurrentFrame)
            {
                entity.StartMoveCurrentFrame = false;
                lsfMoveCmd.IsMoveStartCmd = true;
                lsfMoveCmd.TargetPosX = entity.FinalTarget.x;
                lsfMoveCmd.TargetPosY = entity.FinalTarget.y;
                lsfMoveCmd.TargetPosZ = entity.FinalTarget.z;
            }

#if SERVER
            // 只有数据脏了才进行发送
            if (!this.OnLSF_CheckConsistency(entity, lsfComponent.CurrentFrame - 1, lsfMoveCmd))
            {
                lsfComponent.AddCmdToSendQueue(lsfMoveCmd);
            }
            else
            {
                lsfComponent.AddCmdsToWholeCmdsBuffer(ref lsfMoveCmd);
            }
#endif
        }

#if !SERVER
        public override void OnLSF_RollBackTick(MoveComponent entity, uint frame, ALSF_Cmd stateToCompare)
        {
            LSF_MoveCmd lsfMoveCmd = stateToCompare as LSF_MoveCmd;

            if (lsfMoveCmd == null)
            {
                return;
            }

            Unit unit = entity.GetParent<Unit>();

            Vector3 previousTarget = entity.FinalTarget;
            
            entity.Stop();
            Game.EventSystem.Publish(new EventType.MoveStop() {Unit = unit}).Coroutine();

            Vector3 pos = new Vector3(lsfMoveCmd.PosX, lsfMoveCmd.PosY, lsfMoveCmd.PosZ);
            Quaternion rotation = new Quaternion(lsfMoveCmd.RotA, lsfMoveCmd.RotB, lsfMoveCmd.RotC, lsfMoveCmd.RotW);
            unit.Position = pos;
            unit.Rotation = rotation;
            
            if (previousTarget != Vector3.zero)
            {
                IdleState idleState = ReferencePool.Acquire<IdleState>();
                idleState.SetData(StateTypes.Idle, "Idle", 1);
                unit.NavigateTodoSomething(previousTarget, 0,
                    idleState).Coroutine();
            }
        }

        public override void OnLSF_ViewTick(MoveComponent entity, long deltaTime)
        {
            Unit unit = entity.GetParent<Unit>();
            unit.ViewPosition = Vector3.Lerp(unit.ViewPosition, unit.Position, 0.5f);
            unit.ViewRotation = Quaternion.Slerp(unit.ViewRotation, unit.Rotation, 0.5f);
        }

#endif

        public override void OnLSF_Tick(MoveComponent entity, uint currentFrame, long deltaTime)
        {
            if (entity.ShouldMove)
            {
                entity.MoveForward(deltaTime, false);
#if !SERVER
                //Log.Info($"寻路完成后：{unit.Position.ToString("#0.0000")}");  
#endif
            }

// #if !SERVER
//             if (entity.GetParent<Unit>().BelongToRoom.GetComponent<UnitComponent>().MyUnit != unit)
//             {
//                 Log.Error($"----------------{entity.GetParent<Unit>().Position.ToString("#0.0000")}");
//             }
// #endif
        }
    }
}