//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月12日 18:46:34
//------------------------------------------------------------

using System;

namespace ETModel.NKGMOBA.Battle.State
{
    [Flags]
    public enum StateTypes: long
    {
        /// <summary>
        /// 行走
        /// </summary>
        Run = 1 << 1,

        /// <summary>
        /// 空闲
        /// </summary>
        Idle = 1 << 2,

        /// <summary>
        /// 释放技能
        /// </summary>
        Skill_Cast = 1 << 3,

        /// <summary>
        /// 普攻
        /// </summary>
        CommonAttack = 1 << 4,

        /// <summary>
        /// 击退
        /// </summary>
        RePluse = 1 << 5,

        /// <summary>
        /// 沉默
        /// </summary>
        Silence = 1 << 6,

        /// <summary>
        /// 眩晕
        /// </summary>
        Dizziness = 1 << 7,

        /// <summary>
        /// 击飞
        /// </summary>
        Striketofly = 1 << 8,

        /// <summary>
        /// 嘲讽
        /// </summary>
        Sneer = 1 << 9,

        /// <summary>
        /// 无敌
        /// </summary>
        Invincible = 1 << 10,

        /// <summary>
        /// 禁锢
        /// </summary>
        Shackle = 1 << 11,

        /// <summary>
        /// 隐身
        /// </summary>
        Invisible = 1 << 12,

        /// <summary>
        /// 恐惧
        /// </summary>
        Fear = 1 << 13,

        /// <summary>
        /// 致盲
        /// </summary>
        Blind = 1 << 14,

        /// <summary>
        /// 排斥普攻，有此状态无法普攻
        /// </summary>
        CommonAttackConflict = 1 << 15,

        /// <summary>
        /// 排斥行走，有此状态无法行走
        /// </summary>
        WalkConflict = 1 << 16,
    }
}