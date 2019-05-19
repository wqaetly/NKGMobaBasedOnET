using Sirenix.OdinInspector;

namespace SkillDemo
{
    [System.Flags]
    public enum SkillTypes
    {
        /// <summary>
        /// 主动技能
        /// </summary>
        [LabelText("主动技能")] Active = 1 << 1,

        /// <summary>
        /// 被动技能
        /// </summary>
        [LabelText("被动技能")] Passive = 1 << 2,

        /// <summary>
        /// 可被打断
        /// </summary>
        [LabelText("可被打断")] CanBreak = 1 << 3,

        /// <summary>
        /// 不可被打断
        /// </summary>
        [LabelText("不可被打断")] NoBreak = 1 << 4,

        /// <summary>
        /// 有主动有被动
        /// </summary>
        [LabelText("有主动有被动")] ActiveAndPassive = Active | Passive
    }
}