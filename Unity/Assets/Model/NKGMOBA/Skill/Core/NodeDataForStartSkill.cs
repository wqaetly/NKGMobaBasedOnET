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

namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class NodeDataForStartSkill: BaseNodeData
    {
#if !SERVER
        [TabGroup("基础信息")]
        [PreviewField(Height = 50)]
        [LabelText("技能图标")]
        [GUIColor(1, 0.6f, 0.4f)]
        [BsonIgnore]
        public Sprite SkillSprite;


#endif
        [TabGroup("基础信息")]
        [LabelText("技能名称")]
        [GUIColor(1, 0.6f, 0.4f)]
        public string SkillName;

        [TabGroup("基础信息")]
        [HideLabel]
        [Title("技能描述", Bold = false)]
        [MultiLineProperty(10)]
        [GUIColor(1, 0.6f, 0.4f)]
        public string SkillDescribe;

        [TabGroup("基础信息")]
        [Title("技能资源AB名,第一个是图标")]
        [GUIColor(1, 0.6f, 0.4f)]
        public List<string> SkillABInfo;
        
        [TabGroup("技能动画")]
        [HideLabel]
        public List<AnimationData> m_AnimationData;

        [TabGroup("基础信息")]
        [HideLabel]
        [Title("技能CD", Bold = false)]
        [GUIColor(0.4f, 0.8f, 1)]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> SkillCD;

        [TabGroup("基础信息")]
        [Title("技能类型", Bold = false)]
        [HideLabel]
        [GUIColor(1, 0.6f, 0.4f)]
        public SkillTypes SkillTypes = SkillTypes.NoBreak;

        [TabGroup("基础信息")]
        [Title("技能指示器类型", Bold = false)]
        [HideLabel]
        [GUIColor(1, 0.6f, 0.4f)]
        [HideIf("SkillTypes", SkillTypes.Passive)]
        public SkillReleaseMode SkillReleaseMode;

        [TabGroup("基础信息")]
        [Title("技能目标", Bold = false)]
        [HideLabel]
        [GUIColor(1, 0.6f, 0.4f)]
        [HideIf("SkillTypes", SkillTypes.Passive)]
        public SkillTarget SkillTarget;

        [TabGroup("基础信息")]
        [Title("伤害类型", Bold = false)]
        [HideLabel]
        [GUIColor(1, 0.6f, 0.4f)]
        public BuffDamageTypes BuffDamageTypes;
    }
}