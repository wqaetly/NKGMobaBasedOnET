using System;
using System.Collections.Generic;

namespace ET
{
    //房主点击开始游戏向客户端返回房内玩家列表
    public class C2L_StartGameLobbyHandler : AMRpcHandler<C2L_StartGameLobby, L2C_StartGameLobby>
    {
        protected override async ETTask Run(Session session, C2L_StartGameLobby request, L2C_StartGameLobby response,
            Action reply)
        {
            Scene scene = session.DomainScene();

            Room room = scene.GetComponent<RoomManagerComponent>()
                .GetRoom(Game.Scene.GetComponent<PlayerComponent>().Get(request.PlayerId).RoomId);
            // 随机分配一个Gate
            StartSceneConfig gateConfig = AddressHelper.GetGate(session.DomainZone());

            if (room != null)
            {
                if (room.ContainsPlayers.Count == 6 || room.ContainsPlayers.Count == 2 ||
                    room.ContainsPlayers.Count == 1 || room.ContainsPlayers.Count == 4)
                {
                    foreach (KeyValuePair<long, Player> kvp in room.ContainsPlayers)
                    {
                        response.list.Add(kvp.Key);

                        // Lobby广播给客户端进入战斗
                        kvp.Value.LobbySession.Send(new L2C_PrepareToEnterBattle());

                        // G2L_StartGame createUnit = (G2L_StartGame) await ActorMessageSenderComponent.Instance.Call(gateConfig.InstanceId, new L2G_StartGame() {PlayerId = kvp.Key, Sessionid = 
                        //     room.ContainsPlayers[kvp.Key].GateSessionId,Roomid = room.ContainsPlayers[kvp.Key].RoomId});
                        //重构进入战斗流程
                        Player player = kvp.Value;

                        long mapInstanceId = StartSceneConfigCategory.Instance.GetBySceneName(scene.DomainZone(), "Map")
                            .InstanceId;

                        M2L_CreateHeroUnit createUnit =
                            (M2L_CreateHeroUnit) await ActorMessageSenderComponent.Instance.Call(
                                mapInstanceId,
                                new L2M_CreateHeroUnit()
                                {
                                    PlayerId = player.Id, GateSessionId = player.GateSessionId, Roomid = player.RoomId
                                });

                        response.Message = "开局！";
                        player.UnitId = createUnit.UnitId;
                    }

                    response.Message = "游戏开局成功！";
                }
                else
                {
                    //不符合开局条件返回错误码
                    response.Error = ErrorCode.ERR_StartGameFail;
                }
            }
            else
            {
                response.Error = ErrorCode.ERR_StartGameFail;
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}