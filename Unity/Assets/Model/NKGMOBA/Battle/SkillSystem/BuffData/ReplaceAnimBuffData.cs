//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月24日 21:23:44
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET
{
    [LabelText("动画信息映射")]
    public class AnimMapInfo
    {
        [LabelText("StateType名称")]
        public string StateType;
        [LabelText("动画名称")]
        public string AnimName;
    }
    
    public class ReplaceAnimBuffData: BuffDataBase
    {
        [BoxGroup("自定义项")]
        [LabelText("替换动画")]
        public List<AnimMapInfo> AnimReplaceInfo = new List<AnimMapInfo>();
    }
}