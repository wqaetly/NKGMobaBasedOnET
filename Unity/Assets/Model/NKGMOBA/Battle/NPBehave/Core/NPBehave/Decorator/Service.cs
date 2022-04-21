using ET;

namespace NPBehave
{
    public class Service : Decorator
    {
        private System.Action serviceMethod;

        private float interval = -1.0f;

        private long TimerId;
        
        public Service(float interval, float randomVariation, System.Action service, Node decoratee) : base("Service",
            decoratee)
        {
            this.serviceMethod = service;
            this.interval = interval;

            this.Label = "" + (interval - randomVariation) + "..." + (interval + randomVariation) + "s";
        }

        public Service(float interval, System.Action service, Node decoratee) : base("Service", decoratee)
        {
            this.serviceMethod = service;
            this.interval = interval;
        }

        public Service(System.Action service, Node decoratee) : base("Service", decoratee)
        {
            this.serviceMethod = service;
            this.Label = "every tick";
        }

        protected override void DoStart()
        {
            if (this.interval <= 0f)
            {
                TimerId = this.Clock.AddTimer(1, serviceMethod, -1);
                serviceMethod();
            }
            else
            {
                TimerId = this.Clock.AddTimer((uint) TimeAndFrameConverter.Frame_Float2Frame(this.interval), serviceMethod, -1);
                serviceMethod();
            }

            Decoratee.Start();
        }

        override protected void DoCancel()
        {
            Decoratee.CancelWithoutReturnResult();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            if (this.interval <= 0f)
            {
                this.Clock.RemoveTimer(TimerId);
            }
            else
            {
                this.Clock.RemoveTimer(TimerId);
            }
            
            Stopped(result);
        }
    }
}