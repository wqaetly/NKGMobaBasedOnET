//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 14:51:31
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using ETModel;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 攻速
    /// </summary>
    public class AttackSpeed : SkillBuffBase
    {
        
        [HideLabel] [Title("攻速变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "攻速变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_AttackSpeed = new Dictionary<int, float>();
    }
}