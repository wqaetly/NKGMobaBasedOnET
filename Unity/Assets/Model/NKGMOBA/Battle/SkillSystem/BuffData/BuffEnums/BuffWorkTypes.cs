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
        [LabelText("无")]
        None = 1,

        [LabelText("击退")]
        RePluse = 1 << 1,

        [LabelText("沉默")]
        Silence = 1 << 2,

        [LabelText("眩晕")]
        Dizziness = 1 << 3,

        [LabelText("击飞")]
        Striketofly = 1 << 4,

        [LabelText("暴击")]
        CriticalStrike = 1 << 6,

        [LabelText("治疗")]
        Treatment = 1 << 7,

        [LabelText("改变移动速度")]
        ChangeSpeed = 1 << 8,

        [LabelText("嘲讽")]
        Sneer = 1 << 10,

        [LabelText("无敌")]
        Invincible = 1 << 11,

        [LabelText("禁锢")]
        Shackle = 1 << 12,

        [LabelText("隐身")]
        Invisible = 1 << 13,

        [LabelText("斩杀")]
        Kill = 1 << 14,

        [LabelText("改变攻击距离")]
        ChangeAttackRang = 1 << 15,

        [LabelText("改变攻击速度")]
        ChangeAttackSpeed = 1 << 16,

        [LabelText("恐惧")]
        Fear = 1 << 17,

        [LabelText("致盲")]
        Blind = 1 << 18,

        [LabelText("造成伤害")]
        Damage = 1 << 19,

        [LabelText("改变攻击力")]
        ChangeAttackValue = 1 << 20,
        
        [LabelText("改变法强值")]
        ChangeMagicAttackValue = 1 << 21,
        
        [LabelText("改变护盾")]
        ChangeSheidValue = 1 << 22,
        
        [LabelText("改变吟唱时长")]
        ChangeGuideTime = 1 << 23,
        
        [LabelText("改变技能CD")]
        ChangeSkillCD = 1 << 24,
        
        [LabelText("改变技能范围")]
        ChangeSkillRange = 1 << 25,
        
        [LabelText("改变蓝量")]
        ChangeMagic = 1 << 26,
        
        [LabelText("改变HP")]
        ChangeHP = 1 << 27,
    }
}