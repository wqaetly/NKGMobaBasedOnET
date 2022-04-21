using ProtoBuf;
using UnityEngine;

namespace ET
{
    [ProtoContract]
    public class LSF_MoveCmd : ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.Move;

        /// <summary>
        /// 这个指令是否为寻路开始的指令
        /// </summary>
        [ProtoMember(1)] public bool IsMoveStartCmd;
        
        [ProtoMember(2)] public float PosX;
        [ProtoMember(3)] public float PosY;
        [ProtoMember(4)] public float PosZ;

        [ProtoMember(5)] public float RotA;
        [ProtoMember(6)] public float RotB;
        [ProtoMember(7)] public float RotC;
        [ProtoMember(8)] public float RotW;

        [ProtoMember(9)] public float Speed;
        [ProtoMember(10)] public bool IsStopped;
        
        [ProtoMember(11)] public float TargetPosX;
        [ProtoMember(12)] public float TargetPosY;
        [ProtoMember(13)] public float TargetPosZ;

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;

            return this;
        }

        public override bool CheckConsistency(ALSF_Cmd alsfCmd)
        {
            LSF_MoveCmd lsfMoveCmd = alsfCmd as LSF_MoveCmd;
            
            // 当为开始寻路指令时，直接忽略目标点对比
            if (lsfMoveCmd.IsMoveStartCmd == this.IsMoveStartCmd)
            {
                if (Mathf.Abs(lsfMoveCmd.TargetPosX - this.TargetPosX) > 0.001f)
                {
                    return false;
                }
            
                if (Mathf.Abs(lsfMoveCmd.TargetPosZ - this.TargetPosZ) > 0.001f)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (Mathf.Abs(lsfMoveCmd.PosX - this.PosX) > 0.001f)
            {
                return false;
            }
            
            if (Mathf.Abs(lsfMoveCmd.PosZ - this.PosZ) > 0.001f)
            {
                return false;
            }
            
            if (Mathf.Abs(lsfMoveCmd.Speed - this.Speed) > 0.001f)
            {
                return false;
            }

            if (lsfMoveCmd.IsStopped != this.IsStopped)
            {
                return false;
            }

            return true;
        }

        public override void Clear()
        {
            base.Clear();
            PosX = 0;
            PosY = 0;
            PosZ = 0;
            RotA = 0;
            RotB = 0;
            RotC = 0;
            RotW = 0;
            Speed = 0;
            IsStopped = false;
            IsMoveStartCmd = false;
            Frame = 0;
        }
    }
}