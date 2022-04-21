namespace ET
{
	/// <summary>
	/// 添加到GateSession上，代表这个GateSession是某个玩家的链接，可以通过Player获取具体玩家
	/// 可以为ActorLocation机制提供玩家数据
	/// </summary>
	public class SessionPlayerComponent : Entity
	{
		public Player Player;
	}
}