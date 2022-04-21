using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_CommonAttackCmd: ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.CommonAttack;

        [ProtoMember(1)] public long TargetUnitId;

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;

            return this;
        }

        public override void Clear()
        {
            base.Clear();
            TargetUnitId = 0;
        }
    }
}