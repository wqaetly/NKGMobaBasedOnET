//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月12日 21:33:22
//------------------------------------------------------------

using UnityEngine;

namespace NKGSlate.Runtime
{
    public class ST_LogAction: ST_Action<ST_LogActionData>
    {
        public override bool OnInitialize(uint currentFrame)
        {
            Debug.Log($"ST_LogInfo Initialize");
            return true;
        }

        public override void OnEnter(uint currentFrame)
        {
            base.OnEnter(currentFrame);
            
            Debug.Log($"ST_LogInfo Enter");
        }

        public override void OnUpdate(uint currentFrame, uint previousFrame)
        {
            Debug.Log($"ST_LogInfo: {this.BindingData.LogInfo} 帧数：{currentFrame}");
        }

        public override void OnExit()
        {
            base.OnExit();
            
            Debug.Log($"ST_LogInfo Exit");
        }
    }
}