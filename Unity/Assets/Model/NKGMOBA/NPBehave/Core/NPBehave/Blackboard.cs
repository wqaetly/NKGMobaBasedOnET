using System.Collections.Generic;
using System.Numerics;
using ETModel;

namespace NPBehave
{
    public class Blackboard
    {
        public enum Type
        {
            ADD,
            REMOVE,
            CHANGE
        }

        private struct Notification
        {
            public string key;
            public Type type;
            public NP_BlackBoardDataForCompare value;

            public Notification(string key, Type type, NP_BlackBoardDataForCompare value)
            {
                this.key = key;
                this.type = type;
                this.value = value;
            }
        }

        private Clock clock;
        private Dictionary<string, NP_BlackBoardDataForCompare> data = new Dictionary<string, NP_BlackBoardDataForCompare>();
        private Dictionary<string, List<System.Action<Type, object>>> observers = new Dictionary<string, List<System.Action<Type, object>>>();
        private bool isNotifiyng = false;
        private Dictionary<string, List<System.Action<Type, object>>> addObservers = new Dictionary<string, List<System.Action<Type, object>>>();
        private Dictionary<string, List<System.Action<Type, object>>> removeObservers = new Dictionary<string, List<System.Action<Type, object>>>();
        private List<Notification> notifications = new List<Notification>();
        private List<Notification> notificationsDispatch = new List<Notification>();
        private HashSet<Blackboard> children = new HashSet<Blackboard>();

        public Blackboard(Clock clock)
        {
            this.clock = clock;
        }

        public void Disable()
        {
            if (this.clock != null)
            {
                this.clock.RemoveTimer(this.NotifiyObservers);
            }
        }

        public NP_BlackBoardDataForCompare this[string key]
        {
            get
            {
                //暂时一颗行为树支持一个黑板
                return this.GetFromSelf(key);
            }
            set
            {
                Set(key, value);
            }
        }

        public void Set(string key)
        {
            if (!Isset(key))
            {
                Set(key, null);
            }
        }

        /// <summary>
        /// 刷新键为key的数据
        /// </summary>
        /// <param name="key"></param>
        public void RefreshData(string key)
        {
            this.notifications.Add(new Notification(key, Type.CHANGE, this.data[key]));
            this.clock.AddTimer(0f, 0, NotifiyObservers);
        }

        public void SetFloat(string key, float value)
        {
            GetFromSelf(key)._float = value;
            RefreshData(key);
        }
        
        public void SetInt(string key, int value)
        {
            GetFromSelf(key)._int = value;
            RefreshData(key);
        }

        public void SetBool(string key, bool value)
        {
            GetFromSelf(key)._bool = value;
            RefreshData(key);
        }
        
        public void SetLong(string key, long value)
        {
            GetFromSelf(key)._long = value;
            RefreshData(key);
        }
        
        public void SetString(string key, string value)
        {
            GetFromSelf(key)._string = value;
            RefreshData(key);
        }

        /// <summary>
        /// 整体赋值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, NP_BlackBoardDataForCompare value)
        {
            if (!this.data.ContainsKey(key))
            {
                this.data[key] = value;
                this.notifications.Add(new Notification(key, Type.ADD, value));
                this.clock.AddTimer(0f, 0, NotifiyObservers);
            }
            else
            {
                this.data[key] = value;
                this.notifications.Add(new Notification(key, Type.CHANGE, value));
                this.clock.AddTimer(0f, 0, NotifiyObservers);
            }
        }

        public void Unset(string key)
        {
            if (this.data.ContainsKey(key))
            {
                this.data.Remove(key);
                this.notifications.Add(new Notification(key, Type.REMOVE, null));
                this.clock.AddTimer(0f, 0, NotifiyObservers);
            }
        }

