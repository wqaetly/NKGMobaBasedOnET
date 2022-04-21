using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class UnitAttributesDataConfigCategory : ProtoObject
    {
        public static UnitAttributesDataConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, UnitAttributesDataConfig> dict = new Dictionary<int, UnitAttributesDataConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<UnitAttributesDataConfig> list = new List<UnitAttributesDataConfig>();
		
        public UnitAttributesDataConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (UnitAttributesDataConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public UnitAttributesDataConfig Get(int id)
        {
            this.dict.TryGetValue(id, out UnitAttributesDataConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (UnitAttributesDataConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, UnitAttributesDataConfig> GetAll()
        {
            return this.dict;
        }

        public UnitAttributesDataConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class UnitAttributesDataConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public long UnitAttributesDataSupportorId { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
