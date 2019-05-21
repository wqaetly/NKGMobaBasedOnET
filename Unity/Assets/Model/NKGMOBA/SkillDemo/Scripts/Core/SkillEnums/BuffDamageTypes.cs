using Sirenix.OdinInspector;

namespace SkillDemo
{
    [System.Flags]
    public enum BuffDamageTypes
    {
        /// <summary>
        /// 无伤害
        /// </summary>
        None = 1 << 1,
        [LabelText("单体伤害")]
        Single = 1 << 2,
        [LabelText("群体技能")]
        Range = 1 << 3,
        [LabelText("持续伤害")]
        Sustain = 1 << 4,

        [LabelText("物理伤害")]
        Physical = 1 << 5,
        [LabelText("魔法伤害")]
        Magic = 1 << 6,
        [LabelText("真实伤害")]
        Real = 1 << 7,

        /// <summary>
        /// 物理伤害
        /// </summary>
        [LabelText("物理单体伤害")]
        PhysicalSingle = Physical | Single,
        [LabelText("物理单体持续伤害")]
        PhysicalSingle_Sustain = PhysicalSingle | Sustain,
        [LabelText("物理群体伤害")]
        PhysicalRange = Physical | Range,
        [LabelText("物理群体持续伤害")]
        PhysicalRange_Sustain = PhysicalRange | Sustain,

        /// <summary>
        /// 法术伤害
        /// </summary>
        [LabelText("魔法单体伤害")]
        MagicSingle = Magic | Single,
        [LabelText("魔法单体持续伤害")]
        MagicSingle_Sustain = MagicSingle | Sustain,
        [LabelText("魔法群体伤害")]
        MagicRange = Magic | Range,
        [LabelText("魔法群体持续伤害")]
        MagicRange_Sustain = MagicRange | Sustain,

        /// <summary>
        /// 真实伤害
        /// </summary>
        [LabelText("真实单体伤害")]
        RealSingle = Real | Single,
        [LabelText("真实单体持续伤害")]
        RealSingle_Sustain = RealSingle | Sustain,
        [LabelText("真实群体伤害")]
        RealRange = Real | Range,
        [LabelText("真实群体持续伤害")]
        RealRange_Sustain = RealRange | Sustain,
    }
}