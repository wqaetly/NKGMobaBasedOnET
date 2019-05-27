//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月14日 14:47:44
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ETModel;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodeEditorFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SkillDemo
{
    /// <summary>
    /// 技能结点（绘图用）
    /// </summary>
    [NodeCanvasType("SkillNodeCanvas")]
    public class SkillNodeCanvas: NodeCanvas
    {
        public override string canvasName => Name;

        [LabelText("二进制文件名")]
        public string Name = "Skill";

        /// <summary>
        /// 节点数据载体，用以搜集所有本SO文件的数据
        /// </summary>
        public NodeDataSupporter m_TestDic;

        /// <summary>
        /// 节点数据载体，测试用
        /// </summary>
        public NodeDataSupporter m_DebugDic;

        CostumNodeData tempData = new CostumNodeData();

        [Button("扫描所有NodeData并添加", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            m_TestDic.m_DataDic.Clear();
            foreach (var VARIABLE in nodes)
            {
                if (m_TestDic.m_DataDic.TryGetValue(VARIABLE.GetNodeData().BelongToSkillId, out tempData))
                {
                    tempData.NodeDataInnerDic.Add(VARIABLE.GetNodeData().NodeID, VARIABLE.GetNodeData());
                }
                else
                {
                    tempData = new CostumNodeData();
                    tempData.NodeDataInnerDic.Add(VARIABLE.GetNodeData().NodeID, VARIABLE.GetNodeData());
                    m_TestDic.m_DataDic.Add(VARIABLE.GetNodeData().BelongToSkillId,
                        tempData);
                }
            }
        }

        [Button("保存技能信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            m_DebugDic.m_DataDic.Clear();
            using (FileStream file = File.Create($"Assets/Res/Config/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), m_TestDic);
            }

            Debug.Log("保存成功");
        }

        [Button("测试反序列化", 25), GUIColor(0.4f, 0.8f, 1)]
        public void TestDeserialize()
        {
            byte[] mfile = File.ReadAllBytes($"Assets/Res/Config/{this.Name}.bytes");

            if (mfile.Length == 0) Debug.Log("没有读取到文件");

            m_DebugDic = BsonSerializer.Deserialize<NodeDataSupporter>(mfile);

            foreach (KeyValuePair<int, CostumNodeData> kvp in m_DebugDic.m_DataDic)
            {
                Debug.Log($"key为{kvp.Key},Value为{kvp.Value}");
            }
        }
    }
}
