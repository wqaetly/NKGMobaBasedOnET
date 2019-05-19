//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月18日 13:26:41
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using UnityEngine;

namespace SkillDemo.Bson
{
    public class CostumNodeData
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, BaseNodeData> NodeDataInnerDic = new Dictionary<int, BaseNodeData>();
    }

    /// <summary>
    /// 节点数据载体，用以导出读取二进制数据
    /// </summary>
    [BsonIgnoreExtraElements]
    public class NodeDataSupporter
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, CostumNodeData>
            m_DataDic = new Dictionary<int, CostumNodeData>();

        /// <summary>
        /// 用以承载技能数据的字典
        /// </summary>
        [BsonIgnore] public CostumNodeData TempNodeData = new CostumNodeData();

        /// <summary>
        /// 按技能Id获取技能
        /// </summary>
        /// <param name="SkillId"></param>
        /// <returns></returns>
        public CostumNodeData GetSkillById(int SkillId)
        {
            if (m_DataDic.TryGetValue(SkillId, out TempNodeData))
            {
                return TempNodeData;
            }

            Debug.LogError("要查找的技能不存在！请确认技能ID");
            return null;
        }

        /// <summary>
        /// 根据ID获得此节点前面的一个NodeData
        /// </summary>
        /// <param name="belongtoId">当前节点所属技能ID</param>
        /// <param name="preId">当前节点的前面一个ID为PreId的结点</param>
        /// <returns></returns>
        public BaseNodeData GetNodeDataByPreId(int belongtoId, int preId)
        {
            return GetSkillById(belongtoId).NodeDataInnerDic[preId];
        }

        /// <summary>
        /// 根据ID获得此节点后面的一个NodeData
        /// </summary>
        /// <param name="belongtoId">当前节点所属技能ID</param>
        /// <param name="preId">当前节点的前面一个ID为NextId的结点</param>
        /// <returns></returns>
        public BaseNodeData GetNodeDataByNextId(int belongtoId, int nextId)
        {
            return GetSkillById(belongtoId).NodeDataInnerDic[nextId];
        }
    }
}