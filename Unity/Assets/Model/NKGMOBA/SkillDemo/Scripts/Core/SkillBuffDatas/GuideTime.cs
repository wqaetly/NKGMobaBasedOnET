//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 14:55:15
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using ETModel;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 引导时长
    /// </summary>
    public class GuideTime: SkillBuffBase
    {
        [HideLabel]
        [Title("引导时长变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "引导时长变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_GuideTime = new Dictionary<int, float>();
    }
}