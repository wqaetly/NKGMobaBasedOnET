using System;

namespace ET
{
    public class StartGameHelper
    {
        public static async ETTask StartGame(Entity fuiComponent)
        {
            try
            {
                Scene zoneScene = fuiComponent.DomainScene();

                PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();
                
                await playerComponent.LobbySession.Call(new C2L_StartGameLobby()
                        {PlayerId = playerComponent.PlayerId});
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}