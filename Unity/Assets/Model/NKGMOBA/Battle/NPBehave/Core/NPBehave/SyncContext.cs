//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月19日 11:28:12
//------------------------------------------------------------

using System.Collections.Generic;
using ET;

namespace NPBehave
{
    public class SyncContext
    {
        private Dictionary<string, Blackboard> blackboards = new Dictionary<string, Blackboard>();

        private Clock clock;

        public SyncContext(NP_SyncComponent npSyncComponent)
        {
            clock = new Clock(npSyncComponent.GetParent<Unit>().BelongToRoom.GetComponent<LSF_Component>());
        }

        public Clock GetClock()
        {
            return clock;
        }

        public Blackboard GetSharedBlackboard(string key)
        {
            if (!blackboards.ContainsKey(key))
            {
                blackboards.Add(key, new Blackboard(clock));
            }

            return blackboards[key];
        }

        public void Update()
        {
            clock.Update();
        }
    }
}