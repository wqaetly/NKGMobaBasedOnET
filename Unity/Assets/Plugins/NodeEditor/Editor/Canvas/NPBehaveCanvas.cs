//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月20日 7:55:05
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using ETModel;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodeEditorFramework;
using NPBehave;
using Plugins.NodeEditor.Editor.NPBehaveNodes;
using Sirenix.OdinInspector;
using UnityEngine;
using Node = NPBehave.Node;

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

        [LabelText("此行为树数据载体")]
        public NP_DataSupportor MNpDataSupportor = new NP_DataSupportor();
        
        [LabelText("反序列化测试")]
        public NP_DataSupportor MNpDataSupportor1 = new NP_DataSupportor();

        [Button("自动配置所有结点数据", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            MNpDataSupportor.mNP_DataSupportorDic.Clear();
            foreach (var VARIABLE1 in this.nodes)
            {
                NP_NodeDataBase mNodeData = ((NP_NodeBase) VARIABLE1).NP_GetNodeData();
                List<long> mNodeDataLinkedIds = mNodeData.linkedID;
                long mNodeDataID = mNodeData.id;
                mNodeDataLinkedIds.Clear();
                
                List<long> tempNode = new List<long>();
                foreach (var VARIABLE2 in ((NP_NodeBase) VARIABLE1).NextNode.connections)
                {
                    tempNode.Add(((NP_NodeBase) VARIABLE2.body).NP_GetNodeData().id);
                }
                
                if (tempNode.Count > 0)
                {
                    mNodeDataLinkedIds.AddRange(tempNode);
                }

                MNpDataSupportor.mNP_DataSupportorDic.Add(mNodeDataID, mNodeData);
            }
        }

        [Button("保存行为树信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            using (FileStream file = File.Create($"{SavePath}/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MNpDataSupportor);
            }

            Debug.Log("保存成功");
        }

        [Button("测试反序列化", 25), GUIColor(0.4f, 0.8f, 1)]
        public void TestDe()
        {
            byte[] mfile = File.ReadAllBytes($"{SavePath}/{this.Name}.bytes");

            if (mfile.Length == 0) Debug.Log("没有读取到文件");

            MNpDataSupportor1 = BsonSerializer.Deserialize<NP_DataSupportor>(mfile);
        }
    }
}