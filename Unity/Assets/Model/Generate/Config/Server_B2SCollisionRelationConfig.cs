using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class Server_B2SCollisionRelationConfigCategory : ProtoObject
    {
        public static Server_B2SCollisionRelationConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, Server_B2SCollisionRelationConfig> dict = new Dictionary<int, Server_B2SCollisionRelationConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Server_B2SCollisionRelationConfig> list = new List<Server_B2SCollisionRelationConfig>();
		
        public Server_B2SCollisionRelationConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (Server_B2SCollisionRelationConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public Server_B2SCollisionRelationConfig Get(int id)
        {
            this.dict.TryGetValue(id, out Server_B2SCollisionRelationConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (Server_B2SCollisionRelationConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Server_B2SCollisionRelationConfig> GetAll()
        {
            return this.dict;
        }

        public Server_B2SCollisionRelationConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class Server_B2SCollisionRelationConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public int B2S_ColliderConfigId { get; set; }
		[ProtoMember(3, IsRequired  = true)]
		public string B2S_ColliderHandlerName { get; set; }
		[ProtoMember(4, IsRequired  = true)]
		public bool FriendlyHero { get; set; }
		[ProtoMember(5, IsRequired  = true)]
		public bool EnemyHero { get; set; }
		[ProtoMember(6, IsRequired  = true)]
		public bool FriendlySoldier { get; set; }
		[ProtoMember(7, IsRequired  = true)]
		public bool EnemySoldier { get; set; }
		[ProtoMember(8, IsRequired  = true)]
		public bool FriendlyBuilds { get; set; }
		[ProtoMember(9, IsRequired  = true)]
		public bool EnemyBuilds { get; set; }
		[ProtoMember(10, IsRequired  = true)]
		public bool Creeps { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
