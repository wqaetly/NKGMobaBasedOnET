//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月3日 17:49:23
//------------------------------------------------------------

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class LogAction: Action
    {
        public TaskStatus m_TaskState;
        
        public override TaskStatus OnTick()
        {
            Debug.Log("????????");
            return m_TaskState;
        }
    }
}