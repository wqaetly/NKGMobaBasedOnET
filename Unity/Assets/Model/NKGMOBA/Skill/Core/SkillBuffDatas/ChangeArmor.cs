//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月17日 18:36:32
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace SkillDemo
{
    /// <summary>
    /// 改变护甲
    /// </summary>
    public class ChangeArmor:SkillBuffBase
    {
        [HideLabel] [Title("护甲变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "护甲变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_ChangeArmor = new Dictionary<int, float>();
    }
}