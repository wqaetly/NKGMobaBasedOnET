//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 17:15:17
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace SkillDemo
{
    public enum SkillReleaseMode
    {
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
        [LabelText("使用鼠标选择")]  ATarget = 3,
        
        /// <summary>
        /// 无，以自身为中心的技能 ，如皇子W
        /// </summary>
        [LabelText("无，以自身为中心的技能 ，如皇子W")] None = 4,
    }
}