//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月19日 11:28:12
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;

namespace NPBehave
{
    public class SyncContext
    {
        private static SyncContext _instance;

        /// <summary>
        /// 世界默认更新频率为30hz
        /// </summary>
        private const float c_GameUpdateInterval = 1f / 30f;

        /// <summary>
        /// 行为树默认更新频率为15hz
        /// </summary>
        private const float s_UpdateInterval = c_GameUpdateInterval * 2;

        /// <summary>
        /// 计时器
        /// </summary>
        private static float s_Timer = s_UpdateInterval;

        public static SyncContext Instance
        {
            get
            {
                return _instance ?? (_instance = new SyncContext());
            }
        }

        private Dictionary<string, Blackboard> blackboards = new Dictionary<string, Blackboard>();

        private Clock clock = new Clock();

        public Clock GetClock()
        {
            return Instance.clock;
        }

        public static Blackboard GetSharedBlackboard(string key)
        {
            if (!Instance.blackboards.ContainsKey(key))
            {
                Instance.blackboards.Add(key, new Blackboard(Instance.clock));
            }

            return Instance.blackboards[key];
        }

        public void Update()
        {
            s_Timer += c_GameUpdateInterval;
            if (s_Timer >= s_UpdateInterval)
            {
                //默认15hz运行
                clock.Update(s_UpdateInterval);
                s_Timer = 0;
            }
        }
    }
}