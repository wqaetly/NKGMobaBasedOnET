//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 14:53:29
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using ETModel;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 血量变化
    /// </summary>
    public class Hp: SkillBuffBase
    {
        [HideLabel]
        [Title("血量变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "血量变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_Hp = new Dictionary<int, float>();
    }
}