//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月17日 21:10:47
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NodeDataForCheckSkill: SkillBaseNodeData
    {
        [HideLabel]
        [Title("技能消耗类型")]
        public SkillCostTypes SkillCostTypes = SkillCostTypes.None;


    }
}