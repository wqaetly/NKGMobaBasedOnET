using ETModel;

namespace NPBehave
{
    public class Service : Decorator
    {
        private System.Action serviceMethod;

        private float interval = -1.0f;
        private float randomVariation;

        public Service(float interval, float randomVariation, System.Action service, Node decoratee) : base("Service", decoratee)
        {
            this.serviceMethod = service;
            this.interval = interval;
            this.randomVariation = randomVariation;

            this.Label = "" + (interval - randomVariation) + "..." + (interval + randomVariation) + "s";
        }

        public Service(float interval, System.Action service, Node decoratee) : base("Service", decoratee)
        {
            this.serviceMethod = service;
            this.interval = interval;
            this.randomVariation = -1;
            this.Label = "" + (interval - randomVariation) + "..." + (interval + randomVariation) + "s";
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
                this.Clock.AddUpdateObserver(serviceMethod);
                serviceMethod();
            }
            else if (randomVariation <= 0f)
            {
                this.Clock.AddTimer(this.interval, -1, serviceMethod);
                serviceMethod();
                Log.Info("注册Timer--------------");
            }
            else
            {
                InvokeServiceMethodWithRandomVariation();
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
                this.Clock.RemoveUpdateObserver(serviceMethod);
            }
            else if (randomVariation <= 0f)
            {
                this.Clock.RemoveTimer(serviceMethod);
                Log.Info("移除Timer----------------");
            }
            else
            {
                this.Clock.RemoveTimer(InvokeServiceMethodWithRandomVariation);
            }
            Stopped(result);
        }

        private void InvokeServiceMethodWithRandomVariation()
        {
            serviceMethod();
            this.Clock.AddTimer(interval, randomVariation, 0, InvokeServiceMethodWithRandomVariation);
        }
    }
}