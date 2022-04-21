using System;
using ET;

namespace NPBehave
{
    public class Condition : ObservingDecorator
    {
        private Func<bool> condition;
        private float checkInterval;
        private long Timer;

        public Condition(Func<bool> condition, Node decoratee) : base("Condition", Stops.NONE, decoratee)
        {
            this.condition = condition;
            this.checkInterval = 0.0f;
        }

        public Condition(Func<bool> condition, Stops stopsOnChange, Node decoratee) : base("Condition", stopsOnChange,
            decoratee)
        {
            this.condition = condition;
            this.checkInterval = 0.0f;
        }

        public Condition(Func<bool> condition, Stops stopsOnChange, float checkInterval, Node decoratee) : base(
            "Condition", stopsOnChange, decoratee)
        {
            this.condition = condition;
            this.checkInterval = checkInterval;
        }

        override protected void StartObserving()
        {
            Timer = this.RootNode.Clock.AddTimer((uint) TimeAndFrameConverter.Frame_Float2Frame(checkInterval),
                Evaluate, -1);
        }

        override protected void StopObserving()
        {
            this.RootNode.Clock.RemoveTimer(Timer);
        }

        protected override bool IsConditionMet()
        {
            return this.condition();
        }
    }
}