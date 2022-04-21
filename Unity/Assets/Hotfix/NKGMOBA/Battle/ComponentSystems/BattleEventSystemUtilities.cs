using System;
using System.Collections.Generic;

namespace ET
{
    public class BattleEventDestroySystem: DestroySystem<BattleEventSystemComponent>
    {
        public override void Destroy(BattleEventSystemComponent self)
        {
            self.Destroy();
        }
    }
    
    public static class BattleEventSystemUtilities
    {
        public static void RegisterEvent(this BattleEventSystemComponent self, string eventId, ISkillSystemEvent e)
        {
            if (!self.AllEvents.ContainsKey(eventId))
            {
                self.AllEvents.Add(eventId, new LinkedList<ISkillSystemEvent>());
            }

            self.AllEvents[eventId].AddLast(e);
        }

        public static void UnRegisterEvent(this BattleEventSystemComponent self,string eventId, ISkillSystemEvent e)
        {
            if (self.CachedNodes.Count > 0)
            {
                foreach (KeyValuePair<string, LinkedListNode<ISkillSystemEvent>> cachedNode in self.CachedNodes)
                {
                    //预防极端情况，比如两个不同的事件id订阅了同一个事件处理者
                    if (cachedNode.Value != null && cachedNode.Key == eventId && cachedNode.Value.Value == e)
                    {
                        //注意这里添加的Handler是下一个
                        self.TempNodes.Add(cachedNode.Key, cachedNode.Value.Next);
                    }
                }

                //把临时结点字典中的目标元素值更新到缓存结点字典
                if (self.TempNodes.Count > 0)
                {
                    foreach (KeyValuePair<string, LinkedListNode<ISkillSystemEvent>> cachedNode in self.TempNodes)
                    {
                        self.CachedNodes[cachedNode.Key] = cachedNode.Value;
                    }

                    //清除临时结点
                    self.TempNodes.Clear();
                }
            }

            if (self.AllEvents.ContainsKey(eventId))
            {
                self.AllEvents[eventId].Remove(e);
                ReferencePool.Release(e);
            }
        }

        public static void Run(this BattleEventSystemComponent self,string type)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!self.AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    self.CachedNodes[type] = temp.Next;
                    temp.Value?.Handle();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = self.CachedNodes[type];
            }

            self.CachedNodes.Remove(type);
        }

        public static void Run<A>(this BattleEventSystemComponent self,string type, A a)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!self.AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    self.CachedNodes[type] = temp.Next;
                    temp.Value?.Handle(a);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = self.CachedNodes[type];
            }

            self.CachedNodes.Remove(type);
        }

        public static void Run<A, B>(this BattleEventSystemComponent self,string type, A a, B b)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!self.AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    self.CachedNodes[type] = temp.Next;
                    temp.Value?.Handle(a, b);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = self.CachedNodes[type];
            }

            self.CachedNodes.Remove(type);
        }

        public static void Run<A, B, C>(this BattleEventSystemComponent self,string type, A a, B b, C c)
        {
            LinkedList<ISkillSystemEvent> iEvents;
            if (!self.AllEvents.TryGetValue(type, out iEvents))
            {
                return;
            }

            LinkedListNode<ISkillSystemEvent> temp = iEvents.First;

            while (temp != null)
            {
                try
                {
                    self.CachedNodes[type] = temp.Next;
                    temp.Value?.Handle(a, b, c);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                temp = self.CachedNodes[type];
            }

            self.CachedNodes.Remove(type);
        }

        public static void Destroy(this BattleEventSystemComponent self)
        {
            self.AllEvents.Clear();
            self.CachedNodes.Clear();
            self.TempNodes.Clear();
        }
    }
}