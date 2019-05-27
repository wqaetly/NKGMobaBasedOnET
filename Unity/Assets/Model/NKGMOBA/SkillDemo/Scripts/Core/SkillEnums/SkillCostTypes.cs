//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月17日 18:24:47
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    [System.Flags]
    public enum SkillCostTypes
    {
        /// <summary>
        /// 耗魔
        /// </summary>
        [LabelText("耗魔")] MagicValue = 1 << 1,

        /// <summary>
        /// 耗血
        /// </summary>
        [LabelText("耗血")]HPValue = 1 << 2,

        /// <summary>
        /// 其他
        /// </summary>
        [LabelText("其他")]Other = 1 << 3,

        /// <summary>
        /// 耗时
        /// </summary>
        [LabelText("耗时")] Time = 1 << 4,

        /// <summary>
        /// 无消耗
        /// </summary>
        [LabelText("无消耗")]None = 1 << 5,
    }
}