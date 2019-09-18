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
    /// Buff池组件
    /// </summary>
    public class BuffPoolComponent: Component
    {
        public Dictionary<Type, Queue<BuffBase>> MBuffBases = new Dictionary<Type, Queue<BuffBase>>();

        public T Acquire<T>() where T : BuffBase
        {
            Queue<BuffBase> buffBase;
            if (this.MBuffBases.TryGetValue(typeof (T), out buffBase))
            {
                if (buffBase.Count > 0)
                {
                    return (T) buffBase.Dequeue();
                }
            }

            return (T) Activator.CreateInstance(typeof (T));
        }

        public void Recycle(BuffBase buffBase)
        {
            Type type = buffBase.GetType();
            if (this.MBuffBases.ContainsKey(type))
            {
                MBuffBases[type].Enqueue(buffBase);
            }
            else
            {
                Queue<BuffBase> temp = new Queue<BuffBase>();
                temp.Enqueue(buffBase);
                this.MBuffBases.Add(type, temp);
            }
        }
    }
}