//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年7月25日 16:53:13
//------------------------------------------------------------

using System.Collections.Generic;

namespace NKGSlate.Runtime
{
    public interface ST_IDirectable
    {
        ST_DirectableData DirectableData { get; set; }
        uint StartFrame { get; set; }
        uint EndFrame { get; set; }

        bool Initialize(uint currentFrame, ST_DirectableData stDirectableData);
        void Enter(uint currentFrame);
        void Update(uint currentFrame, uint previousFrame);
        void Exit();
    }
}