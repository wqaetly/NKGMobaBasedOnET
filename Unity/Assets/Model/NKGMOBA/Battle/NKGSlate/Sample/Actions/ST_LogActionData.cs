//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月12日 21:40:03
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace NKGSlate.Runtime
{
    [HideLabel]
    [HideReferenceObjectPicker]
    public class ST_LogActionData : ST_ActionData
    {
        [LabelText("将要打印的信息")]
        [BoxGroup("自定义数据")]
        public string LogInfo;
    }
}