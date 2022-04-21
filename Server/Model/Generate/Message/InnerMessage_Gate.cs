using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(typeof(G2G_LockResponse))]
	[Message(InnerOpcode_Gate.G2G_LockRequest)]
	[ProtoContract]
	public partial class G2G_LockRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public string Address { get; set; }

	}

	[Message(InnerOpcode_Gate.G2G_LockResponse)]
	[ProtoContract]
	public partial class G2G_LockResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(typeof(G2G_LockReleaseResponse))]
	[Message(InnerOpcode_Gate.G2G_LockReleaseRequest)]
	[ProtoContract]
	public partial class G2G_LockReleaseRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public string Address { get; set; }

	}

	[Message(InnerOpcode_Gate.G2G_LockReleaseResponse)]
	[ProtoContract]
	public partial class G2G_LockReleaseResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(InnerOpcode_Gate.G2R_GetLoginKey)]
	[ProtoContract]
	public partial class G2R_GetLoginKey: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

	}

	[ResponseType(typeof(L2G_GetRoomId))]
	[Message(InnerOpcode_Gate.G2L_GetRoomId)]
	[ProtoContract]
	public partial class G2L_GetRoomId: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(InnerOpcode_Gate.G2M_SessionDisconnect)]
	[ProtoContract]
	public partial class G2M_SessionDisconnect: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(94)]
		public long ActorId { get; set; }

	}

}
