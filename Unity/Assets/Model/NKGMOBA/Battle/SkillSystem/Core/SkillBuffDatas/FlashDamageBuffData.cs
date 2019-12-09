//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月18日 16:38:28
//------------------------------------------------------------

using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class FlashDamageBuffData: BuffDataBase
    {
        [LabelText("伤害类型")]
        public BuffDamageTypes BuffDamageTypes;

        [LabelText("预伤害修正")]
        public float damageFix = 1.0f;
    }
}