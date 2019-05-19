//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 14:53:08
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using SkillDemo;
using Sirenix.OdinInspector;

namespace SkillDemo
{
    /// <summary>
    /// 真实伤害
    /// </summary>
    public class RealDamage: SkillBuffBase
    {
        [HideLabel]
        [Title("真伤变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "真伤变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_RealDamage = new Dictionary<int, float>();
    }
}