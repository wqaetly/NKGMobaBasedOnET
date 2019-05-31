//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 14:55:29
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using ETModel;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 技能作用次数
    /// </summary>
    public class WorkTime : SkillBuffBase
    {
        [HideLabel]
        [Title("技能作用次数变化")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "技能作用次数变化")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_WorkTime = new Dictionary<int, float>();
    }
}