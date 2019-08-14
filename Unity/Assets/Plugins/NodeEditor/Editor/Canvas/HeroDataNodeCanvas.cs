//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 18:27:44
//------------------------------------------------------------

using System.IO;
using ETMode;
using ETModel;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodeEditorFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.NodeEditor.Editor.Canvas
{
    [NodeCanvasType("英雄数据Canvas")]
    public class HeroDataNodeCanvas: NodeCanvas
    {
        public override string canvasName => Name;

        [Title("本Canvas所有数据整理部分")]
        [LabelText("保存文件名"), GUIColor(0.9f, 0.7f, 1)]
        public string Name = "HeroData";

        [LabelText("保存路径"), GUIColor(0.1f, 0.7f, 1)]
        [FolderPath]
        public string SavePath;

        /// <summary>
        /// 节点数据载体，用以搜集所有本SO文件的数据
        /// </summary>
        public HeroDataSupportor m_TestDic=new HeroDataSupportor();

        
        [Button("扫描所有NodeData并添加", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            m_TestDic.MHeroDataSupportorDic.Clear();
            foreach (var VARIABLE in nodes)
            {
                m_TestDic.MHeroDataSupportorDic.Add(VARIABLE.HeroData_GetNodeData().HeroID, VARIABLE.HeroData_GetNodeData());
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