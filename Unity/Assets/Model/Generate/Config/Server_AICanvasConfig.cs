using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class Server_AICanvasConfigCategory : ProtoObject
    {
        public static Server_AICanvasConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, Server_AICanvasConfig> dict = new Dictionary<int, Server_AICanvasConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Server_AICanvasConfig> list = new List<Server_AICanvasConfig>();
		
        public Server_AICanvasConfigCategory()
        {
            Instance = this;
        }
		
		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            foreach (Server_AICanvasConfig config in list)
            {
                this.dict.Add(config.Id, config);
            }
            list.Clear();
            this.EndInit();
        }
		
        public Server_AICanvasConfig Get(int id)
        {
            this.dict.TryGetValue(id, out Server_AICanvasConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (Server_AICanvasConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Server_AICanvasConfig> GetAll()
        {
            return this.dict;
        }

        public Server_AICanvasConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class Server_AICanvasConfig: ProtoObject, IConfig
	{
		[ProtoMember(1, IsRequired  = true)]
		public int Id { get; set; }
		[ProtoMember(2, IsRequired  = true)]
		public long NPBehaveId { get; set; }


		[ProtoAfterDeserialization]
        public void AfterDeserialization()
        {
            this.EndInit();
        }
	}
}
