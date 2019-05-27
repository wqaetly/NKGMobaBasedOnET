//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月16日 13:06:06
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// Buff的奏效的表现
    /// </summary>
    [System.Flags]
    public enum BuffWorkTypes
    {
        /// <summary>
        /// 无
        /// </summary>
        [LabelText("无")] None = 1,

        /// <summary>
        /// 击退
        /// </summary>
        [LabelText("击退")] RePluse = 1 << 1,

        /// <summary>
        /// 沉默
        /// </summary>
        [LabelText("沉默")] Silence = 1 << 2,

        /// <summary>
        /// 眩晕
        /// </summary>
        [LabelText("眩晕")] Dizziness = 1 << 3,

        /// <summary>
        /// 击飞
        /// </summary>
        [LabelText("击飞")] Striketofly = 1 << 4,

        /// <summary>
        /// 暴击
        /// </summary>
        [LabelText("暴击")] CriticalStrike = 1 << 6,

        /// <summary>
        /// 治疗
        /// </summary>
        [LabelText("治疗")] Treatment = 1 << 7,

        /// <summary>
        /// 改变移动速度
        /// </summary>
        [LabelText("改变移动速度")] ChangeSpeed = 1 << 8,

        /// <summary>
        /// 嘲讽
        /// </summary>
        [LabelText("嘲讽")] Sneer = 1 << 10,

        /// <summary>
        /// 无敌
        /// </summary>
        [LabelText("无敌")] Invincible = 1 << 11,

        /// <summary>
        /// 禁锢
        /// </summary>
        [LabelText("禁锢")] Shackle = 1 << 12,

        /// <summary>
        /// 隐身
        /// </summary>
        [LabelText("隐身")] Invisible = 1 << 13,

        /// <summary>
        /// 斩杀
        /// </summary>
        [LabelText("斩杀")] Kill = 1 << 14,

        /// <summary>
        /// 改变攻击距离
        /// </summary>
        [LabelText("改变攻击距离")] ChangeAttackRang = 1 << 15,

        /// <summary>
        /// 改变攻击速度
        /// </summary>
        [LabelText("改变攻击速度")] ChangeAttackSpeed = 1 << 16,

        /// <summary>
        /// 恐惧
        /// </summary>
        [LabelText("恐惧")] Fear = 1 << 17,

        /// <summary>
        /// 致盲
        /// </summary>
        [LabelText("致盲")] Blind = 1 << 18,

        /// <summary>
        /// 伤害
        /// </summary>
        [LabelText("造成伤害")] Damage = 1 << 19
    }
}