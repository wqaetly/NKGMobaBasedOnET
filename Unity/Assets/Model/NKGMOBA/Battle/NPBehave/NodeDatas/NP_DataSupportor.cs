//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 8:06:52
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    [HideLabel]
    public class NP_DataSupportor
    {
        [BoxGroup("技能中的Buff数据结点")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, BuffNodeDataBase> BuffDataDic = new Dictionary<long, BuffNodeDataBase>();

        public NP_DataSupportorBase NpDataSupportorBase = new NP_DataSupportorBase();
    }
}