using ETModel;
namespace ETHotfix
{
//登录部分
	[Message(HotfixOpcode.C2R_Login)]
	public partial class C2R_Login : IRequest {}

	[Message(HotfixOpcode.R2C_Login)]
	public partial class R2C_Login : IResponse {}

	[Message(HotfixOpcode.C2G_LoginGate)]
	public partial class C2G_LoginGate : IRequest {}

	[Message(HotfixOpcode.G2C_LoginGate)]
	public partial class G2C_LoginGate : IResponse {}

//注册部分
	[Message(HotfixOpcode.C2R_Register)]
	public partial class C2R_Register : IRequest {}

	[Message(HotfixOpcode.R2C_Register)]
	public partial class R2C_Register : IResponse {}

// 获取用户信息
	[Message(HotfixOpcode.C2G_GetUserInfo)]
	public partial class C2G_GetUserInfo : IRequest {}

// 获取用户信息
	[Message(HotfixOpcode.G2C_GetUserInfo)]
	public partial class G2C_GetUserInfo : IResponse {}

	[Message(HotfixOpcode.G2C_TestHotfixMessage)]
	public partial class G2C_TestHotfixMessage : IMessage {}

	[Message(HotfixOpcode.C2M_TestActorRequest)]
	public partial class C2M_TestActorRequest : IActorLocationRequest {}

	[Message(HotfixOpcode.M2C_TestActorResponse)]
	public partial class M2C_TestActorResponse : IActorLocationResponse {}

	[Message(HotfixOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(HotfixOpcode.C2G_PlayerInfo)]
	public partial class C2G_PlayerInfo : IRequest {}

	[Message(HotfixOpcode.G2C_PlayerInfo)]
	public partial class G2C_PlayerInfo : IResponse {}

	[Message(HotfixOpcode.G2C_PlayerOffline)]
	public partial class G2C_PlayerOffline : IMessage {}

}
namespace ETHotfix
{
	public static partial class HotfixOpcode
	{
		 public const ushort C2R_Login = 10001;
		 public const ushort R2C_Login = 10002;
		 public const ushort C2G_LoginGate = 10003;
		 public const ushort G2C_LoginGate = 10004;
		 public const ushort C2R_Register = 10005;
		 public const ushort R2C_Register = 10006;
		 public const ushort C2G_GetUserInfo = 10007;
		 public const ushort G2C_GetUserInfo = 10008;
		 public const ushort G2C_TestHotfixMessage = 10009;
		 public const ushort C2M_TestActorRequest = 10010;
		 public const ushort M2C_TestActorResponse = 10011;
		 public const ushort PlayerInfo = 10012;
		 public const ushort C2G_PlayerInfo = 10013;
		 public const ushort G2C_PlayerInfo = 10014;
		 public const ushort G2C_PlayerOffline = 10015;
	}
}
