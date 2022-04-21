//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月15日 11:19:33
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ET;
using GraphProcessor;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public class UnitAttributesDataGraph: BaseGraph
    {
        [Title("本Canvas所有数据整理部分")]
        [LabelText("保存文件名"), GUIColor(0.9f, 0.7f, 1)]
        public string Name = "HeroData";

        [LabelText("保存路径"), GUIColor(0.1f, 0.7f, 1)]
        [FolderPath]
        public string SavePath;

        /// <summary>
        /// 节点数据载体，用以搜集所有本SO文件的数据
        /// </summary>
        public UnitAttributesDataSupportor m_TestDic = new UnitAttributesDataSupportor();

        [Button("扫描所有NodeData并添加", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            m_TestDic.UnitAttributesDataSupportorDic.Clear();
            foreach (var node in nodes)
            {
                if (node is UnitAttributesNodeBase unitAttributesNodeBase)
                {
                    m_TestDic.UnitAttributesDataSupportorDic.Add(unitAttributesNodeBase.UnitAttributesData_GetNodeData().UnitDataNodeId,
                        unitAttributesNodeBase.UnitAttributesData_GetNodeData());
                }
            }
        }

        [Button("保存技能信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            using (FileStream file = File.Create($"{SavePath}/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), m_TestDic);
            }

            Debug.Log("保存成功");
        }
    }
}