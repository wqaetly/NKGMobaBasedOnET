namespace ET
{
	public class PlayerComponentDestroySystem: DestroySystem<PlayerComponent>
	{
		public override void Destroy(PlayerComponent self)
		{
			self.GateSession?.Dispose();
		}
	}
}
