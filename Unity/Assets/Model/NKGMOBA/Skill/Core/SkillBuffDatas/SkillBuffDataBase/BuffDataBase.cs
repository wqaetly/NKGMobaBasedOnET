//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月14日 22:44:27
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    [GUIColor(193 / 255f, 255 / 255f, 193 / 255f)]
    public class BuffDataBase
    {
        [Title("Buff基本信息")]
        [LabelText("Buff是否状态栏可见")]
        public bool Base_isVisualable;

        [LabelText("归属的技能ID")]
        public int BelongSkillId;

        [LabelText("归属的BuffSystem类型")]
        public BuffSystemType BelongBuffSystemType;

        [ShowIf("Base_isVisualable")]
        [LabelText("Buff图标的名称")]
        public string SpriteABInfo;

        [LabelText("Buff的基本特征")]
        public BuffBaseType BuffBaseType;
        
        [LabelText("Buff效果为")]
        public BuffWorkTypes Base_BuffExtraWork;

        [LabelText("Buff持续时间,-1代表永久,0代表此处设置无效")]
        public float Base_buffSustainTime;

        [LabelText("Buff基础数值影响者")]
        public BuffEffectedTypes Base_BuffEffectedTypes;

        [LabelText("具体的加成(可能会一个效果多种加成方式)")]
        public Dictionary<BuffAdditionTypes, float> additionValue;

        [LabelText("将要被改变的数值，若键为-1，表明定值")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> ValueToBeChanged = new Dictionary<int, float>();
    }
}