//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月16日 12:09:21
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace SkillDemo
{
    /// <summary>
    /// 延时Buff，多用于处理叠加类型的技能，最经典的，诺克五层血怒
    /// </summary>
    public class DelayBuff : SkillBuffBase
    {
        [LabelText("最大叠加数")] public int MaxOverlay;

        [HideLabel]
        [Title("每层叠加属性")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "每层叠加属性")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_GuideTime = new Dictionary<int, float>();

        [HideLabel] [Title("叠加完成或中断要做的事")] public List<SkillBuffBase> CompleteOrBreak;
    }
}