//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月16日 11:18:02
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace SkillDemo
{
    public class AttackRange : SkillBuffBase
    {
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "攻击范围变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        [HideLabel] [Title("攻击范围变化")]
        public Dictionary<int, float> m_Range = new Dictionary<int, float>();
    }
}