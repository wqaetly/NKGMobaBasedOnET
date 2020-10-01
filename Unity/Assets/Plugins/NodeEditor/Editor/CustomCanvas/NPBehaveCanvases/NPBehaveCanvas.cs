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
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking.Types;
using Node = NPBehave.Node;

namespace Plugins.NodeEditor.Editor.Canvas
{
    [NodeCanvasType("NP行为树Canvas")]
    public class NPBehaveCanvas: NPBehaveCanvasBase
    {
        [BoxGroup("此行为树数据载体")]
        public NP_DataSupportor MNpDataSupportor = new NP_DataSupportor();

        [BoxGroup("反序列化测试")]
        public NP_DataSupportor MNpDataSupportor1 = new NP_DataSupportor();

        [Button("自动配置所有结点数据", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            base.AutoSetCanvasDatas(MNpDataSupportor.NpDataSupportorBase);
            this.AutoSetSkillData_NodeData();
        }

        [Button("保存行为树信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            if (string.IsNullOrEmpty(SavePath) || string.IsNullOrEmpty(Name))
            {
                Log.Error("保存路径或文件名不能为空，请检查配置");
                return;
            }

            using (FileStream file = File.Create($"{SavePath}/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), MNpDataSupportor);
            }

            Debug.Log($"保存 {SavePath}/{this.Name}.bytes 成功");
        }

        [Button("测试反序列化", 25), GUIColor(0.4f, 0.8f, 1)]
        public void TestDe()
        {
            byte[] mfile = File.ReadAllBytes($"{SavePath}/{this.Name}.bytes");

            if (mfile.Length == 0) Debug.Log("没有读取到文件");

            try
            {
                MNpDataSupportor1 = BsonSerializer.Deserialize<NP_DataSupportor>(mfile);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        private void AutoSetSkillData_NodeData()
        {
            if (MNpDataSupportor.BuffNodeDataDic == null) return;
            MNpDataSupportor.BuffNodeDataDic.Clear();

            foreach (var node in this.nodes)
            {
                if (node is BuffNodeBase mNode)
                {
                    mNode.AutoAddLinkedBuffs();
                    BuffNodeDataBase buffNodeDataBase = mNode.Skill_GetNodeData();
                    if (buffNodeDataBase is NormalBuffNodeData normalBuffNodeData)
                    {
                        normalBuffNodeData.BuffData.BelongToBuffDataSupportorId = MNpDataSupportor.NpDataSupportorBase.RootId;
                    }

                    this.MNpDataSupportor.BuffNodeDataDic.Add(buffNodeDataBase.NodeId.Value, buffNodeDataBase);
                }
            }
        }
    }
}