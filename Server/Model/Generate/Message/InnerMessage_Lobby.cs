using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[Message(InnerOpcode_Lobby.L2G_GetRoomId)]
	[ProtoContract]
	public partial class L2G_GetRoomId: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long RoomId { get; set; }

	}

	[ResponseType(typeof(M2L_CreateHeroUnit))]
	[Message(InnerOpcode_Lobby.L2M_CreateHeroUnit)]
	[ProtoContract]
	public partial class L2M_CreateHeroUnit: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public long GateSessionId { get; set; }

		[ProtoMember(3)]
		public long Roomid { get; set; }

		[ProtoMember(4)]
		public PlayerBattleInfo PlayerBattleInfo { get; set; }

	}

	[ResponseType(typeof(M2L_PreparedToEnterBattle))]
	[Message(InnerOpcode_Lobby.L2M_PreparedToEnterBattle)]
	[ProtoContract]
	public partial class L2M_PreparedToEnterBattle: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long Roomid { get; set; }

	}

}
