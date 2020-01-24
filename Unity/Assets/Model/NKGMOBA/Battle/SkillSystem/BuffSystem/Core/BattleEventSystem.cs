//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月21日 15:10:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 战斗系统中的事件系统组件，一场战斗挂载一个，这里是Demo，就直接挂在Game.Scene里
    /// </summary>
    public class BattleEventSystem: Component
    {
        private readonly Dictionary<string, List<IEvent>> allEvents = new Dictionary<string, List<IEvent>>();

        public void RegisterEvent(string eventId, IEvent e)
        {
            if (!this.allEvents.ContainsKey(eventId))
            {
                this.allEvents.Add(eventId, new List<IEvent>());
            }

            this.allEvents[eventId].Add(e);
        }

        public void UnRegisterEvent(string eventId, IEvent e)
        {
            if (this.allEvents.ContainsKey(eventId))
            {
                this.allEvents[eventId].Remove(e);
            }
        }

        public void Run(string type)
        {
            List<IEvent> iEvents;
            if (!this.allEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            foreach (IEvent iEvent in iEvents)
            {
                try
                {
                    iEvent?.Handle();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Run<A>(string type, A a)
        {
            List<IEvent> iEvents;
            if (!this.allEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            foreach (IEvent iEvent in iEvents)
            {
                try
                {
                    iEvent?.Handle(a);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Run<A, B>(string type, A a, B b)
        {
            List<IEvent> iEvents;
            if (!this.allEvents.TryGetValue(type, out iEvents))
            {
                return;
            }
            foreach (IEvent iEvent in iEvents)
            {
                try
                {
                    iEvent?.Handle(a, b);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Run<A, B, C>(string type, A a, B b, C c)
        {
            List<IEvent> iEvents;
            if (!this.allEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            foreach (IEvent iEvent in iEvents)
            {
                try
                {
                    iEvent?.Handle(a, b, c);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}