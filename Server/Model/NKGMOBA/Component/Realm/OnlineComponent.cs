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
        private readonly Dictionary<string, Tuple<long, int>> m_dictionarty = new Dictionary<string, Tuple<long, int>>();

        /// <summary>
        /// 添加在线玩家
        /// </summary>
        /// <param name="playerAccount"></param>
        /// <param name="gateAppId"></param>
        public void Add(string playerAccount, long playerId, int gateAppId)
        {
            this.m_dictionarty.Add(playerAccount, new Tuple<long, int>(playerId, gateAppId));
        }

        /// <summary>
        /// 获取在线玩家ID
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public long GetPlayerId(string playerAccount)
        {
            Tuple<long, int> temp = new Tuple<long, int>(0, 0);
            this.m_dictionarty.TryGetValue(playerAccount, out temp);
            return temp.Item1;
        }

        /// <summary>
        /// 获取在线玩家网关服务器ID
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public int GetGateAppId(string playerAccount)
        {
            if (this.m_dictionarty.Count >= 1)
            {
                Tuple<long, int> temp = new Tuple<long, int>(0, 0);
                this.m_dictionarty.TryGetValue(playerAccount, out temp);
                if (temp != null && temp.Item2 != 0 && temp.Item1 != 0)
                    return temp.Item2;
                else
                {
                    return 0;
                }
            }

            return 0;
        }

        /// <summary>
        /// 移除在线玩家
        /// </summary>
        /// <param name="playerId"></param>
        public void Remove(string playerAccount)
        {
            Tuple<long, int> temp;
            if (!this.m_dictionarty.TryGetValue(playerAccount, out temp)) return;
            this.m_dictionarty.Remove(playerAccount);
        }
    }
}