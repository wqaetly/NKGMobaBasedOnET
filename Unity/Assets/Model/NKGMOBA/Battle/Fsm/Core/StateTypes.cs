//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月12日 18:46:34
//------------------------------------------------------------

namespace ETModel.NKGMOBA.Battle.State
{
    public enum StateTypes: byte
    {
        /// <summary>
        /// 行走
        /// </summary>
        Run,

        /// <summary>
        /// 空闲
        /// </summary>
        Idle,
        
        //为多段技能动画预留的标识位
        Q_Spell,
        Q_Spell_1,
        Q_Spell_2,

        W_Spell,
        W_Spell_1,
        W_Spell_2,

        E_Spell,
        E_Spell_1,
        E_Spell_2,

        R_Spell,
        R_Spell_1,
        R_Spell_2,

        /// <summary>
        /// 普攻
        /// </summary>
        CommonAttack,

        /// <summary>
        /// 击退
        /// </summary>
        RePluse,

        /// <summary>
        /// 沉默
        /// </summary>
        Silence,

        /// <summary>
        /// 眩晕
        /// </summary>
        Dizziness,

        /// <summary>
        /// 击飞
        /// </summary>
        Striketofly,

        /// <summary>
        /// 嘲讽
        /// </summary>
        Sneer,

        /// <summary>
        /// 无敌
        /// </summary>
        Invincible,

        /// <summary>
        /// 禁锢
        /// </summary>
        Shackle,

        /// <summary>
        /// 隐身
        /// </summary>
        Invisible,

        /// <summary>
        /// 恐惧
        /// </summary>
        Fear,

        /// <summary>
        /// 致盲
        /// </summary>
        Blind,
    }
}