using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class Server_UnitConfigCategory : ProtoObject
    {
        public static Server_UnitConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, Server_UnitConfig> dict = new Dictionary<int, Server_UnitConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Server_UnitConfig> list = new List<Server_UnitConfig>();
		
        public Server_UnitConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (Server_UnitConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public Server_UnitConfig Get(int id)
        {
            this.dict.TryGetValue(id, out Server_UnitConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (Server_UnitConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Server_UnitConfig> GetAll()
        {
            return this.dict;
        }

        public Server_UnitConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class Server_UnitConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public int UnitAttributesDataId { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public int UnitColliderDataConfigId { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public int UnitPassiveSkillId { get; set; }
		[ProtoMember(5, IsRequired  = true)]
		public int UnitQSkillId { get; set; }
		[ProtoMember(6, IsRequired  = true)]
		public int UnitWSkillId { get; set; }
		[ProtoMember(7, IsRequired  = true)]
		public int UnitESkillId { get; set; }
		[ProtoMember(8, IsRequired  = true)]
		public int UnitRSkillId { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
