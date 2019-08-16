using System.Collections.Generic;

namespace ETModel
{
	public class GateSessionKeyComponent : Component
	{
		private readonly Dictionary<long, long> sessionKey = new Dictionary<long, long>();
		
		public void Add(long key, long account)
		{
			this.sessionKey.Add(key, account);
			this.TimeoutRemoveKey(key).Coroutine();
		}

		public long Get(long key)
		{
			long account = 0;
			this.sessionKey.TryGetValue(key, out account);
			return account;
		}

		public void Remove(long key)
		{
			this.sessionKey.Remove(key);
		}

		private async ETVoid TimeoutRemoveKey(long key)
		{
			await Game.Scene.GetComponent<TimerComponent>().WaitAsync(20000);
			this.sessionKey.Remove(key);
		}
	}
}
