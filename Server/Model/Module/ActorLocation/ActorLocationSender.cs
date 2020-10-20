using System.Collections.Generic;
using System.Net;

namespace ETModel
{
	// 知道对方的Id，使用这个类发actor消息
	public class ActorLocationSender : ComponentWithId
	{
		public long ActorId;
		
		// 最近接收或者发送消息的时间
		public long LastSendOrRecvTime;
		
		public int FailTimes;

		public const int MaxFailTimes = 5;

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