//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月16日 12:09:21
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 延时Buff，多用于处理叠加类型的技能，最经典的，诺克五层血怒
    /// </summary>
    public class DelayBuff : SkillBuffDataBase
    {
        [LabelText("最大叠加数")] public int MaxOverlay;
    }
}