using System;
using System.Collections.Generic;

namespace ET
{
    //玩家加入房间
    public class C2L_JoinRoomHandler : AMRpcHandler<C2L_JoinRoomLobby, L2C_JoinRoomLobby>
    {
        protected override async ETTask Run(Session session, C2L_JoinRoomLobby request, L2C_JoinRoomLobby response,
            Action reply)
        {
            Scene scene = session.DomainScene();
            Room room = scene.GetComponent<RoomManagerComponent>().GetRoom(request.RoomId);

            //房间不为空向房间里添加玩家
            if (room != null && room.ContainsPlayers.Count < 6)
            {
                Player Jplayer = Game.Scene.GetComponent<PlayerComponent>().Get(request.PlayerId);
                Jplayer.RoomId = request.RoomId;

                for (int i = 1; i <= 6; i++)
                {
                    if (!room.PlayersCamp.TryGetValue(i, out long playerid))
                    {
                        Jplayer.camp = i;
                        room.PlayersCamp.Add(i,Jplayer.Id);
                        break;
                    }
                }
                // 加入房间把自己广播给周围的人
                L2C_PlayerTriggerRoom joinRoom = new L2C_PlayerTriggerRoom();
                joinRoom.playerInfoRoom = new PlayerInfoRoom();
                joinRoom.playerInfoRoom.Account = Jplayer.Account;
                joinRoom.playerInfoRoom.UnitId = Jplayer.UnitId;
                joinRoom.playerInfoRoom.SessionId = Jplayer.GateSessionId;
                joinRoom.playerInfoRoom.RoomId = Jplayer.RoomId;
                joinRoom.playerInfoRoom.camp = Jplayer.camp;
                joinRoom.playerInfoRoom.playerid = Jplayer.Id;
                joinRoom.JoinOrLeave = true;

                foreach (KeyValuePair<long, Player> kvp in room.ContainsPlayers)
                {
                    PlayerInfoRoom pInfo = new PlayerInfoRoom();
                    Player Tplayer = room.ContainsPlayers[kvp.Key];
                    pInfo.Account = Tplayer.Account;
                    pInfo.UnitId = Tplayer.UnitId;
                    pInfo.SessionId = Tplayer.GateSessionId;
                    pInfo.RoomId = Tplayer.RoomId;
                    pInfo.playerid = Tplayer.Id;
                    pInfo.camp = Tplayer.camp;
                    response.playerInfoRoom.Add(pInfo);
                    room.ContainsPlayers[kvp.Key].GateSession.Send(joinRoom);
                }
                
                room.ContainsPlayers.Add(request.PlayerId, Jplayer);
                
                response.Message = "房间加入成功！";
                response.camp = Jplayer.camp;
                response.RoomId = request.RoomId;
                response.RoomHolderId = room.RoomHolder.Id;
                response.RoomName = room.RoomName;
                
            }
            else
            {
                response.Error = ErrorCode.ERR_RoomNotExist;
            }

            reply();
            await ETTask.CompletedTask;
        }
    }


    //点击创建房间逻辑
    public class C2L_CreateNewRoomLobbyHandler : AMRpcHandler<C2L_CreateNewRoomLobby, L2C_CreateNewRoomLobby>
    {
        protected override async ETTask Run(Session session, C2L_CreateNewRoomLobby request,
            L2C_CreateNewRoomLobby response,
            Action reply)
        {
            Scene scene = session.DomainScene();

            int roomId = scene.GetComponent<RoomManagerComponent>().RoomIdNum + 1;
            scene.GetComponent<RoomManagerComponent>().RoomIdNum += 1;
            Room room = scene.GetComponent<RoomManagerComponent>().CreateLobbyRoom(roomId,6);//暂时指定开局人数为6，后续根据需求扩展
            
            //房间不为空向房间里添加玩家
            if (room != null)
            {
                Player Dplayer = Game.Scene.GetComponent<PlayerComponent>().Get(request.PlayerId);
                room.RoomHolder = Dplayer;
                Dplayer.RoomId = roomId;
                Dplayer.camp = room.ContainsPlayers.Count + 1;

                room.ContainsPlayers.Add(request.PlayerId, Dplayer);
                room.RoomName = Dplayer.Account + " s room";
                room.PlayersCamp.Add(Dplayer.camp, request.PlayerId);

                response.Message = room.RoomName;
                response.RoomId = roomId;
                response.mode = 1; //暂时一种模式写死
                response.camp = Dplayer.camp;
                
                TimerComponent.Instance.NewRepeatedTimer(2000,
                    () =>
                    {
                        Room troom=scene.GetComponent<RoomManagerComponent>().GetRoom(roomId);
                        if (troom != null)
                        {
                            if (troom.RoomHolder != null)
                            {
                                if (troom.RoomHolder.LobbySession.IsDisposed)
                                {
                                    scene.GetComponent<RoomManagerComponent>().RemoveRoom(troom.Id);
                                    room.Dispose();
                                }
                            }
                        }


                    });
            }
            else
            {
                response.Error = ErrorCode.ERR_RoomNotExist;
            }

            reply();
            await ETTask.CompletedTask;
        }
    }

