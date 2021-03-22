//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月1日 19:50:44
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 在线组件，用于记录在线玩家
    /// </summary>
    public class OnlineComponent: Component
    {
        /// <summary>
        /// 记录玩家在线情况的字典，long为PlayerIdInDB（MongoDB数据库中的账号Id），long为PlayerComonent的id，int为GateAppID
        /// </summary>
        private readonly Dictionary<long, (long, int)> m_dictionarty = new Dictionary<long, (long, int)>();

        /// <summary>
        /// 添加在线玩家
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="playerIDInPlayerComponent"></param>
        /// <param name="gateAppId"></param>
        public void Add(long playerId, long playerIDInPlayerComponent, int gateAppId)
        {
            this.m_dictionarty.Add(playerId, (playerIDInPlayerComponent, gateAppId));
        }

        /// <summary>
        /// 获取在线玩家网关服务器ID
        /// </summary>
        /// <param name="playerID">玩家ID（MongoDB数据库中的）</param>
        /// <returns></returns>
        public int GetGateAppId(long playerID)
        {
            if (this.m_dictionarty.TryGetValue(playerID, out (long,int) tempGateAppID))
            {
                return tempGateAppID.Item2;
            }

            //Log.Error($"没有找到id为{playerID}的玩家");
            return tempGateAppID.Item2;
        }
        
        /// <summary>
        /// 根据玩家账号Id(数据库中账号Id)获取在线玩家id(PlayerComponent中的id)
        /// </summary>
        /// <param name="playerIdInDB">玩家Id（MongoDB数据库中的）</param>
        /// <returns></returns>
        public long GetPlayerIdInPlayerComponent(long playerIdInDB)
        {
            if (this.m_dictionarty.TryGetValue(playerIdInDB, out (long,int) tempGateAppID))
            {
                return tempGateAppID.Item1;
            }

            //Log.Error($"没有找到id为{playerID}的玩家");
            return tempGateAppID.Item1;
        }

        /// <summary>
        /// 移除在线玩家
        /// </summary>
        /// <param name="player"></param>
        public void Remove(Player player)
        {
            this.m_dictionarty.Remove(player.PlayerIdInDB);
            //TODO 注意这里是直接找UnitComponent进行Remove Map上的Unit，但正确做法是应该考虑分布式服务器，发送消息给Map，让Map去管理
            //TODO 或者说玩家离线时不需要Remove，只在一整局游戏结束之后再Remove，这样更安全
            //UnitComponent.Instance.Remove(player.UnitId);
        }
    }
}