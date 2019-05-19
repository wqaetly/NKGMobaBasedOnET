//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 14:54:35
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using SkillDemo;
using Sirenix.OdinInspector;

namespace SkillDemo
{
    /// <summary>
    /// 技能范围
    /// </summary>
    public class SkillRange: SkillBuffBase
    {

        [HideLabel]
        [Title("范围变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "范围变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_Range = new Dictionary<int, float>();
    }
}