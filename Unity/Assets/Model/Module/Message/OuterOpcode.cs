using ETModel;
namespace ETModel
{
	[Message(OuterOpcode.C2M_TestRequest)]
	public partial class C2M_TestRequest : IActorLocationRequest {}

	[Message(OuterOpcode.M2C_TestResponse)]
	public partial class M2C_TestResponse : IActorLocationResponse {}

	[Message(OuterOpcode.Actor_TransferRequest)]
	public partial class Actor_TransferRequest : IActorLocationRequest {}

	[Message(OuterOpcode.Actor_TransferResponse)]
	public partial class Actor_TransferResponse : IActorLocationResponse {}

	[Message(OuterOpcode.C2G_EnterMap)]
	public partial class C2G_EnterMap : IRequest {}

	[Message(OuterOpcode.G2C_EnterMap)]
	public partial class G2C_EnterMap : IResponse {}

// 自己的unit id
// 所有的unit
	[Message(OuterOpcode.UnitInfo)]
	public partial class UnitInfo {}

	[Message(OuterOpcode.M2C_CreateUnits)]
	public partial class M2C_CreateUnits : IActorMessage {}

	[Message(OuterOpcode.SpilingInfo)]
	public partial class SpilingInfo {}

//创建木桩
	[Message(OuterOpcode.M2C_CreateSpilings)]
	public partial class M2C_CreateSpilings : IActorMessage {}

	[Message(OuterOpcode.Frame_ClickMap)]
	public partial class Frame_ClickMap : IActorLocationMessage {}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	public partial class M2C_PathfindingResult : IActorMessage {}

	[Message(OuterOpcode.C2R_Ping)]
	public partial class C2R_Ping : IRequest {}

	[Message(OuterOpcode.R2C_Ping)]
	public partial class R2C_Ping : IResponse {}

	[Message(OuterOpcode.G2C_Test)]
	public partial class G2C_Test : IMessage {}

	[Message(OuterOpcode.C2M_Reload)]
	public partial class C2M_Reload : IRequest {}

	[Message(OuterOpcode.M2C_Reload)]
	public partial class M2C_Reload : IResponse {}

	[Message(OuterOpcode.C2G_HeartBeat)]
	public partial class C2G_HeartBeat : IRequest {}

	[Message(OuterOpcode.G2C_HeartBeat)]
	public partial class G2C_HeartBeat : IResponse {}

	[Message(OuterOpcode.M2C_BuffInfo)]
	public partial class M2C_BuffInfo : IActorMessage {}

//请求攻击
	[Message(OuterOpcode.C2M_CommonAttack)]
	public partial class C2M_CommonAttack : IActorLocationMessage {}

//服务器返回攻击指令，开始播放动画
	[Message(OuterOpcode.M2C_CommonAttack)]
	public partial class M2C_CommonAttack : IActorMessage {}

}
namespace ETModel
{
	public static partial class OuterOpcode
	{
		 public const ushort C2M_TestRequest = 101;
		 public const ushort M2C_TestResponse = 102;
		 public const ushort Actor_TransferRequest = 103;
		 public const ushort Actor_TransferResponse = 104;
		 public const ushort C2G_EnterMap = 105;
		 public const ushort G2C_EnterMap = 106;
		 public const ushort UnitInfo = 107;
		 public const ushort M2C_CreateUnits = 108;
		 public const ushort SpilingInfo = 109;
		 public const ushort M2C_CreateSpilings = 110;
		 public const ushort Frame_ClickMap = 111;
		 public const ushort M2C_PathfindingResult = 112;
		 public const ushort C2R_Ping = 113;
		 public const ushort R2C_Ping = 114;
		 public const ushort G2C_Test = 115;
		 public const ushort C2M_Reload = 116;
		 public const ushort M2C_Reload = 117;
		 public const ushort C2G_HeartBeat = 118;
		 public const ushort G2C_HeartBeat = 119;
		 public const ushort M2C_BuffInfo = 120;
		 public const ushort C2M_CommonAttack = 121;
		 public const ushort M2C_CommonAttack = 122;
	}
}
