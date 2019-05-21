//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月17日 21:10:47
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace SkillDemo
{
    public class NodeDataForCheckSkill : BaseNodeData
    {
        [HideLabel] [Title("技能消耗类型")] public SkillCostTypes SkillCostTypes = SkillCostTypes.None;

        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "消耗值")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        [HideLabel]
        [Title("技能消耗具体数据")]
        public Dictionary<int, float> m_SkillRequestCost;
    }
}