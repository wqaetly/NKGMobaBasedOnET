using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_CreateSpilingCmd: ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.CreateSpiling;
        
        [ProtoMember(1)]
        public UnitInfo UnitInfo;

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;

            return this;
        }

        public override void Clear()
        {
            base.Clear();
            UnitInfo = null;
        }
    }
}