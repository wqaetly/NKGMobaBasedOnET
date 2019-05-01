using System;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override void Run(Session session, C2G_LoginGate message, Action<G2C_LoginGate> reply)
		{
			G2C_LoginGate response = new G2C_LoginGate();
			try
			{
				//从已经分发的KEY里面寻找，如果没找到，说明非法用户，不给他连接gate服务器
				string account = Game.Scene.GetComponent<GateSessionKeyComponent>().Get(message.Key);
				if (account == null)
				{
					response.Error = ErrorCode.ERR_ConnectGateKeyError;
					response.Message = "Gate key验证失败!";
					reply(response);
					return;
				}
				//专门给这个玩家创建一个Player对象
				Player player = ComponentFactory.Create<Player, string>(account);
				//注册到PlayerComponent，方便管理
				Game.Scene.GetComponent<PlayerComponent>().Add(player);

				//给这个session安排上Player
				session.AddComponent<SessionPlayerComponent>().Player = player;
				//添加邮箱组件表示该session是一个Actor,接收的消息将会队列处理
				session.AddComponent<MailBoxComponent, string>(MailboxType.GateSession);

				response.PlayerId = player.Id;
				
				//回复客户端的连接gate服务器请求
				reply(response);

				//向客户端发送热更层信息
				session.Send(new G2C_TestHotfixMessage() { Info = "recv hotfix message success" });
			}
			catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}
	}
}