//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 19:42:21
//------------------------------------------------------------

using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
    public static class RealmHelper
    {
        /// <summary>
        /// 将玩家踢下线
        /// </summary>
        /// <param name="playerID">playerID</param>
        /// <returns></returns>
        public static async ETTask KickOutPlayer(long playerID, PlayerOfflineTypes playerOfflineType)
        {
            //验证账号是否在线，在线则踢下线
            int gateAppId = Game.Scene.GetComponent<OnlineComponent>().GetGateAppId(playerID);
            //Log.Info("开始验证账号");
            if (gateAppId != 0)
            {
                long m_playerIDInPlayerComponent = Game.Scene.GetComponent<OnlineComponent>().GetPlayerIdInPlayerComponent(playerID);
                //Log.Info($"发送断线信息,playerID:{playerID}");
                Player player = Game.Scene.GetComponent<PlayerComponent>().Get(m_playerIDInPlayerComponent);
                if (player == null)
                {
                    Log.Error("没有获取到player");
                }

                long playerSessionId = player.GetComponent<UnitGateComponent>().GateSessionActorId;
                Session lastGateSession = Game.Scene.GetComponent<NetOuterComponent>().Get(playerSessionId);

                if (lastGateSession == null)
                {
                    Log.Info("没有获取到Session");
                }

                //Log.Info("开始发送下线消息");
                switch (playerOfflineType)
                {
                    case PlayerOfflineTypes.NoPlayForLongTime:
                        // 因长时间未操作而强制下线
                        lastGateSession.Send(new G2C_PlayerOffline() { MPlayerOfflineType = 1 });
                        break;
                    case PlayerOfflineTypes.SamePlayerLogin:
                        // 因账号冲突而强制下线
                        lastGateSession.Send(new G2C_PlayerOffline() { MPlayerOfflineType = 2 });
                        break;
                }

                //Log.Info("下线消息发送完成");
                // 延时1s，保证消息发送完成
                TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();
                await timerComponent.WaitAsync(1000);
                
                //正式移除旧的客户端连接
                Game.Scene.GetComponent<OnlineComponent>().Remove(playerID);
                Game.Scene.GetComponent<NetOuterComponent>().Remove(playerSessionId);
                //Log.Info($"玩家{playerID}已被踢下线");
            }

            await ETTask.CompletedTask;
        }
    }
}