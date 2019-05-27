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

        [TitleGroup("初始属性"), GUIColor(205 / 255f, 205 / 255f, 180 / 255f), LabelText("初始属性")]
        public OriginValues m_OriginValues;

        [TitleGroup("成长属性"), GUIColor(84 / 255f, 1f, 159 / 255f), LabelText("成长属性")]
        public GrowingValues m_GrowingValues;

        [TitleGroup("额外属性"), GUIColor(1f, 193 / 255f, 37 / 255f), LabelText("额外属性")]
        public ExtraValues m_ExtraValues;

     
    }
}