        public NP_BlackBoardDataForCompare GetFromSelf(string key)
        {
            if (this.data.ContainsKey(key))
            {
                return data[key];
            }
            
            NP_BlackBoardDataForCompare temp = new NP_BlackBoardDataForCompare();
            this.data.Add(key, temp);
            this.notifications.Add(new Notification(key, Type.ADD, temp));
            this.clock.AddTimer(0f, 0, NotifiyObservers);
            
            return data[key];
        }

        public bool Isset(string key)
        {
            return this.data.ContainsKey(key);
        }

        public void AddObserver(string key, System.Action<Type, object> observer)
        {
            List<System.Action<Type, object>> observers = GetObserverList(this.observers, key);
            if (!isNotifiyng)
            {
                if (!observers.Contains(observer))
                {
                    observers.Add(observer);
                }
            }
            else
            {
                if (!observers.Contains(observer))
                {
                    List<System.Action<Type, object>> addObservers = GetObserverList(this.addObservers, key);
                    if (!addObservers.Contains(observer))
                    {
                        addObservers.Add(observer);
                    }
                }

                List<System.Action<Type, object>> removeObservers = GetObserverList(this.removeObservers, key);
                if (removeObservers.Contains(observer))
                {
                    removeObservers.Remove(observer);
                }
            }
        }

        public void RemoveObserver(string key, System.Action<Type, object> observer)
        {
            List<System.Action<Type, object>> observers = GetObserverList(this.observers, key);
            if (!isNotifiyng)
            {
                if (observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
            else
            {
                List<System.Action<Type, object>> removeObservers = GetObserverList(this.removeObservers, key);
                if (!removeObservers.Contains(observer))
                {
                    if (observers.Contains(observer))
                    {
                        removeObservers.Add(observer);
                    }
                }

                List<System.Action<Type, object>> addObservers = GetObserverList(this.addObservers, key);
                if (addObservers.Contains(observer))
                {
                    addObservers.Remove(observer);
                }
            }
        }

#if UNITY_EDITOR
        public List<string> Keys
        {
            get
            {
                return new List<string>(data.Keys);
            }
        }

        public int NumObservers
        {
            get
            {
                int count = 0;
                foreach (string key in observers.Keys)
                {
                    count += observers[key].Count;
                }

                return count;
            }
        }
#endif

        private void NotifiyObservers()
        {
            if (notifications.Count == 0)
            {
                return;
            }

            notificationsDispatch.Clear();
            notificationsDispatch.AddRange(notifications);
            foreach (Blackboard child in children)
            {
                child.notifications.AddRange(notifications);
                child.clock.AddTimer(0f, 0, child.NotifiyObservers);
            }

            notifications.Clear();

            isNotifiyng = true;
            foreach (Notification notification in notificationsDispatch)
            {
                if (!this.observers.ContainsKey(notification.key))
                {
                    //                Debug.Log("1 do not notify for key:" + notification.key + " value: " + notification.value);
                    continue;
                }

                List<System.Action<Type, object>> observers = GetObserverList(this.observers, notification.key);
                foreach (System.Action<Type, object> observer in observers)
                {
                    if (this.removeObservers.ContainsKey(notification.key) && this.removeObservers[notification.key].Contains(observer))
                    {
                        continue;
                    }

                    observer(notification.type, notification.value);
                }
            }

            foreach (string key in this.addObservers.Keys)
            {
                GetObserverList(this.observers, key).AddRange(this.addObservers[key]);
            }

            foreach (string key in this.removeObservers.Keys)
            {
                foreach (System.Action<Type, object> action in removeObservers[key])
                {
                    GetObserverList(this.observers, key).Remove(action);
                }
            }

            this.addObservers.Clear();
            this.removeObservers.Clear();

            isNotifiyng = false;
        }

        private List<System.Action<Type, object>> GetObserverList(Dictionary<string, List<System.Action<Type, object>>> target, string key)
        {
            List<System.Action<Type, object>> observers;
            if (target.ContainsKey(key))
            {
                observers = target[key];
            }
            else
            {
                observers = new List<System.Action<Type, object>>();
                target[key] = observers;
            }

            return observers;
        }
    }
}