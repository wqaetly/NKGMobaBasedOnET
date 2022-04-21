using System.Collections.Generic;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_ChangeBBValueCmd: ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.ChangeBlackBoardValue;

        /// <summary>
        /// 目标行为树Id
        /// </summary>
        [ProtoMember(1)] public long TargetNPBehaveTreeId;

        /// <summary>
        /// 将要同步修改的黑板键值
        /// </summary>
        [ProtoMember(2)]
        public NP_RuntimeTreeBBSnap NP_RuntimeTreeBBSnap;

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;
            this.TargetNPBehaveTreeId = 0;

            return this;
        }

        public override void Clear()
        {
            NP_RuntimeTreeBBSnap.NP_FrameBBValues.Clear();
            NP_RuntimeTreeBBSnap.NP_FrameBBValueOperations.Clear();
        }
    }
}