using System;

namespace ET
{
    public interface IFixedUpdateSystem : ISystemType
    {
        void Run(object o);
    }

    [ObjectSystem]
    public abstract class FixedUpdateSystem<T> : IFixedUpdateSystem
    {
        public void Run(object o)
        {
            this.FixedUpdate((T) o);
        }

        public Type Type()
        {
            return typeof(T);
        }

        public Type SystemType()
        {
            return typeof(IFixedUpdateSystem);
        }

        public abstract void FixedUpdate(T self);
    }
}