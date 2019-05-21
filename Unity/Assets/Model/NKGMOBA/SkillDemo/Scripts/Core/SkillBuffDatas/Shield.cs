//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 14:53:57
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using SkillDemo;
using Sirenix.OdinInspector;

namespace SkillDemo
{
    /// <summary>
    /// 护盾值
    /// </summary>
    public class Shield: SkillBuffBase
    {
        [HideLabel]
        [Title("护盾变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "护盾变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_Shield = new Dictionary<int, float>();
    }
}