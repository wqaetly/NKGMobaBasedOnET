//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月19日 20:21:18
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    public class ChangeSpeedBuffData: BuffDataBase
    {
        [LabelText("伤害类型")]
        public BuffDamageTypes BuffDamageTypes;

        [LabelText("是否为持续伤害")]
        public bool isSustainDamage = false;

        [ShowIf("isSustainDamage")]
        [LabelText("持续时间")]
        public float SustainTime = 0;

        [ShowIf("isSustainDamage")]
        [LabelText("作用间隔")]
        public float WorkInternal = 0;

        [LabelText("预伤害修正")]
        public float damageFix = 1.0f;
    }
}