//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月1日 15:23:58
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    public class SustainDamageBuffData: BuffDataBase
    {
        [LabelText("伤害类型")]
        public BuffDamageTypes BuffDamageTypes;
        
        [LabelText("预伤害修正")]
        public float damageFix = 1.0f;

        [LabelText("作用间隔")]
        public long WorkInternal = 0;
    }
}