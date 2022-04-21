using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(typeof(M2C_TestResponse))]
	[Message(OuterOpcode_Map.C2M_TestRequest)]
	[ProtoContract]
	public partial class C2M_TestRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string request { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_TestResponse)]
	[ProtoContract]
	public partial class M2C_TestResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string response { get; set; }

	}

	[ResponseType(typeof(Actor_TransferResponse))]
	[Message(OuterOpcode_Map.Actor_TransferRequest)]
	[ProtoContract]
	public partial class Actor_TransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode_Map.Actor_TransferResponse)]
	[ProtoContract]
	public partial class Actor_TransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(typeof(M2C_Ping))]
	[Message(OuterOpcode_Map.C2M_Ping)]
	[ProtoContract]
	public partial class C2M_Ping: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_Ping)]
	[ProtoContract]
	public partial class M2C_Ping: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public long TimePoint { get; set; }

		[ProtoMember(93)]
		public uint ServerFrame { get; set; }

	}

	[ResponseType(typeof(M2C_Reload))]
	[Message(OuterOpcode_Map.C2M_Reload)]
	[ProtoContract]
	public partial class C2M_Reload: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_Reload)]
	[ProtoContract]
	public partial class M2C_Reload: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(typeof(M2C_TestRobotCase))]
	[Message(OuterOpcode_Map.C2M_TestRobotCase)]
	[ProtoContract]
	public partial class C2M_TestRobotCase: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_TestRobotCase)]
	[ProtoContract]
	public partial class M2C_TestRobotCase: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_EndBattleSettle)]
	[ProtoContract]
	public partial class M2C_EndBattleSettle: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<PlayerBattlePoint> settleAccount = new List<PlayerBattlePoint>();

	}

	[Message(OuterOpcode_Map.M2C_KillEvent)]
	[ProtoContract]
	public partial class M2C_KillEvent: Object, IActorMessage
	{
		[ProtoMember(1)]
		public PlayerBattlePoint killer { get; set; }

		[ProtoMember(2)]
		public PlayerBattlePoint deadPlayer { get; set; }

	}

	[Message(OuterOpcode_Map.UnitInfo)]
	[ProtoContract]
	public partial class UnitInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

// 所属的玩家id
// 所属的玩家id
		[ProtoMember(99)]
		public long BelongToPlayerId { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public int RoleCamp { get; set; }

		[ProtoMember(7)]
		public long RoomId { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_CreateUnits)]
	[ProtoContract]
	public partial class M2C_CreateUnits: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long PlayerId { get; set; }

		[ProtoMember(95)]
		public long RoomId { get; set; }

		[ProtoMember(2)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(OuterOpcode_Map.M2C_UnitDestoryed)]
	[ProtoContract]
	public partial class M2C_UnitDestoryed: Object, IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

//被破坏的UnitId
//被破坏的UnitId
		[ProtoMember(94)]
		public long DestoryedUnitId { get; set; }

	}

	[Message(OuterOpcode_Map.C2M_Stop)]
	[ProtoContract]
	public partial class C2M_Stop: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_Stop)]
	[ProtoContract]
	public partial class M2C_Stop: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public float A { get; set; }

		[ProtoMember(7)]
		public float B { get; set; }

		[ProtoMember(8)]
		public float C { get; set; }

		[ProtoMember(9)]
		public float W { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_ReceiveDamage)]
	[ProtoContract]
	public partial class M2C_ReceiveDamage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(4)]
		public float FinalValue { get; set; }

	}

	[Message(OuterOpcode_Map.C2M_CastHeroSkill)]
	[ProtoContract]
	public partial class C2M_CastHeroSkill: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_CastHeroSkill)]
	[ProtoContract]
	public partial class M2C_CastHeroSkill: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_RecoverHP)]
	[ProtoContract]
	public partial class M2C_RecoverHP: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long SpriteUnitId { get; set; }

		[ProtoMember(2)]
		public float RecoverHPValue { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_BuffInfo)]
	[ProtoContract]
	public partial class M2C_BuffInfo: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(96)]
		public long SkillId { get; set; }

		[ProtoMember(2)]
		public string BBKey { get; set; }

		[ProtoMember(95)]
		public long TheUnitBelongToId { get; set; }

		[ProtoMember(91)]
		public long TheUnitFromId { get; set; }

		[ProtoMember(3)]
		public int BuffLayers { get; set; }

		[ProtoMember(4)]
		public float BuffMaxLimitTime { get; set; }

	}

//同步CD信息
	[Message(OuterOpcode_Map.M2C_SyncCDData)]
	[ProtoContract]
	public partial class M2C_SyncCDData: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

//CD名称
//CD名称
		[ProtoMember(2)]
		public string CDName { get; set; }

//CD总时长
//CD总时长
		[ProtoMember(3)]
		public long CDLength { get; set; }

//剩余CD时长
//剩余CD时长
		[ProtoMember(5)]
		public long RemainCDLength { get; set; }

	}

	[Message(OuterOpcode_Map.C2M_FrameCmd)]
	[ProtoContract]
	public partial class C2M_FrameCmd: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(92)]
		public ALSF_Cmd CmdContent { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_FrameCmd)]
	[ProtoContract]
	public partial class M2C_FrameCmd: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long ServerTimeSnap { get; set; }

		[ProtoMember(92)]
		public ALSF_Cmd CmdContent { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_SyncUnitAttribute)]
	[ProtoContract]
	public partial class M2C_SyncUnitAttribute: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

		[ProtoMember(95)]
		public int NumericType { get; set; }

		[ProtoMember(3)]
		public float FinalValue { get; set; }

	}

	[Message(OuterOpcode_Map.M2C_ChangeUnitAttribute)]
	[ProtoContract]
	public partial class M2C_ChangeUnitAttribute: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

		[ProtoMember(95)]
		public int NumericType { get; set; }

		[ProtoMember(2)]
		public float ChangeValue { get; set; }

	}

////////////////////////////////////////////// 战斗相关 END ///////////////////////////////////////////////////////////////
}
