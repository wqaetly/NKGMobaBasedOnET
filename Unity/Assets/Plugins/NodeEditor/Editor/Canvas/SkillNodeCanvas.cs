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
    [NodeCanvasType("技能Canvas")]
    public class SkillNodeCanvas: NodeCanvas
    {
        public override string canvasName => Name;

        [Title("本Canvas所有数据整理部分")]
        [LabelText("保存文件名"), GUIColor(0.9f, 0.7f, 1)]
        public string Name = "Skill";

        [LabelText("保存路径"), GUIColor(0.1f, 0.7f, 1)]
        [FolderPath]
        public string SavePath;

        /// <summary>
        /// 节点数据载体，用以搜集所有本SO文件的数据
        /// </summary>
        public SkillNodeDataSupporter m_TestDic = new SkillNodeDataSupporter();

        /// <summary>
        /// 节点数据载体，测试用
        /// </summary>
        public SkillNodeDataSupporter m_DebugDic =new SkillNodeDataSupporter();


        [Button("保存技能信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            m_DebugDic.m_DataDic.Clear();
            using (FileStream file = File.Create($"{SavePath}/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), m_TestDic);
            }

            Debug.Log("保存成功");
        }

        [Button("测试反序列化", 25), GUIColor(0.4f, 0.8f, 1)]
        public void TestDeserialize()
        {
            byte[] mfile = File.ReadAllBytes($"{SavePath}/{this.Name}.bytes");

            if (mfile.Length == 0) Debug.Log("没有读取到文件");
            
            m_DebugDic = BsonSerializer.Deserialize<SkillNodeDataSupporter>(mfile);
            
            Log.Info("反序列化成功");
        }
    }
}