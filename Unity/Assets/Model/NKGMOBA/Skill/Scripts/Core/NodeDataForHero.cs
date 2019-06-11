//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月25日 10:36:46
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NodeDataForHero: BaseNodeData
    {
        [LabelText("英雄名称")]
        public string HeroName;

        [LabelText("英雄头像Sprite名称")]
        public string HeroAvatar;

        [LabelText("升级所需经验")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> LevelUpExp = new Dictionary<int, int>
        {
            { 1, 100 },
            { 2, 200 },
            { 3, 300 },
            { 4, 500 },
            { 5, 700 },
            { 6, 900 },
            { 7, 1200 },
            { 8, 1500 },
            { 9, 1800 },
            { 10, 2100 },
            { 11, 2400 },
            { 12, 2800 },
            { 13, 3300 },
            { 14, 3800 },
            { 15, 4300 },
            { 16, 4800 }
        };

        [TitleGroup("初始属性")]
        [LabelText("初始生命值")]
        public float OriHP;

        [LabelText("初始生命恢复")]
        public float OriHPRec;

        [LabelText("初始魔法值")]
        public float OriMagicValue;

        [LabelText("初始魔法恢复")]
        public float OriMagicRec;

        [LabelText("初始攻击力")]
        public float OriAttackValue;

        [LabelText("初始法强")]
        public float OriMagicStrength;

        [LabelText("初始护甲")]
        public float OriArmor;

        [LabelText("初始魔抗")]
        public float OriMagicResistance;

        [LabelText("初始移速")]
        public float OriMoveSpeed;

        [LabelText("初始攻速")]
        public float OriAttackSpeed;

        [LabelText("初始破甲")]
        public float OriArmorPenetration;

        [LabelText("初始法穿")]
        public float OriMagicPenetration;

        [LabelText("初始暴击率")]
        public float OriCriticalStrikeProbability;

        [LabelText("初始暴击伤害")]
        public float OriCriticalStrikeHarm;

        [LabelText("初始CD")]
        public float OriSkillCD;

        [TitleGroup("成长属性")]
        [LabelText("成长生命值")]
        public float GroHP;

        [LabelText("成长生命恢复")]
        public float GroHPRec;

        [LabelText("成长魔法值")]
        public float GroMagicValue;

        [LabelText("成长魔法恢复")]
        public float GroMagicRec;

        [LabelText("成长攻击力")]
        public float GroAttackValue;

        [LabelText("成长法强")]
        public float GroMagicStrength;

        [LabelText("成长护甲")]
        public float GroArmor;

        [LabelText("成长魔抗")]
        public float GroMagicResistance;

        [LabelText("成长移速")]
        public float GroMoveSpeed;

        [LabelText("成长攻速")]
        public float GroAttackSpeed;

        [LabelText("成长破甲")]
        public float GroArmorPenetration;

        [LabelText("成长法穿")]
        public float GroMagicPenetration;

        [LabelText("成长暴击率")]
        public float GroCriticalStrikeProbability;

        [LabelText("成长暴击伤害")]
        public float GroCriticalStrikeHarm;

        [LabelText("成长CD")]
        public float GroSkillCD;

        [TitleGroup("额外属性")]
        [LabelText("额外生命值")]
        public float ExtHP;

        [LabelText("额外魔法恢复")]
        public float ExtHPRec;

        [LabelText("额外魔法值")]
        public float ExtMagicValue;

        [LabelText("额外恢复")]
        public float ExtMagicRec;

        [LabelText("额外攻击力")]
        public float ExtAttackValue;

        [LabelText("额外法强")]
        public float ExtMagicStrength;

        [LabelText("额外护甲")]
        public float ExtArmor;

        [LabelText("额外魔抗")]
        public float ExtMagicResistance;

        [LabelText("额外移速")]
        public float ExtMoveSpeed;

        [LabelText("额外攻速")]
        public float ExtAttackSpeed;

        [LabelText("额外破甲")]
        public float ExtArmorPenetration;

        [LabelText("额外法穿")]
        public float ExtMagicPenetration;

        [LabelText("额外暴击率")]
        public float ExtCriticalStrikeProbability;

        [LabelText("额外暴击伤害")]
        public float ExtCriticalStrikeHarm;

        [LabelText("额外CD")]
        public float ExtSkillCD;
    }
}