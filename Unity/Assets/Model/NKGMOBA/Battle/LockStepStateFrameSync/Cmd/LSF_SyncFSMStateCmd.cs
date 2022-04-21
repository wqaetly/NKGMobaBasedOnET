using System.Collections.Generic;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_SyncFSMStateCmd: ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.SyncFSMState;

        [ProtoMember(1)]
        public Dictionary<string, AFsmStateBase> ChangedStates = new Dictionary<string, AFsmStateBase>();

        public override ALSF_Cmd Init(long unitId)
        {
            throw new System.NotImplementedException();
        }
    }
}