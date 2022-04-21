using System;

namespace ET
{
    public interface ISkillSystemEvent : IReference
    {
        void Handle();
        void Handle(object a);
        void Handle(object a, object b);
        void Handle(object a, object b, object c);
        void Handle(object a, object b, object c, object d);
    }

    public abstract class ASkillSystemEvent : ISkillSystemEvent
    {
        public void Handle()
        {
            this.Run();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c, object d)
        {
            throw new NotImplementedException();
        }

        public abstract void Run();

        public void Clear()
        {
        }
    }

    public abstract class ASkillSystemEvent<A> : ISkillSystemEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            this.Run((A) a);
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c, object d)
        {
            throw new NotImplementedException();
        }

        public abstract void Run(A a);
        public void Clear()
        {
            
        }
    }

    public abstract class ASkillSystemEvent<A, B> : ISkillSystemEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            this.Run((A) a, (B) b);
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c, object d)
        {
            throw new NotImplementedException();
        }

        public abstract void Run(A a, B b);
        public void Clear()
        {
            
        }
    }

    public abstract class ASkillSystemEvent<A, B, C> : ISkillSystemEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            this.Run((A) a, (B) b, (C) c);
        }

        public void Handle(object a, object b, object c, object d)
        {
            throw new NotImplementedException();
        }

        public abstract void Run(A a, B b, C c);
        public void Clear()
        {
            
        }
    }

    public abstract class ASkillSystemEvent<A, B, C, D> : ISkillSystemEvent
    {
        public void Handle()
        {
            throw new NotImplementedException();
        }

        public void Handle(object a)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c)
        {
            throw new NotImplementedException();
        }

        public void Handle(object a, object b, object c, object d)
        {
            this.Run((A) a, (B) b, (C) c, (D) d);
        }

        public abstract void Run(A a, B b, C c, D d);
        public void Clear()
        {
            
        }
    }
}