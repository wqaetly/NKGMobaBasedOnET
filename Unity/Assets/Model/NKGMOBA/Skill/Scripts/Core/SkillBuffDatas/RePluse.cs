//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月16日 20:27:39
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 击退BUFF
    /// </summary>
    public class RePluse : SkillBuffBase
    {
        [HideLabel]
        [Title("击退距离变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "击退距离变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_RePluseDistance = new Dictionary<int, float>();
    }
}