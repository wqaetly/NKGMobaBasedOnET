//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 16:09:42
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class B2S_CollisionsRelationSupport
    {
        [LabelText("此数据载体ID")]
        public int SupportId;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, B2S_CollisionInstance> B2S_CollisionsRelationDic = new Dictionary<long, B2S_CollisionInstance>();
    }
}