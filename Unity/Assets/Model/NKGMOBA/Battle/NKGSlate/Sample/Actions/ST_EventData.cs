//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月17日 22:33:00
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace NKGSlate.Runtime
{
    [HideLabel]
    [HideReferenceObjectPicker]
    public class ST_EventData: ST_ActionData
    {
        [LabelText("将要发送的的事件")]
        [BoxGroup("自定义数据")]
        public string EventInfo;
    }
}