//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月15日 15:01:23
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;
using SkillDemo;

namespace SkillDemo
{
    /// <summary>
    /// 增加英雄移速
    /// </summary>
    public class InHeroSpeed : SkillBuffBase
    {
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "增加移速")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        [HideLabel]
        [Title("增加移速")]
        public Dictionary<int, float> m_DeSpeedValue = new Dictionary<int, float>();

        [HideLabel]
        [Title("增加移速时长")]
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "增加移速时长")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> m_WorkTime = new Dictionary<int, float>();
    }
}