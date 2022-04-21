using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class RecastNavDataConfigCategory : ProtoObject
    {
        public static RecastNavDataConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, RecastNavDataConfig> dict = new Dictionary<int, RecastNavDataConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<RecastNavDataConfig> list = new List<RecastNavDataConfig>();
		
        public RecastNavDataConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (RecastNavDataConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public RecastNavDataConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RecastNavDataConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (RecastNavDataConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RecastNavDataConfig> GetAll()
        {
            return this.dict;
        }

        public RecastNavDataConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class RecastNavDataConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public string ConfigName { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
