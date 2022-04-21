//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月18日 20:23:13
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ET
{
    public class HeroAttributesNodeData: UnitAttributesNodeDataBase
    {
        [TitleGroup("自定义属性")]
        [LabelText("英雄职业")]
        public HeroProfession HeroProfession;

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
    }
}