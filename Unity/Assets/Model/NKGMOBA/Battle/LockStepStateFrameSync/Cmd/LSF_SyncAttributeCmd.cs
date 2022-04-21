using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class LSF_SyncAttributeCmd: ALSF_Cmd
    {
        public const uint CmdType = LSF_CmdType.SyncAttribute;

        /// <summary>
        /// 同步最终结果
        /// </summary>
        [ProtoMember(1)]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> SyncAttributesResult = new Dictionary<int, float>();
        
        /// <summary>
        /// 同步变化量
        /// </summary>
        [ProtoMember(2)]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> SyncAttributesChanged = new Dictionary<int, float>();

        public override ALSF_Cmd Init(long unitId)
        {
            this.LockStepStateFrameSyncDataType = CmdType;
            this.UnitId = unitId;

            return this;
        }
    }
}