    //玩家离开房间逻辑
    public class C2L_LeaveRoomLobbyHandler : AMRpcHandler<C2L_LeaveRoomLobby, L2C_LeaveRoomLobby>
    {
        protected override async ETTask Run(Session session, C2L_LeaveRoomLobby request, L2C_LeaveRoomLobby response,
            Action reply)
        {
            Scene scene = session.DomainScene();

            Player Lplayer = Game.Scene.GetComponent<PlayerComponent>().Get(request.PlayerId);
            
            // 客户端将不会发送离开的房间Id,因为服务端有玩家所在房间信息，为了安全以服务端为准
            Room room = scene.GetComponent<RoomManagerComponent>().GetRoom(Lplayer.RoomId);
            
            if (room != null)
            {
                room.ContainsPlayers.Remove(request.PlayerId);
                room.PlayersCamp.Remove(Lplayer.camp);

                if (room.ContainsPlayers.Count != 0)
                {
                    if (room.RoomHolder == Lplayer)
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (room.PlayersCamp.TryGetValue(i, out long newHolder))
                            {
                                room.RoomHolder = Game.Scene.GetComponent<PlayerComponent>().Get(newHolder);
                                response.newRoomHolder = newHolder;
                                break;
                            }
                        }
                    }
                    
                    // 离开房间把自己广播给周围的人
                    L2C_PlayerTriggerRoom leaveRoom = new L2C_PlayerTriggerRoom();
                    leaveRoom.playerInfoRoom = new PlayerInfoRoom();
                    leaveRoom.playerInfoRoom.Account = Lplayer.Account;
                    leaveRoom.playerInfoRoom.UnitId = Lplayer.UnitId;
                    leaveRoom.playerInfoRoom.SessionId = Lplayer.GateSessionId;
                    leaveRoom.playerInfoRoom.RoomId = Lplayer.RoomId;
                    leaveRoom.playerInfoRoom.camp = Lplayer.camp;
                    leaveRoom.playerInfoRoom.playerid = Lplayer.Id;
                    leaveRoom.JoinOrLeave = false;

                    foreach (KeyValuePair<long, Player> kvp in room.ContainsPlayers)
                    {
                        room.ContainsPlayers[kvp.Key].GateSession.Send(leaveRoom);
                    }
                    
                    response.Message = "player leave room!";
                    response.camp = Lplayer.camp;
                    response.isDestory = false;
                    response.RoomId = Lplayer.RoomId;
                }
                else
                {
                    // 客户端将不会发送离开的房间Id,因为服务端有玩家所在房间信息，为了安全以服务端为准
                    scene.GetComponent<RoomManagerComponent>().RemoveRoom(Lplayer.RoomId);
                    // 返回离开的RoomId
                    response.Message = "player leave room ,no player destory room";
                    response.camp = Lplayer.camp;
                    response.isDestory = true;
                    response.RoomId = Lplayer.RoomId;
                }
            }
            else
            {
                response.Error = ErrorCode.ERR_RoomNotExist;
            }

            response.PlayerId = Lplayer.Id;
            reply();
            await ETTask.CompletedTask;
        }
    }
}