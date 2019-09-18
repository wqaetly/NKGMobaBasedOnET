//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月18日 16:38:28
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 百分比加成的伤害
    /// </summary>
    public class DamageBuff_Flash: SkillBuffDataBase
    {
        [LabelText("伤害类型")]
        public BuffDamageTypes BuffDamageTypes;
        
        [LabelText("具体的加成百分比(可能会一个伤害多种加成方式)")]
        public List<float> additionValue;

        [LabelText("预伤害修正")]
        public float damageFix = 1.0f;
    }
}