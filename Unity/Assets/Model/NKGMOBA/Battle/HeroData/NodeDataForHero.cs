//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月25日 10:36:46
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 英雄基本数据
    /// </summary>
    public class NodeDataForHero
    {
        [LabelText("此结点(英雄数据结点)ID为")]
        public long HeroID;

        [LabelText("英雄名称")]
        public string HeroName;

        [LabelText("英雄职业")]
        public HeroProfession HeroProfession;

        [LabelText("英雄头像Sprite名称")]
        public string HeroAvatar;

        [LabelText("英雄天赋技能图标名称")]
        public string Talent_SkillSprite;

        [LabelText("英雄Q技能图标名称")]
        public string Q_SkillSprite;

        [LabelText("英雄W技能图标名称")]
        public string W_SkillSprite;

        [LabelText("英雄E技能图标名称")]
        public string E_SkillSprite;

        [LabelText("英雄R技能图标名称")]
        public string R_SkillSprite;

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
        
        [LabelText("初始攻击距离")]
        public float OriAttackRange;

        [LabelText("初始法强")]
        public float OriMagicStrength;

        [LabelText("初始护甲")]
        public float OriArmor;

        [LabelText("初始魔抗")]
        public float OriMagicResistance;

        [LabelText("初始移速")]
        public float OriMoveSpeed;

        [LabelText("初始攻击前摇")]
        public float OriAttackPre;
        
        [LabelText("初始攻速")]
        public float OriAttackSpeed;
        
        [LabelText("攻速收益")]
        public float OriAttackIncome;

        [LabelText("初始攻击后摇")]
        public float OriAttackPos;

        [LabelText("初始破甲")]
        public float OriArmorPenetration;

        [LabelText("初始法穿")]
        public float OriMagicPenetration;

        [LabelText("初始暴击率")]
        public float OriCriticalStrikeProbability;

        [LabelText("初始暴击伤害")]
        public float OriCriticalStrikeHarm;
        
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
    }
}