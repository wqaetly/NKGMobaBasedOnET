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
    public class NP_DataSupportor
    {
        [LabelText("此行为树根结点ID")]
        public long RootId;

        [LabelText("单个行为树所有结点")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, NP_NodeDataBase> mNP_DataSupportorDic = new Dictionary<long, NP_NodeDataBase>();
        
        [LabelText("技能数据所有结点")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, SkillBaseNodeData> mSkillDataDic = new Dictionary<long, SkillBaseNodeData>();
    }
}