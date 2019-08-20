//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月20日 7:55:05
//------------------------------------------------------------

using System.IO;
using ETModel;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodeEditorFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.NodeEditor.Editor.Canvas
{
    [NodeCanvasType("NP行为树Canvas")]
    public class NPBehaveCanvas: NodeCanvas
    {
        public override string canvasName => Name;

        [Title("本Canvas所有数据整理部分")]
        [LabelText("保存文件名"), GUIColor(0.9f, 0.7f, 1)]
        public string Name = "NPBehaveData";

        [LabelText("保存路径"), GUIColor(0.1f, 0.7f, 1)]
        [FolderPath]
        public string SavePath;

        [LabelText("此行为树ID")]
        public long id;

        /// <summary>
        /// 节点数据载体，用以搜集所有本SO文件的数据
        /// </summary>
        public NP_NodeDataSupportor m_Test = new NP_NodeDataSupportor();

        [Button("结点自动进行所有数据绑定", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            m_Test.id = id;
            //this.m_Test.MNpNodeDataBase = 
        }

        [Button("保存行为树信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            using (FileStream file = File.Create($"{SavePath}/{this.Name}.bytes"))
            {
               // BsonSerializer.Serialize(new BsonBinaryWriter(file), m_TestDic);
            }

            Debug.Log("保存成功");
        }
    }
}