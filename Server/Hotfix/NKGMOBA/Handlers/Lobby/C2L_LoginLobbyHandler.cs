using System;
using System.Collections.Generic;

namespace ET
{
    //登录lobby返回房间列表
    public class C2L_LoginLobbyHandler : AMRpcHandler<C2L_LoginLobby, L2C_LoginLobby>
    {
        protected override async ETTask Run(Session session, C2L_LoginLobby request, L2C_LoginLobby response,
            Action reply)
        {
            Scene scene = session.DomainScene();

            Dictionary<long,Room> roomList = scene.GetComponent<RoomManagerComponent>().Rooms;
            if (roomList.Count > 0)
            {
                foreach (KeyValuePair<long, Room> kvp in roomList)
                {
                    response.RoomIdList.Add(kvp.Key);
                    response.RoomNameList.Add(roomList[kvp.Key].RoomName);
                    response.RoomPlayerNum.Add(roomList[kvp.Key].ContainsPlayers.Count);
                    response.RoomHolderPlayerList.Add(roomList[kvp.Key].RoomHolder.Id);
                }
            }

            Game.Scene.GetComponent<PlayerComponent>().Get(request.PlayerId).LobbySession = session;
            
            response.Message = "目前房间列表";

            reply();
            await ETTask.CompletedTask;
        }
    }
}