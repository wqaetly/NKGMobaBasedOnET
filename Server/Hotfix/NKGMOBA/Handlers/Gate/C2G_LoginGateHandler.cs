using System;


namespace ET
{
	[MessageHandler]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
		{
			Scene scene = session.DomainScene();
			string account = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
			if (account == null)
			{
				response.Error = ErrorCode.ERR_ConnectGateKeyError;
				response.Message = "Gate key验证失败!";
				reply();
				return;
			}

			// long mapInstanceId = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "Map").InstanceId;
			// long lobbyInstanceId=StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "Lobby").InstanceId;
			// Log.Debug("test-------------------"+mapInstanceId.ToString());
			// Log.Debug("test-------------------"+lobbyInstanceId.ToString());

			PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();
			Player player = playerComponent.AddChild<Player, string>(account);
			playerComponent.Add(player);
			
			session.AddComponent<SessionPlayerComponent>().Player = player;
			player.GateSessionId = session.InstanceId;
			player.GateSession = session;
			
			session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);

			// 随机分配一个Lobby
			StartSceneConfig lobbyConfig = AddressHelper.GetLobby(session.DomainZone());
			
			response.LobbyAddress = lobbyConfig.OuterIPPortForClient.ToString();
			response.PlayerId = player.Id;

			reply();
			await ETTask.CompletedTask;
		}
	}
}