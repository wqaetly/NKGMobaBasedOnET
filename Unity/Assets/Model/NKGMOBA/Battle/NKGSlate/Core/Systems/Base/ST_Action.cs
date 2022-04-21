//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年7月25日 16:53:36
//------------------------------------------------------------

using System.Collections.Generic;

namespace NKGSlate.Runtime
{
    public class ST_Action<T> : ST_IDirectable where T : ST_DirectableData
    {
        public ST_DirectableData DirectableData { get; set; }

        public T BindingData => DirectableData as T;

        /// <summary>
        /// 起始帧，为运行时计算的帧数
        /// </summary>
        public uint StartFrame { get; set; }

        /// <summary>
        /// 结束帧，为运行时计算的帧数
        /// </summary>
        public uint EndFrame { get; set; }

        public bool Initialize(uint currentFrame, ST_DirectableData stDirectableData)
        {
            this.DirectableData = stDirectableData;
            StartFrame =
                currentFrame + ST_TimeToFrameCaculator.CaculateFrameCountFromTimeLength(this.DirectableData.RelativelyStartTime);
            EndFrame =
                currentFrame + ST_TimeToFrameCaculator.CaculateFrameCountFromTimeLength(this.DirectableData.RelativelyEndTime);
            return OnInitialize(currentFrame);
        }

        void ST_IDirectable.Enter(uint currentFrame)
        {
            OnEnter(currentFrame);
        }

        void ST_IDirectable.Update(uint currentFrame, uint previousFrame)
        {
            OnUpdate(currentFrame, previousFrame);
        }

        void ST_IDirectable.Exit()
        {
            OnExit();
        }

        public virtual bool OnInitialize(uint currentFrame)
        {
            return true;
        }

        public virtual void OnEnter(uint currentFrame)
        {
        }

        public virtual void OnUpdate(uint currentFrame, uint previousFrame)
        {
        }

        public virtual void OnExit()
        {
        }
    }
}