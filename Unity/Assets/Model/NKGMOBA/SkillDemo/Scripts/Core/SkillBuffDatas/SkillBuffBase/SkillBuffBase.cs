//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月14日 22:44:27
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace SkillDemo
{
    [GUIColor(0.4f, 0.8f, 1)]
    public abstract class SkillBuffBase
    {
        [Title("Buff基本信息")] [LabelText("Buff是否状态栏可见")]
        public bool Base_isVisualable;

        [LabelText("Buff效果为")] public BuffWorkTypes Base_BuffExtraWork;

        [LabelText("Buff持续时间,-1代表永久,0代表此处设置无效")]
        public float Base_buffSustainTime;

        [LabelText("Buff面板数值依赖者")] public BuffEffectedTypes Base_BuffEffectedTypes;

        [LabelText("Buff加成方式")] public BuffTypes Base_BuffTypes;
    }
}