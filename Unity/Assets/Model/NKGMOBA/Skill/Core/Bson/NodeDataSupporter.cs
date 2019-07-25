//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月18日 13:26:41
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 表示一个技能完整的数据结构
    /// </summary>
    public class CostumNodeData
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, SkillBaseNodeData> NodeDataInnerDic = new Dictionary<long, SkillBaseNodeData>();
    }

    /// <summary>
    /// 节点数据载体，用以导出读取二进制数据
    /// </summary>
    [BsonIgnoreExtraElements]
    public class NodeDataSupporter
    {
        /// <summary>
        /// 表示一个英雄所有技能
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, CostumNodeData>
                m_DataDic = new Dictionary<long, CostumNodeData>();
    }
}