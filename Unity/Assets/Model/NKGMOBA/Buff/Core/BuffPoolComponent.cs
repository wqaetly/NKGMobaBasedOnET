//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 22:28:26
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// Buff池组件,包括Buff的事件池化
    /// </summary>
    public class BuffPoolComponent: Component
    {
        public Dictionary<Type, Queue<BuffSystemBase>> MBuffBases = new Dictionary<Type, Queue<BuffSystemBase>>();

        public T AcquireBuff<T, A>(A a) where T : BuffSystemBase where A : BuffDataBase
        {
            Queue<BuffSystemBase> buffBase;
            if (this.MBuffBases.TryGetValue(typeof (T), out buffBase))
            {
                if (buffBase.Count > 0)
                {
                    return (T) buffBase.Dequeue();
                }
            }

            T temp = (T) Activator.CreateInstance(typeof (T));
            temp.MSkillBuffDataBase = a;
            return temp;
        }

        public void RecycleBuff(BuffSystemBase buffBase)
        {
            Type type = buffBase.GetType();
            if (this.MBuffBases.ContainsKey(type))
            {
                MBuffBases[type].Enqueue(buffBase);
            }
            else
            {
                Queue<BuffSystemBase> temp = new Queue<BuffSystemBase>();
                temp.Enqueue(buffBase);
                this.MBuffBases.Add(type, temp);
            }
        }
    }
}