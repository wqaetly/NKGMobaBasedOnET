using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class Server_B2SColliderConfigCategory : ProtoObject
    {
        public static Server_B2SColliderConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, Server_B2SColliderConfig> dict = new Dictionary<int, Server_B2SColliderConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Server_B2SColliderConfig> list = new List<Server_B2SColliderConfig>();
		
        public Server_B2SColliderConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (Server_B2SColliderConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public Server_B2SColliderConfig Get(int id)
        {
            this.dict.TryGetValue(id, out Server_B2SColliderConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (Server_B2SColliderConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Server_B2SColliderConfig> GetAll()
        {
            return this.dict;
        }

        public Server_B2SColliderConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class Server_B2SColliderConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public long B2S_ColliderId { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public bool SyncToUnit { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
