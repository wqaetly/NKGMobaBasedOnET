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

        [Button("自动配置所有结点数据", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            foreach (var VARIABLE in this.nodes)
            {
                VARIABLE.NP_GetNodeData().GetNPBehaveNode();
            }

            foreach (var VARIABLE1 in this.nodes)
            {
                List<Node> tempNode = new List<Node>();
                foreach (var VARIABLE2 in ((NP_NodeBase) VARIABLE1).NextNode.connections)
                {
                    tempNode.Add(VARIABLE2.body.NP_GetNodeData().GetNPBehaveNode());
                }

                // VARIABLE1.NP_GetNodeData().AutoSetNodeData(tempNode);
            }
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