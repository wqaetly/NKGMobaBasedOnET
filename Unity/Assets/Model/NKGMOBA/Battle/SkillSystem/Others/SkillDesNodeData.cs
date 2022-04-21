//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月17日 20:59:41
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [BsonIgnoreExtraElements]
    [GUIColor(0.4f, 0.8f, 1)]
    public class SkillDesNodeData: BuffNodeDataBase
    {
        [TabGroup("基础信息")]
        [LabelText("技能名称")]
        public string SkillName;

        [TabGroup("基础信息")]
        [HideLabel]
        [Title("技能描述", Bold = false)]
        [MultiLineProperty(10)]
        public string SkillDescribe;

        [TabGroup("基础信息")]
        [Title("技能资源AB名,第一个是图标")]
        public List<string> SkillABInfo;
        
        [TabGroup("基础信息")]
        [HideLabel]
        [Title("技能消耗类型")]
        public SkillCostTypes SkillCostTypes = SkillCostTypes.None;
        
        [TabGroup("基础信息")]
        [HideLabel]
        [Title("技能CD", Bold = false)]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> SkillCD;
        
        [TabGroup("基础信息")]
        [HideLabel]
        [Title("技能消耗", Bold = false)]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> SkillCost;

        [TabGroup("基础信息")]
        [Title("技能类型", Bold = false)]
        [HideLabel]
        public SkillTypes SkillTypes = SkillTypes.NoBreak;

        [TabGroup("基础信息")]
        [Title("技能指示器类型", Bold = false)]
        [HideLabel]
        [HideIf("SkillTypes", SkillTypes.Passive)]
        public SkillReleaseMode SkillReleaseMode;

        [TabGroup("基础信息")]
        [Title("伤害类型", Bold = false)]
        [HideLabel]
        public BuffDamageTypes BuffDamageTypes;
    }
}