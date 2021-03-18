namespace ETModel
{
	[ObjectSystem]
	public class PlayerSystem : AwakeSystem<Player, long>
	{
		public override void Awake(Player self, long a)
		{
			self.Awake(a);
		}
	}

	public sealed class Player : Entity
	{
		/// <summary>
		/// 数据库中玩家Id
		/// </summary>
		public long PlayerIdInDB { get; private set; }
		
		public long UnitId { get; set; }

		public void Awake(long account)
		{
			this.PlayerIdInDB = account;
		}
		
		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();
		}
	}
}