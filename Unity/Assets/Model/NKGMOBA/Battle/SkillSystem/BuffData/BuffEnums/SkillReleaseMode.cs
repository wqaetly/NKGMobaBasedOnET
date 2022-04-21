//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 17:15:17
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ET
{
    public enum SkillReleaseMode
    {
        /// <summary>
        /// 无
        /// </summary>
        [LabelText("无")] None = 0,

        /// <summary>
        /// 圆环指示器
        /// </summary>
        [LabelText("圆环指示器")] ARange = 1,

        /// <summary>
        /// 箭头指示器
        /// </summary>
        [LabelText("箭头指示器")] AArrow = 2,

        /// <summary>
        /// 使用鼠标选择
        /// </summary>
        [LabelText("使用鼠标选择")] ATarget = 3,

        /// <summary>
        /// 使用鼠标选择
        /// </summary>
        [LabelText("扇形指示器")] Sector = 4,
    }
}