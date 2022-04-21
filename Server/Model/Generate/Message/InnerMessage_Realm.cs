using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(typeof(G2R_GetLoginKey))]
	[Message(InnerOpcode_Realm.R2G_GetLoginKey)]
	[ProtoContract]
	public partial class R2G_GetLoginKey: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

	}

}
