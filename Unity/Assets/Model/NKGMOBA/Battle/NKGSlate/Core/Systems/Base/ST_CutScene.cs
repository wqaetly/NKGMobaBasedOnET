//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年7月26日 23:23:34
//------------------------------------------------------------

using System.Collections.Generic;

namespace NKGSlate.Runtime
{
    public class ST_CutScene:ST_IDirectable
    {
        public ST_DirectableData DirectableData { get; set; }
        public uint StartFrame { get; set; }
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

        public void Enter(uint currentFrame)
        {

        }

        public void Update(uint currentFrame, uint previousFrame)
        {

        }

        public void Exit()
        {

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