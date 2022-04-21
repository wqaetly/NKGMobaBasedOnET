using System;
using Microsoft.CodeAnalysis;
using ProtoBuf;
using UnityEngine;

namespace ET
{
    [ProtoContract]
    public class LSF_PlaySkillInputCmd : ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.PlayerSkillInput;

        /// <summary>
        /// 输入的Tag标识，比如PlayerInput，E长按
        /// </summary>
        [ProtoMember(1)] public string InputTag;

        /// <summary>
        /// 输入的具体指令，比如E，EHold
        /// </summary>
        [ProtoMember(2)] public string InputKey;

        /// <summary>
        /// 角度
        /// </summary>
        [ProtoMember(3)] public float Angle;

        /// <summary>
        /// 目标位置
        /// </summary>
        [ProtoMember(4)] public float TargetPosX;
        
        /// <summary>
        /// 目标位置
        /// </summary>
        [ProtoMember(5)] public float TargetPosY;
        
        /// <summary>
        /// 目标位置
        /// </summary>
        [ProtoMember(6)] public float TargetPosZ;
        
        /// <summary>
        /// 目标Unit
        /// </summary>
        [ProtoMember(7)] public long TargetUnitId;

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;
            return this;
        }

        public override void Clear()
        {
            InputKey = String.Empty;
        }
    }
}