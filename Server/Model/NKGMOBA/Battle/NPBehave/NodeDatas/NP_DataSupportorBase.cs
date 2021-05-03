//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月9日 20:05:37
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel.BBValues;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    [BoxGroup("NPBehave行为树通用基础数据")]
    [HideLabel]
    public class NP_DataSupportorBase
    {
        [LabelText("此行为树Id，也是根节点Id")]
        public long NPBehaveTreeDataId;

        [LabelText("单个行为树所有结点")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, NP_NodeDataBase> NP_DataSupportorDic = new Dictionary<long, NP_NodeDataBase>();
        
        [LabelText("黑板数据")]
        public Dictionary<string, ANP_BBValue> NP_BBValueManager = new Dictionary<string, ANP_BBValue>();
    }
}