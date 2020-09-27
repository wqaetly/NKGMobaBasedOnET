using System;
using System.Net;

namespace ETModel
{
	public class ActorMessageSenderComponent: Component
	{
		/// <summary>
		/// 根据actorId获取一个Actor信息发送者，包含gatesession的Id以及目标内网地址
		/// </summary>
		/// <param name="actorId"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public ActorMessageSender Get(long actorId)
		{
			if (actorId == 0)
			{
				throw new Exception($"actor id is 0");
			}
			IPEndPoint ipEndPoint = StartConfigComponent.Instance.GetInnerAddress(IdGenerater.GetAppId(actorId));
			ActorMessageSender actorMessageSender = new ActorMessageSender(actorId, ipEndPoint);
			return actorMessageSender;
		}
	}
}
