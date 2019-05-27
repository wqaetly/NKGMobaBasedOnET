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
#if !SERVER
        [LabelText("耗魔")]
        #endif
        MagicValue = 1 << 1,
#if !SERVER
        [LabelText("耗血")]
#endif
        HPValue = 1 << 2,

        [LabelText("其他")]
        Other = 1 << 3,

        [LabelText("耗时")]
        Time = 1 << 4,

        [LabelText("无消耗")]
        None = 1 << 5,
    }
}