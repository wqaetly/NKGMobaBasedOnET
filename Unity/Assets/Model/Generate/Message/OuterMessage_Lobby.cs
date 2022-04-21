using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(typeof(L2C_LoginLobby))]
	[Message(OuterOpcode_Lobby.C2L_LoginLobby)]
	[ProtoContract]
	public partial class C2L_LoginLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_LoginLobby)]
	[ProtoContract]
	public partial class L2C_LoginLobby: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(5)]
		public List<long> RoomHolderPlayerList = new List<long>();

		[ProtoMember(1)]
		public List<long> RoomIdList = new List<long>();

		[ProtoMember(2)]
		public List<string> RoomNameList = new List<string>();

		[ProtoMember(3)]
		public List<int> RoomPlayerNum = new List<int>();

	}

///////////////////////////////// 房间相关  START ///////////////////////////////////
	[ResponseType(typeof(L2C_CreateNewRoomLobby))]
	[Message(OuterOpcode_Lobby.C2L_CreateNewRoomLobby)]
	[ProtoContract]
	public partial class C2L_CreateNewRoomLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_CreateNewRoomLobby)]
	[ProtoContract]
	public partial class L2C_CreateNewRoomLobby: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int RoomId { get; set; }

		[ProtoMember(2)]
		public int mode { get; set; }

		[ProtoMember(3)]
		public int camp { get; set; }

	}

	[ResponseType(typeof(L2C_JoinRoomLobby))]
	[Message(OuterOpcode_Lobby.C2L_JoinRoomLobby)]
	[ProtoContract]
	public partial class C2L_JoinRoomLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public long RoomId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_JoinRoomLobby)]
	[ProtoContract]
	public partial class L2C_JoinRoomLobby: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long RoomId { get; set; }

		[ProtoMember(2)]
		public int camp { get; set; }

		[ProtoMember(3)]
		public long RoomHolderId { get; set; }

		[ProtoMember(4)]
		public string RoomName { get; set; }

		[ProtoMember(5)]
		public List<PlayerInfoRoom> playerInfoRoom = new List<PlayerInfoRoom>();

	}

	[Message(OuterOpcode_Lobby.PlayerInfoRoom)]
	[ProtoContract]
	public partial class PlayerInfoRoom: Object
	{
		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public long SessionId { get; set; }

		[ProtoMember(4)]
		public long RoomId { get; set; }

		[ProtoMember(5)]
		public int camp { get; set; }

		[ProtoMember(6)]
		public long playerid { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_PlayerTriggerRoom)]
	[ProtoContract]
	public partial class L2C_PlayerTriggerRoom: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PlayerInfoRoom playerInfoRoom { get; set; }

		[ProtoMember(2)]
		public bool JoinOrLeave { get; set; }

	}

	[ResponseType(typeof(L2C_LeaveRoomLobby))]
	[Message(OuterOpcode_Lobby.C2L_LeaveRoomLobby)]
	[ProtoContract]
	public partial class C2L_LeaveRoomLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_LeaveRoomLobby)]
	[ProtoContract]
	public partial class L2C_LeaveRoomLobby: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int camp { get; set; }

		[ProtoMember(2)]
		public long newRoomHolder { get; set; }

		[ProtoMember(4)]
		public long RoomId { get; set; }

		[ProtoMember(5)]
		public long PlayerId { get; set; }

		[ProtoMember(3)]
		public bool isDestory { get; set; }

	}

	[Message(OuterOpcode_Lobby.PlayerBattleInfo)]
	[ProtoContract]
	public partial class PlayerBattleInfo: Object
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public int HeroConfig { get; set; }

	}

	[ResponseType(typeof(L2C_StartGameLobby))]
	[Message(OuterOpcode_Lobby.C2L_StartGameLobby)]
	[ProtoContract]
	public partial class C2L_StartGameLobby: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public List<PlayerBattleInfo> PlayerBattleInfos = new List<PlayerBattleInfo>();

	}

	[Message(OuterOpcode_Lobby.L2C_StartGameLobby)]
	[ProtoContract]
	public partial class L2C_StartGameLobby: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<long> list = new List<long>();

	}

	[Message(OuterOpcode_Lobby.PlayerBattlePoint)]
	[ProtoContract]
	public partial class PlayerBattlePoint: Object
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public int Point { get; set; }

		[ProtoMember(3)]
		public int SingleMatchCount { get; set; }

		[ProtoMember(4)]
		public string Account { get; set; }

		[ProtoMember(5)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_PrepareToEnterBattle)]
	[ProtoContract]
	public partial class L2C_PrepareToEnterBattle: Object, IMessage
	{
	}

	[Message(OuterOpcode_Lobby.C2L_PreparedToEnterBattle)]
	[ProtoContract]
	public partial class C2L_PreparedToEnterBattle: Object, IMessage
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode_Lobby.L2C_AllowToEnterMap)]
	[ProtoContract]
	public partial class L2C_AllowToEnterMap: Object, IMessage
	{
	}

}
