//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年7月26日 23:20:30
//------------------------------------------------------------

namespace NKGSlate.Runtime
{
    public class ST_EndTimePoint : ST_ITimePointer
    {
        public bool HasTrigger { get; set; }

        public uint TriggerFrame
        {
            get => GetTarget.EndFrame;
            set => GetTarget.EndFrame = value;
        }

        public ST_IDirectable GetTarget { get; }

        public ST_EndTimePoint(ST_IDirectable target)
        {
            GetTarget = target;
        }

        public void TriggerForward(uint currentFrame, uint previousFrame)
        {
            if (currentFrame > TriggerFrame)
            {
                if (HasTrigger) return;
                HasTrigger = true;
                GetTarget.Exit();
            }
        }
    }
}