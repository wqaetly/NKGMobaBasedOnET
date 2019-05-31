//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 15:00:33
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using ETModel;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 减少移速
    /// </summary>
    public class DeHeroSpeed : SkillBuffBase
    {
        [HideLabel]
        [Title("减少敌人移速")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "减少敌人移速")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_DeSpeedValue = new Dictionary<int, float>();

        [HideLabel]
        [Title("减少敌人移速时长")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "减少敌人移速时长")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_WorkTime = new Dictionary<int, float>();
    }
}