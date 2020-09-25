//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 12:24:00
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    public class TreatmentBuffData: BuffDataBase
    {
        [BoxGroup("自定义项")]
        [LabelText("预治疗修正")]
        public float TreatmentFix = 1.0f;
    }
}