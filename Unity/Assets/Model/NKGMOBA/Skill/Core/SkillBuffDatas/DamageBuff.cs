//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月3日 18:37:18
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 伤害Buff
    /// </summary>
    public class DamageBuff: SkillBuffBase
    {
        [LabelText("伤害类型")]
        public BuffDamageTypes BuffDamageTypes;
    }
}