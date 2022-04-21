using System;
using UnityEngine;

namespace ET
{
    public class
        L2M_PreparedToEnterBattleHandler : AMActorRpcHandler<Scene, L2M_PreparedToEnterBattle,
            M2L_PreparedToEnterBattle>
    {
        protected override async ETTask Run(Scene scene, L2M_PreparedToEnterBattle request,
            M2L_PreparedToEnterBattle response,
            Action reply)
        {
            Room room = scene.GetComponent<RoomManagerComponent>().GetRoom(request.Roomid);
            UnitComponent unitComponent = room.GetComponent<UnitComponent>();

            foreach (var players in room.ContainsPlayers)
            {
                Unit hero = unitComponent.Get(players.Value.UnitId);
                //UnitFactory.CompleteHeroComponent(hero);
            }

            room.GetComponent<LSF_Component>().StartFrameSync();

            reply();

            await ETTask.CompletedTask;
        }
    }
}