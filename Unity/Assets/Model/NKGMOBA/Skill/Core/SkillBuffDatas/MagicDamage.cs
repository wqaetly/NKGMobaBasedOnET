//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 14:52:53
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using SkillDemo;
using Sirenix.OdinInspector;

namespace SkillDemo
{
    /// <summary>
    /// 法伤
    /// </summary>
    public class MagicDamage: SkillBuffBase
    {
        [LabelText("伤害类型")]
        public BuffDamageTypes m_BuffTypes;
        
        [LabelText("加成值")]
        public float MagicAddition;
        
        [HideLabel]
        [Title("法伤变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "法伤变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_MagicDamage = new Dictionary<int, float>();
        
       // TODO：照理来说加成系数都是不变的，当然了，如果策划的需求够变态，那就用字典的形式
       // [DictionaryDrawerSettings(KeyLabel = "技能等级", ValueLabel = "法强加成")]
       // public Dictionary<int, float> m_MagicDamageAddition = new Dictionary<int, float>();
    }
}