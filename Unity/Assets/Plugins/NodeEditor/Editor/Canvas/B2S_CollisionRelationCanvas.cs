//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 16:45:46
//------------------------------------------------------------

using System.IO;
using ETMode;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodeEditorFramework;
using Plugins.NodeEditor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace B2S_CollisionRelation
{
    [NodeCanvasType("包含一个英雄所有相关的碰撞关系的Canvas")]
    public class B2S_CollisionRelationCanvas: NodeCanvas
    {
        public override string canvasName => Name;

        [Title("本Canvas所有数据整理部分")]
        [LabelText("保存文件名"), GUIColor(0.9f, 0.7f, 1)]
        public string Name = "CollisionRelation";

        [LabelText("保存路径"), GUIColor(0.1f, 0.7f, 1)]
        [FolderPath]
        public string SavePath;

        /// <summary>
        /// 节点数据载体，用以搜集所有本SO文件的数据
        /// </summary>
        public B2S_CollisionsRelationSupport m_TestDic = new B2S_CollisionsRelationSupport();

        [Button("自动配置所有Node所有数据", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AutoSetNodeData()
        {
            foreach (var VARIABLE in nodes)
            {
                if (VARIABLE is B2S_CollisionRelationForOneHero)
                {
                    ((B2S_CollisionRelationForOneHero) VARIABLE).AutoSetCollisionRelations();
                }
            }

            foreach (var VARIABLE in this.groups)
            {
                foreach (var VARIABLE1 in VARIABLE.pinnedNodes)
                {
                    VARIABLE1.B2SCollisionRelation_GetNodeData().BelongGroup = VARIABLE.title;
                }
            }
        }

        [Button("自动扫描所需Node并添加到字典", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            m_TestDic.B2S_CollisionsRelationDic.Clear();
            foreach (var VARIABLE in this.groups)
            {
                if (VARIABLE.title == "GenerateCollision" || VARIABLE.title == "NoGenerateCollision")
                    foreach (var VARIABLE1 in VARIABLE.pinnedNodes)
                    {
                        m_TestDic.B2S_CollisionsRelationDic.Add(VARIABLE1.B2SCollisionRelation_GetNodeData().nodeDataId,
                            VARIABLE1.B2SCollisionRelation_GetNodeData());
                    }
            }
        }

        [Button("保存碰撞信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
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