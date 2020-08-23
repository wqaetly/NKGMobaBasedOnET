using System.Collections.Generic;
using System.Numerics;
using ETModel;
using ETModel.BBValues;
using Vector3 = UnityEngine.Vector3;

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
            public string Key;
            public Type Type;
            public ANP_BBValue Value;

            public Notification(string key, Type type, ANP_BBValue value)
            {
                this.Key = key;
                this.Type = type;
                this.Value = value;
            }
        }

        private Clock m_Clock;
        private Dictionary<string, ANP_BBValue> m_Data = new Dictionary<string, ANP_BBValue>();

        private Dictionary<string, List<System.Action<Type, ANP_BBValue>>> m_Observers =
                new Dictionary<string, List<System.Action<Type, ANP_BBValue>>>();

        private bool m_IsNotifiyng = false;

        private Dictionary<string, List<System.Action<Type, ANP_BBValue>>> m_AddObservers =
                new Dictionary<string, List<System.Action<Type, ANP_BBValue>>>();

        private Dictionary<string, List<System.Action<Type, ANP_BBValue>>> m_RemoveObservers =
                new Dictionary<string, List<System.Action<Type, ANP_BBValue>>>();

        private List<Notification> m_Notifications = new List<Notification>();
        private List<Notification> m_NotificationsDispatch = new List<Notification>();
        private Blackboard m_ParentBlackboard;
        private HashSet<Blackboard> m_Children = new HashSet<Blackboard>();

        public Blackboard(Blackboard mParent, Clock mClock)
        {
            this.m_Clock = mClock;
            this.m_ParentBlackboard = mParent;
        }

        public Blackboard(Clock mClock)
        {
            this.m_ParentBlackboard = null;
            this.m_Clock = mClock;
        }

        public void Enable()
        {
            if (this.m_ParentBlackboard != null)
            {
                this.m_ParentBlackboard.m_Children.Add(this);
            }
        }

        /// <summary>
        /// 获取所有键值对
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ANP_BBValue> GetDatas()
        {
            return this.m_Data;
        }
        
        public void Disable()
        {
            if (this.m_ParentBlackboard != null)
            {
                this.m_ParentBlackboard.m_Children.Remove(this);
            }

            if (this.m_Clock != null)
            {
                this.m_Clock.RemoveTimer(this.NotifiyObservers);
            }
        }

        /// <summary>
        /// 设置黑板值，注意此处的T需要是已注册的黑板类型（例如NP_BBValue_Int）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public void Set<T>(string key, T value)
        {
            if (this.m_ParentBlackboard != null && this.m_ParentBlackboard.Isset(key))
            {
                this.m_ParentBlackboard.Set(key, value);
            }
            else
            {
                if (!this.m_Data.ContainsKey(key))
                {
                    ANP_BBValue newBBValue = AutoCreateNPBBValueFromTValue(value);
                    this.m_Data.Add(key, newBBValue);
                    this.m_Notifications.Add(new Notification(key, Type.ADD, newBBValue));
                    this.m_Clock.AddTimer(0f, 0, NotifiyObservers);
                }
                else
                {
                    NP_BBValueBase<T> targetBBValue = this.m_Data[key] as NP_BBValueBase<T>;
                    if ((targetBBValue == null && value != null) ||
                        (targetBBValue != null && !targetBBValue.GetValue().Equals(value)))
                    {
                        targetBBValue.SetValue(value);
                        this.m_Notifications.Add(new Notification(key, Type.CHANGE, targetBBValue));
                        this.m_Clock.AddTimer(0f, 0, NotifiyObservers);
                    }
                }
            }
        }

        public void Unset(string key)
        {
            if (this.m_Data.ContainsKey(key))
            {
                this.m_Data.Remove(key);
                this.m_Notifications.Add(new Notification(key, Type.REMOVE, null));
                this.m_Clock.AddTimer(0f, 0, NotifiyObservers);
            }
        }

        public T Get<T>(string key)
        {
            ANP_BBValue result = Get(key);
            if (result == null)
            {
                return default;
            }

            return (result as NP_BBValueBase<T>).GetValue();
        }

        public ANP_BBValue Get(string key)
        {
            if (this.m_Data.ContainsKey(key))
            {
                return this.m_Data[key];
            }
            else if (this.m_ParentBlackboard != null)
            {
                return this.m_ParentBlackboard.Get(key);
            }
            else
            {
                return null;
            }
        }

        public bool Isset(string key)
        {
            return this.m_Data.ContainsKey(key) || (this.m_ParentBlackboard != null && this.m_ParentBlackboard.Isset(key));
        }

        public void AddObserver(string key, System.Action<Type, ANP_BBValue> observer)
        {
            List<System.Action<Type, ANP_BBValue>> observers = GetObserverList(this.m_Observers, key);
            if (!this.m_IsNotifiyng)
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
                    List<System.Action<Type, ANP_BBValue>> addObservers = GetObserverList(this.m_AddObservers, key);
                    if (!addObservers.Contains(observer))
                    {
                        addObservers.Add(observer);
                    }
                }

                List<System.Action<Type, ANP_BBValue>> removeObservers = GetObserverList(this.m_RemoveObservers, key);
                if (removeObservers.Contains(observer))
                {
                    removeObservers.Remove(observer);
                }
            }
        }

        public void RemoveObserver(string key, System.Action<Type, ANP_BBValue> observer)
        {
            List<System.Action<Type, ANP_BBValue>> observers = GetObserverList(this.m_Observers, key);
            if (!this.m_IsNotifiyng)
            {
                if (observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
            else
            {
                List<System.Action<Type, ANP_BBValue>> removeObservers = GetObserverList(this.m_RemoveObservers, key);
                if (!removeObservers.Contains(observer))
                {
                    if (observers.Contains(observer))
                    {
                        removeObservers.Add(observer);
                    }
                }

                List<System.Action<Type, ANP_BBValue>> addObservers = GetObserverList(this.m_AddObservers, key);
                if (addObservers.Contains(observer))
                {
                    addObservers.Remove(observer);
                }
            }
        }

        private void NotifiyObservers()
        {
            if (this.m_Notifications.Count == 0)
            {
                return;
            }

            this.m_NotificationsDispatch.Clear();
            this.m_NotificationsDispatch.AddRange(this.m_Notifications);
            foreach (Blackboard child in this.m_Children)
            {
                child.m_Notifications.AddRange(this.m_Notifications);
                child.m_Clock.AddTimer(0f, 0, child.NotifiyObservers);
            }

            this.m_Notifications.Clear();

            this.m_IsNotifiyng = true;
            foreach (Notification notification in this.m_NotificationsDispatch)
            {
                if (!this.m_Observers.ContainsKey(notification.Key))
                {
                    //                Debug.Log("1 do not notify for key:" + notification.key + " value: " + notification.value);
                    continue;
                }

                List<System.Action<Type, ANP_BBValue>> observers = GetObserverList(this.m_Observers, notification.Key);
                foreach (System.Action<Type, ANP_BBValue> observer in observers)
                {
                    if (this.m_RemoveObservers.ContainsKey(notification.Key) && this.m_RemoveObservers[notification.Key].Contains(observer))
                    {
                        continue;
                    }

                    observer(notification.Type, notification.Value);
                }
            }

            foreach (string key in this.m_AddObservers.Keys)
            {
                GetObserverList(this.m_Observers, key).AddRange(this.m_AddObservers[key]);
            }

            foreach (string key in this.m_RemoveObservers.Keys)
            {
                foreach (System.Action<Type, ANP_BBValue> action in this.m_RemoveObservers[key])
                {
                    GetObserverList(this.m_Observers, key).Remove(action);
                }
            }

            this.m_AddObservers.Clear();
            this.m_RemoveObservers.Clear();

            this.m_IsNotifiyng = false;
        }

        private List<System.Action<Type, ANP_BBValue>> GetObserverList(Dictionary<string, List<System.Action<Type, ANP_BBValue>>> target, string key)
        {
            List<System.Action<Type, ANP_BBValue>> observers;
            if (target.ContainsKey(key))
            {
                observers = target[key];
            }
            else
            {
                observers = new List<System.Action<Type, ANP_BBValue>>();
                target[key] = observers;
            }

            return observers;
        }

        /// <summary>
        /// 自动从T创建一个NP_BBValue
        /// </summary>
        private static ANP_BBValue AutoCreateNPBBValueFromTValue<T>(T value)
        {
            string valueType = typeof (T).ToString();
            object targetValue = value;
            switch (valueType)
            {
                case "int":
                    NP_BBValue_Int npBbValueInt = new NP_BBValue_Int();
                    npBbValueInt.SetValue((int) targetValue);
                    return npBbValueInt;
                case "Vector3":
                    NP_BBValue_Vector3 npBbValueVector3 = new NP_BBValue_Vector3();
                    npBbValueVector3.SetValue((Vector3) targetValue);
                    return npBbValueVector3;
                default:
                    Log.Error($"未找到类型为{valueType}的NP_BBValue类型");
                    return null;
            }
        }
    }
}