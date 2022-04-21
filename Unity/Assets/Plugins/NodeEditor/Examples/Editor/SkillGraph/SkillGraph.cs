//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月15日 11:19:33
//------------------------------------------------------------

using System;
using System.IO;
using ET;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public class SkillGraph : NPBehaveGraph
    {
        [BoxGroup("此技能树数据载体(客户端)")] [DisableInEditorMode]
        public NP_DataSupportor SkillDataSupportor_Client = new NP_DataSupportor();

        [BoxGroup("技能树反序列化测试(客户端)")] [DisableInEditorMode]
        public NP_DataSupportor SkillDataSupportor_Client_Des = new NP_DataSupportor();

        [BoxGroup("此技能树数据载体(服务端)")] [DisableInEditorMode]
        public NP_DataSupportor SkillDataSupportor_Server = new NP_DataSupportor();

        [BoxGroup("技能树反序列化测试(服务端)")] [DisableInEditorMode]
        public NP_DataSupportor SkillDataSupportor_ServerDes = new NP_DataSupportor();

        [Button("自动配置所有技能结点数据", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AutoSetCanvasDatas()
        {
            this.OnGraphEnable();
            base.AutoSetCanvasDatas();
            SkillDataSupportor_Server.NpDataSupportorBase = this.NpDataSupportor_Server;
            SkillDataSupportor_Client.NpDataSupportorBase = this.NpDataSupportor_Client;
            this.AutoSetSkillData_NodeData(SkillDataSupportor_Server);
            this.AutoSetSkillData_NodeData(SkillDataSupportor_Client);
        }

        [Button("保存技能树信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            if (string.IsNullOrEmpty(SavePathServer) || string.IsNullOrEmpty(SavePathClient) ||
                string.IsNullOrEmpty(Name))
            {
                Log.Error($"保存路径或文件名不能为空，请检查配置");
                return;
            }

            using (FileStream file = File.Create($"{SavePathServer}/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), SkillDataSupportor_Server);
            }

            using (FileStream file = File.Create($"{SavePathClient}/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), SkillDataSupportor_Client);
            }

            Log.Info($"保存 {SavePathServer}/{this.Name}.bytes {SavePathClient}/{this.Name}.bytes 成功");
        }

        [Button("测试技能树反序列化", 25), GUIColor(0.4f, 0.8f, 1)]
        public void TestDe()
        {
            try
            {
                byte[] mServerfile = File.ReadAllBytes($"{SavePathServer}/{this.Name}.bytes");
                if (mServerfile.Length == 0) Log.Info("没有读取到文件");
                SkillDataSupportor_ServerDes = BsonSerializer.Deserialize<NP_DataSupportor>(mServerfile);
                Log.Info($"反序列化 {SavePathServer}/{this.Name}.bytes 成功");

                byte[] mClientfile = File.ReadAllBytes($"{SavePathClient}/{this.Name}.bytes");
                if (mClientfile.Length == 0) Log.Info("没有读取到文件");
                SkillDataSupportor_Client_Des = BsonSerializer.Deserialize<NP_DataSupportor>(mClientfile);
                Log.Info($"反序列化 {SavePathClient}/{this.Name}.bytes 成功");
            }
            catch (Exception e)
            {
                Log.Info(e.ToString());
                throw;
            }
        }

        private void AutoSetSkillData_NodeData(NP_DataSupportor npDataSupportor)
        {
            if (npDataSupportor.BuffNodeDataDic == null) return;
            npDataSupportor.BuffNodeDataDic.Clear();

            foreach (var node in this.nodes)
            {
                if (node is BuffNodeBase mNode)
                {
                    mNode.AutoAddLinkedBuffs();
                    BuffNodeDataBase buffNodeDataBase = mNode.GetBuffNodeData();
                    if (buffNodeDataBase is NormalBuffNodeData normalBuffNodeData)
                    {
                        normalBuffNodeData.BuffData.BelongToBuffDataSupportorId =
                            npDataSupportor.NpDataSupportorBase.NPBehaveTreeDataId;
                    }

                    npDataSupportor.BuffNodeDataDic.Add(buffNodeDataBase.NodeId.Value, buffNodeDataBase);
                }
            }
        }
    }
}