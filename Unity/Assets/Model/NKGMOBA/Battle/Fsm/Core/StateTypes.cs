//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月12日 18:46:34
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;

namespace ETModel.NKGMOBA.Battle.State
{
    [Flags]
    public enum StateTypes
    {
        /// <summary>
        /// 行走
        /// </summary>
        [LabelText("行走")]
        Run = 1 << 1,

        /// <summary>
        /// 空闲
        /// </summary>
        [LabelText("空闲")]
        Idle = 1 << 2,

        /// <summary>
        /// 释放技能
        /// </summary>
        [LabelText("释放技能")]
        Skill_Cast = 1 << 3,

        /// <summary>
        /// 普攻
        /// </summary>
        [LabelText("普攻")]
        CommonAttack = 1 << 4,

        /// <summary>
        /// 击退
        /// </summary>
        [LabelText("击退")]
        RePluse = 1 << 5,

        /// <summary>
        /// 沉默
        /// </summary>
        [LabelText("沉默")]
        Silence = 1 << 6,

        /// <summary>
        /// 眩晕
        /// </summary>
        [LabelText("眩晕")]
        Dizziness = 1 << 7,

        /// <summary>
        /// 击飞
        /// </summary>
        [LabelText("击飞")]
        Striketofly = 1 << 8,

        /// <summary>
        /// 嘲讽
        /// </summary>
        [LabelText("嘲讽")]
        Sneer = 1 << 9,

        /// <summary>
        /// 无敌
        /// </summary>
        [LabelText("无敌")]
        Invincible = 1 << 10,

        /// <summary>
        /// 禁锢
        /// </summary>
        [LabelText("禁锢")]
        Shackle = 1 << 11,

        /// <summary>
        /// 隐身
        /// </summary>
        [LabelText("隐身")]
        Invisible = 1 << 12,

        /// <summary>
        /// 恐惧
        /// </summary>
        [LabelText("恐惧")]
        Fear = 1 << 13,

        /// <summary>
        /// 致盲
        /// </summary>
        [LabelText("致盲")]
        Blind = 1 << 14,

        /// <summary>
        /// 排斥普攻，有此状态无法普攻
        /// </summary>
        [LabelText("排斥普攻")]
        CommonAttackConflict = 1 << 15,

        /// <summary>
        /// 排斥行走，有此状态无法行走
        /// </summary>
        [LabelText("排斥行走")]
        WalkConflict = 1 << 16,
        
        /// <summary>
        /// 排斥释放技能，有此状态无法释放技能
        /// </summary>
        [LabelText("排斥技能释放")]
        CastSkillConflict = 1<<17,
    }
}