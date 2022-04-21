using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class Client_UnitConfigCategory : ProtoObject
    {
        public static Client_UnitConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, Client_UnitConfig> dict = new Dictionary<int, Client_UnitConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Client_UnitConfig> list = new List<Client_UnitConfig>();
		
        public Client_UnitConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (Client_UnitConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public Client_UnitConfig Get(int id)
        {
            this.dict.TryGetValue(id, out Client_UnitConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (Client_UnitConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Client_UnitConfig> GetAll()
        {
            return this.dict;
        }

        public Client_UnitConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class Client_UnitConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public string UnitName { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public int UnitAttributesDataId { get; set; }
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
