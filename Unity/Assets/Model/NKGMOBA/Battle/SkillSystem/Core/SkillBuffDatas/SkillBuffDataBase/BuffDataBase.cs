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

        [LabelText("Buff的标识ID，用以区分不同Buff")]
        public int FlagId;

        [LabelText("归属的技能ID")]
        public int BelongSkillId;

        [LabelText("要抛出的事件ID，如果有的话")]
        public string theEventID;

        [LabelText("归属的BuffSystem类型")]
        public BuffSystemType BelongBuffSystemType;

        [LabelText("Buff的添加目标")]
        public BuffTargetTypes BuffTargetTypes;

        [LabelText("是否可以叠加(不能叠加就刷新，叠加满也刷新)")]
        public bool CanOverlay;

        [ShowIf("CanOverlay")]
        [LabelText("叠加层数")]
        [MinValue(1)]
        public int TargetOverlay = 1;

        [ShowIf("CanOverlay")]
        [LabelText("最大叠加数")]
        public int MaxOverlay;

        [ShowIf("Base_isVisualable")]
        [LabelText("Buff图标的名称")]
        public string SpriteABInfo;

        [LabelText("Buff的基本特征")]
        public BuffBaseType BuffBaseType;

        [LabelText("Buff效果为")]
        public BuffWorkTypes BuffWorkType;

        [LabelText("Buff持续时间,-1代表永久,0代表此处设置无效")]
        public long SustainTime;

        [LabelText("Buff基础数值影响者")]
        public BuffBaseDataEffectTypes BaseBuffBaseDataEffectTypes;

        [LabelText("将要被改变的基础数值")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> ValueToBeChanged = new Dictionary<int, float>();

        [LabelText("具体的加成(可能会一个效果多种加成方式)")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<BuffAdditionTypes, float> additionValue;
    }
}