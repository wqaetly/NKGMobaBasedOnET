namespace ET
{
	public class PlayerComponent: Entity
	{
		public Session GateSession;
		public Session LobbySession;
		
		/// <summary>
		/// 玩家归属的房间，在大厅是组队房间，在战斗中是战斗房间
		/// </summary>
		public Room BelongToRoom;

		/// <summary>
		/// 已经完成的加载次数
		/// </summary>
		public int HasCompletedLoadCount;
		
		public long PlayerId;
		public string PlayerAccount;
	}
}
