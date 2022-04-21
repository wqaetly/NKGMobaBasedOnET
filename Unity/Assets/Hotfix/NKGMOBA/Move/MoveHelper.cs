using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class MoveHelper
    {
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static async ETTask<bool> FindPathMoveToAsync(this Unit unit, Vector3 target, float targetRange = 0,
            ETCancellationToken cancellationToken = null)
        {
            float speed = unit.GetComponent<NumericComponent>()[NumericType.Speed] / 100f;
            if (speed < 0.01)
            {
                return true;
            }

            using var list = ListComponent<Vector3>.Create();

            unit.GetComponent<RecastNavComponent>().Find(unit.Position, target, list.List);

            List<Vector3> path = list.List;
            if (path.Count < 2)
            {
                return true;
            }

            bool ret = await unit.GetComponent<MoveComponent>()
                .MoveToAsync(path, speed, 100, targetRange, cancellationToken);

            return ret;
        }

        public static void Stop(this Unit unit)
        {
            unit.GetComponent<MoveComponent>().Stop();
        }
        
        public static void SendPathFindCmd(this Unit unit, Vector3 targetPoint)
        {
            LSF_MoveCmd pathFindCmd =
                ReferencePool.Acquire<LSF_MoveCmd>().Init(unit.Id) as LSF_MoveCmd;

            pathFindCmd.IsMoveStartCmd = true;

            pathFindCmd.PosX = unit.Position.x;
            pathFindCmd.PosY = unit.Position.y;
            pathFindCmd.PosZ = unit.Position.z;

            pathFindCmd.RotA = unit.Rotation.x;
            pathFindCmd.RotB = unit.Rotation.y;
            pathFindCmd.RotC = unit.Rotation.z;
            pathFindCmd.RotW = unit.Rotation.w;
            
            pathFindCmd.TargetPosX = targetPoint.x;
            pathFindCmd.TargetPosY = targetPoint.y;
            pathFindCmd.TargetPosZ = targetPoint.z;
            
            pathFindCmd.Speed = unit.GetComponent<MoveComponent>().Speed;

            unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(pathFindCmd);
        }
    }
}