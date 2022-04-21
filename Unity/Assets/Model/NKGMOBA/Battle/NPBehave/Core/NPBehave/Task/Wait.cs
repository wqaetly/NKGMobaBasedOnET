using System.Diagnostics;
using ET;

namespace NPBehave
{
    public class Wait : Task
    {
        private System.Func<float> function = null;
        private string blackboardKey = null;
        private float seconds = -1f;
        private long TimerId;

        public Wait(string blackboardKey) : base("Wait")
        {
            this.blackboardKey = blackboardKey;
        }

        protected override void DoStart()
        {
            float seconds = this.seconds;
            if (seconds < 0)
            {
                if (this.blackboardKey != null)
                {
                    seconds = Blackboard.Get<float>(this.blackboardKey);
                }
                else if (this.function != null)
                {
                    seconds = this.function();
                }
            }

            if (seconds < 0)
            {
                seconds = 0;
            }

            TimerId = Clock.AddTimer((uint)TimeAndFrameConverter.Frame_Float2Frame(seconds), onTimer);
        }

        protected override void DoCancel()
        {
            Clock.RemoveTimer(TimerId);
            this.Stopped(false);
        }

        private void onTimer()
        {
            this.Stopped(true);
        }
    }
}