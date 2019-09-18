//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月18日 17:01:41
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// Buff携带伤害buff，buff百分比加成
    /// </summary>
    public class DelayBuff_WithDamage: SkillBuffDataBase
    {
        [LabelText("最大叠加数")]
        public int MaxOverlay;

        public DamgeBuff_Sustain DamgeBuffSustain;
    }
}