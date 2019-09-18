//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月14日 22:44:27
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    [GUIColor(0.4f, 0.8f, 1)]
    public class SkillBuffDataBase
    {
        [Title("Buff基本信息")]
        [LabelText("Buff是否状态栏可见")]
        public bool Base_isVisualable;

        [LabelText("Buff效果为")]
        public BuffWorkTypes Base_BuffExtraWork;

        [LabelText("Buff持续时间,-1代表永久,0代表此处设置无效")]
        public float Base_buffSustainTime;

        [LabelText("Buff基础数值影响者")]
        public BuffEffectedTypes Base_BuffEffectedTypes;

        [LabelText("Buff加成数值来自于")]
        public BuffTypes Base_BuffTypes;

        [LabelText("将要被改变的数值，若键为-1，表明定值")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, float> ValueToBeChanged = new Dictionary<int, float>();
    }
}