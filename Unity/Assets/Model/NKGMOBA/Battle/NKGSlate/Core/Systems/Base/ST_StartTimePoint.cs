//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年7月26日 23:20:19
//------------------------------------------------------------

namespace NKGSlate.Runtime
{
    public class ST_StartTimePoint : ST_ITimePointer
    {
        public bool HasTrigger { get; set; }
        public ST_IDirectable GetTarget { get; }

        public uint TriggerFrame
        {
            get => GetTarget.StartFrame;
            set => GetTarget.StartFrame = value;
        }

        public ST_StartTimePoint(ST_IDirectable target)
        {
            GetTarget = target;
        }

        public void TriggerForward(uint currentFrame, uint previousFrame)
        {
            if (currentFrame >= TriggerFrame)
            {
                if (HasTrigger) return;
                HasTrigger = true;
                GetTarget.Enter(currentFrame);
            }
        }

        public void Update(uint currentFrame, uint previousFrame)
        {
            if (currentFrame >= TriggerFrame && currentFrame <= GetTarget.EndFrame)
                GetTarget.Update(currentFrame, previousFrame);
        }
    }
}