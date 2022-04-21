namespace NKGSlate.Runtime
{
    public interface ST_ITimePointer
    {
        uint TriggerFrame { get; set; }
        
        /// <summary>
        /// 是否已被触发
        /// </summary>
        bool HasTrigger { get; set; }
        
        ST_IDirectable GetTarget { get; }

        /// <summary>
        /// 往前方向（右）触发
        /// </summary>
        /// <param name="currentFrame"></param>
        /// <param name="previousFrame"></param>
        void TriggerForward(uint currentFrame, uint previousFrame);
    }
}