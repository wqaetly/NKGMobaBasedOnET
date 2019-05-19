//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月16日 11:43:11
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace SkillDemo
{
    public class ReduceSkillCD:SkillBuffBase
    {
        [HideLabel]
        [Title("CD减少数值")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "CD减少数值")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_SkillCD = new Dictionary<int, float>();
    }
}