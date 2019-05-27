//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 14:52:26
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using ETModel;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 物伤
    /// </summary>
    public class PhysicalDamage : SkillBuffBase
    {
        [LabelText("伤害类型")] public BuffDamageTypes m_BuffTypes;

        [LabelText("加成值")] public float MagicAddition;

        [HideLabel]
        [Title("物伤变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "物伤变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_PhysicalDamage = new Dictionary<int, float>();
    }
}