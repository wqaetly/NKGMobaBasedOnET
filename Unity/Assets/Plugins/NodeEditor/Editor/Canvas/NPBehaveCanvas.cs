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
using UnityEngine.Networking.Types;
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
            this.AutoSetNP_NodeData();
            this.AutoSetSkillData_NodeData();
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

        private void AutoSetNP_NodeData()
        {
            MNpDataSupportor.mNP_DataSupportorDic.Clear();

            List<NP_NodeBase> tempNode1 = new List<NP_NodeBase>();

            foreach (var VARIABLE in this.nodes)
            {
                if (VARIABLE is NP_NodeBase mNode)
                {
                    tempNode1.Add(mNode);
                }
            }

            tempNode1.Sort((x, y) => -x.position.y.CompareTo(y.position.y));

            foreach (var VARIABLE in tempNode1)
            {
                VARIABLE.NP_GetNodeData().id = IdGenerater.GenerateId();
            }

            MNpDataSupportor.RootId = tempNode1[tempNode1.Count - 1].NP_GetNodeData().id;

            foreach (var VARIABLE1 in tempNode1)
            {
                NP_NodeDataBase mNodeData = VARIABLE1.NP_GetNodeData();
                mNodeData.linkedID.Clear();
                long mNodeDataID = mNodeData.id;
                List<NP_NodeBase> tempNode = new List<NP_NodeBase>();
                foreach (var VARIABLE2 in VARIABLE1.NextNode.connections)
                {
                    tempNode.Add((NP_NodeBase) VARIABLE2.body);
                }

                tempNode.Sort((x, y) => x.position.x.CompareTo(y.position.x));

                foreach (var np_NodeBase in tempNode)
                {
                    mNodeData.linkedID.Add(np_NodeBase.NP_GetNodeData().id);
                }

                //Log.Info($"y:{VARIABLE1.position.y},x:{VARIABLE1.position.x},id:{mNodeDataID}");
                MNpDataSupportor.mNP_DataSupportorDic.Add(mNodeDataID, mNodeData);
            }
        }

        private void AutoSetSkillData_NodeData()
        {
            MNpDataSupportor.mSkillDataDic.Clear();

            foreach (var VARIABLE in this.nodes)
            {
                if (VARIABLE is SkillNodeBase mNode)
                {
                    this.MNpDataSupportor.mSkillDataDic.Add(mNode.Skill_GetNodeData().NodeID, mNode.Skill_GetNodeData());
                }
            }
        }
    }
}