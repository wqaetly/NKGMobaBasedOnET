using System;

namespace ET
{
	public class PlayerSystem : AwakeSystem<Player, string>
	{
		public override void Awake(Player self, string a)
		{
			self.Awake(a);
		}
	}

	public sealed class Player : Entity
	{
		public string Account { get; private set; }
		
		public long UnitId { get; set; }
		
		public long GateSessionId { get; set; }
		
		public Session GateSession { get; set; }
		
		public Session LobbySession { get; set; }

		/// <summary>
		/// 所归属的RoomId
		/// </summary>
		public long RoomId { get; set; }
		/// <summary>
		/// 所归属的阵营
		/// </summary>
		public Int32 camp { get; set; }

		public void Awake(string account)
		{
			this.Account = account;
		}
	}
}