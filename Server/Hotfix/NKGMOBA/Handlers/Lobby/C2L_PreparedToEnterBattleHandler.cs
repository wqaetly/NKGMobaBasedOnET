using UnityEngine;

namespace ET
{
    public class C2L_PreparedToEnterBattleHandler : AMHandler<C2L_PreparedToEnterBattle>
    {
        protected override async ETVoid Run(Session session, C2L_PreparedToEnterBattle message)
        {
            Player player = Game.Scene.GetComponent<PlayerComponent>().Get(message.PlayerId);
            Scene scene = session.DomainScene();
            Room room = scene.GetComponent<RoomManagerComponent>().GetRoom(player.RoomId);
            if (room.enterNum < room.startGameNum)
            {
                room.enterNum += 1;
                if (room.enterNum == room.startGameNum || (room.enterNum == 1 && room.ContainsPlayers.Count == 1) ||
                    (room.enterNum == 2 && room.ContainsPlayers.Count == 2)||
                    (room.enterNum == 4 && room.ContainsPlayers.Count == 4))
                {
                    foreach (var players in room.ContainsPlayers)
                    {
                        players.Value.GateSession.Send(new L2C_AllowToEnterMap());
                    }

                    long mapInstanceId = StartSceneConfigCategory.Instance.GetBySceneName(scene.DomainZone(), "Map")
                        .InstanceId;

                    M2L_PreparedToEnterBattle preparedToEnterBattle =
                        (M2L_PreparedToEnterBattle) await ActorMessageSenderComponent.Instance.Call(
                            mapInstanceId,
                            new L2M_PreparedToEnterBattle() {Roomid = player.RoomId});

                    room.enterNum = 0; //通知完所有玩家进入后清理该值，防止出现其他错误（暂时）
                    scene.GetComponent<RoomManagerComponent>().RemoveRoom(room.Id); //开局销毁局外房间
                    room.Dispose();
                }
            }

            await ETTask.CompletedTask;
        }
    }
}