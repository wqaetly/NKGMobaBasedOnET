//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 16:45:46
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ETModel;
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

        [TabGroup("数据部分")]
        [LabelText("保存文件名"), GUIColor(0.9f, 0.7f, 1)]
        public string Name = "CollisionRelation";

        [TabGroup("数据部分")]
        [LabelText("保存路径"), GUIColor(0.1f, 0.7f, 1)]
        [FolderPath]
        public string SavePath;

        /// <summary>
        /// 节点数据载体，用以搜集所有本SO文件的数据
        /// </summary>
        [TabGroup("数据部分")]
        public B2S_CollisionsRelationSupport m_MainDataDic = new B2S_CollisionsRelationSupport();

        /// <summary>
        /// 预制数据结点载体
        /// </summary>
        [TabGroup("数据部分")]
        [DisableInEditorMode]
        public B2S_CollisionsRelationSupport m_PrefabDic = new B2S_CollisionsRelationSupport();

        /// <summary>
        /// 预制数据结点载体,long为结点id，list为结点所包含的数据
        /// </summary>
        [TabGroup("数据部分")]
        [DisableInEditorMode]
        public Dictionary<long, List<long>> m_PrefabDataDic = new Dictionary<long, List<long>>();

        [TabGroup("数据部分")]
        [Button("自动配置所有Node所有数据", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AutoSetNodeData()
        {
            foreach (var node in nodes)
            {
                if (node is B2S_CollisionRelationForOneHero)
                {
                    ((B2S_CollisionRelationForOneHero) node).AutoSetCollisionRelations();
                }
            }

            foreach (var nodeGroup in this.groups)
            {
                foreach (var VARIABLE1 in nodeGroup.pinnedNodes)
                {
                    VARIABLE1.B2SCollisionRelation_GetNodeData().BelongGroup = nodeGroup.title;
                }
            }
        }

        [TabGroup("数据部分")]
        [Button("自动扫描所需Node并添加到字典", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            this.m_MainDataDic.B2S_CollisionsRelationDic.Clear();
            this.m_PrefabDic.B2S_CollisionsRelationDic.Clear();
            this.m_PrefabDataDic.Clear();
            foreach (var nodeGroup in this.groups)
            {
                if (nodeGroup.title == "GenerateCollision" || nodeGroup.title == "NoGenerateCollision")
                    foreach (var VARIABLE1 in nodeGroup.pinnedNodes)
                    {
                        this.m_MainDataDic.B2S_CollisionsRelationDic.Add(VARIABLE1.B2SCollisionRelation_GetNodeData().nodeDataId,
                            VARIABLE1.B2SCollisionRelation_GetNodeData());
                    }
                else
                {
                    foreach (var VARIABLE2 in nodeGroup.pinnedNodes)
                    {
                        this.m_PrefabDic.B2S_CollisionsRelationDic.Add(VARIABLE2.B2SCollisionRelation_GetNodeData().nodeDataId,
                            VARIABLE2.B2SCollisionRelation_GetNodeData());
                        this.m_PrefabDataDic.Add(VARIABLE2.B2SCollisionRelation_GetNodeData().nodeDataId,
                            VARIABLE2.Prefab_GetNodeData().colliderNodeIDs);
                    }
                }
            }
        }

        [TabGroup("数据部分")]
        [Button("保存碰撞信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            using (FileStream file = File.Create($"{SavePath}/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.m_MainDataDic);
            }

            Debug.Log("保存成功");
        }

        [TabGroup("自动生成代码部分")]
        [LabelText("Key为结点flag，value为保存的类名")]
        public Dictionary<(string, B2S_CollisionInstance), string> ClassName = new Dictionary<(string, B2S_CollisionInstance), string>();

        [TabGroup("自动生成代码部分")]
        [LabelText("碰撞关系代码保存路径")]
        [FolderPath]
        public string theCollisionPathWillBeSaved;

        [TabGroup("自动生成代码部分")]
        [LabelText("用于添加碰撞关系组件EventID保存路径")]
        [FolderPath]
        public string theEventIdPathWillBeSaved;

        //用来记录已经生成过的id
        public List<long> hasRegisterIDs = new List<long>();

        [TabGroup("自动生成代码部分")]
        [Button("读取所有结点flag信息(配置保存类名用)", 25), GUIColor(0.4f, 0.8f, 1)]
        public void ReadAllNodeFlag()
        {
            this.AddAllNodeData();
            if (ClassName == null) ClassName = new Dictionary<(string, B2S_CollisionInstance), string>();
            var tempSave = new Dictionary<string, string>();
            foreach (KeyValuePair<(string, B2S_CollisionInstance), string> className in ClassName)
            {
                tempSave.Add(className.Key.Item1, className.Value);
            }

            this.ClassName.Clear();
            foreach (KeyValuePair<long, B2S_CollisionInstance> b2SCollisionInstance in this.m_MainDataDic.B2S_CollisionsRelationDic)
            {
                if (b2SCollisionInstance.Value.BelongGroup == "GenerateCollision")
                    if (tempSave.ContainsKey(b2SCollisionInstance.Value.Flag))
                    {
                        this.ClassName.Add((b2SCollisionInstance.Value.Flag, b2SCollisionInstance.Value), tempSave[b2SCollisionInstance.Value.Flag]);
                    }
                    else
                    {
                        this.ClassName.Add((b2SCollisionInstance.Value.Flag, b2SCollisionInstance.Value), "");
                    }
            }
        }

        [TabGroup("自动生成代码部分")]
        [Button("开始自动生成代码", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AutoGenerateCollisionCode()
        {
            #region //添加碰撞组件ID部分

            StringBuilder sb1 = new StringBuilder();
            sb1.AppendLine("//------------------------------------------------------------");
            sb1.AppendLine("// Author: 烟雨迷离半世殇");
            sb1.AppendLine("// Mail: 1778139321@qq.com");
            sb1.AppendLine($"// Data: {DateTime.Now}");
            sb1.AppendLine("// Description: 此代码switch case部分由工具生成，请勿进行增减操作");
            sb1.AppendLine("//------------------------------------------------------------");
            sb1.AppendLine();
            sb1.AppendLine("namespace ETHotfix");
            sb1.AppendLine("{");
            sb1.AppendLine("    public static class EventIdType_Collision");
            sb1.AppendLine("    {");
            foreach (KeyValuePair<(string, B2S_CollisionInstance), string> className in this.ClassName)
            {
                if (className.Value == "") continue;
                sb1.AppendLine($"        public const string {className.Value} = \"{className.Key.Item2.nodeDataId}\";");
            }

            sb1.AppendLine("    }");
            sb1.AppendLine("}");
            File.WriteAllText($"{this.theEventIdPathWillBeSaved}/EventIdType_Collision.cs", sb1.ToString());

            #endregion //添加碰撞组件ID部分

            //碰撞关系代码部分
            foreach (KeyValuePair<(string, B2S_CollisionInstance), string> className in this.ClassName)
            {
                if (className.Value == "") continue;
                hasRegisterIDs.Clear();
                List<string> GroupInfo = new List<string>();
                //string为group名称，List为group中结点id
                Dictionary<string, List<long>> Group_IDInfo = new Dictionary<string, List<long>>();
                this.CollectGroupInfos(className.Key.Item2, GroupInfo, Group_IDInfo);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("//------------------------------------------------------------");
                sb.AppendLine("// Author: 烟雨迷离半世殇");
                sb.AppendLine("// Mail: 1778139321@qq.com");
                sb.AppendLine($"// Data: {DateTime.Now}");
                sb.AppendLine("// Description: 此代码switch case与System部分由工具生成，请勿进行增减操作");
                sb.AppendLine("//------------------------------------------------------------");
                sb.AppendLine();
                sb.AppendLine("using ETModel;");
                sb.AppendLine();
                sb.AppendLine("namespace ETHotfix");
                sb.AppendLine("{");

                sb.AppendLine($"    [Event(EventIdType_Collision.{className.Value})]");
                sb.AppendLine($"    public class Add{className.Value}System: AEvent<Entity>");
                sb.AppendLine("    {");
                sb.AppendLine("        public override void Run(Entity a)");
                sb.AppendLine("        {");
                sb.AppendLine($"            a.AddComponent<{className.Value}>();");
                sb.AppendLine("        }");
                sb.AppendLine("    }");

                sb.AppendLine($"    [ObjectSystem]");
                sb.AppendLine($"    public class {className.Value}AwakeSystem: AwakeSystem<{className.Value}>");
                sb.AppendLine("    {");
                sb.AppendLine($"        public override void Awake({className.Value} self)");
                sb.AppendLine("        {");
                sb.AppendLine("            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideStartAction += self.OnCollideStart;");
                sb.AppendLine(
                    "            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideSustainAction += self.OnCollideSustain;");
                sb.AppendLine(
                    "            self.Entity.GetComponent<B2S_CollisionResponseComponent>().OnCollideFinishAction += self.OnCollideFinish;");
                sb.AppendLine("        }");
                sb.AppendLine("    }");

                sb.AppendLine($"    public class {className.Value} : Component");
                sb.AppendLine("    {");
                sb.AppendLine("        public void OnCollideStart(B2S_HeroColliderData b2SHeroColliderData)");
                sb.AppendLine("        {");

                foreach (var VARIABLE1 in GroupInfo)
                {
                    sb.AppendLine("            switch (b2SHeroColliderData.m_B2S_CollisionInstance.nodeDataId)");
                    sb.AppendLine("            {");
                    foreach (var VARIABLE2 in Group_IDInfo)
                    {
                        if (VARIABLE2.Key == VARIABLE1)
                        {
                            //对于此Group中的每个预制数据结点
                            foreach (var VARIABLE3 in VARIABLE2.Value)
                            {
                                //如果有联系
                                if (className.Key.Item2.CollisionRelations.Exists(t => t == VARIABLE3))
                                {
                                    //对于此预制数据结点所包含的所有node信息
                                    foreach (var VARIABLE4 in this.m_PrefabDataDic[VARIABLE3])
                                    {
                                        //如果已经注册过就不用再注册了
                                        if (this.hasRegisterIDs.Exists(t => t == VARIABLE4))
                                        {
                                            continue;
                                        }

                                        sb.AppendLine($"                case {VARIABLE4}://{this.m_MainDataDic.B2S_CollisionsRelationDic[VARIABLE4].Flag}");
                                        foreach (var VARIABLE6 in VARIABLE2.Value)
                                        {
                                            if (className.Key.Item2.CollisionRelations.Exists(t => t == VARIABLE6) &&
                                                this.m_PrefabDataDic[VARIABLE6].Exists(t => t == VARIABLE4))
                                            {
                                                sb.AppendLine(
                                                    $"                    //{this.m_PrefabDic.B2S_CollisionsRelationDic[VARIABLE6].Flag}");
                                            }
                                        }

                                        sb.AppendLine("                    break;");
                                        
                                        //添加已注册信息
                                        this.hasRegisterIDs.Add(VARIABLE4);
                                    }
                                }
                            }
                        }
                    }
                    sb.AppendLine("            }");
                }

                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine("        public void OnCollideSustain(B2S_HeroColliderData b2SHeroColliderData)");
                sb.AppendLine("        {");
                sb.AppendLine();
                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine("        public void OnCollideFinish(B2S_HeroColliderData b2SHeroColliderData)");
                sb.AppendLine("        {");
                sb.AppendLine();
                sb.AppendLine("        }");
                sb.AppendLine("    }");
                sb.AppendLine("}");
                string tempFileInfo = $"{this.theCollisionPathWillBeSaved}/{className.Value}.cs";
                int GS = 0;
                Log.Info(tempFileInfo);
                while (File.Exists(tempFileInfo))
                {
                    GS++;
                    tempFileInfo=$"{this.theCollisionPathWillBeSaved}/{className.Value}_{GS}.cs";
                }
                File.WriteAllText(tempFileInfo, sb.ToString());
            }
        }

        /// <summary>
        /// 收集Group信息
        /// </summary>
        private void CollectGroupInfos(B2S_CollisionInstance b2SCollisionInstance, List<string> groupInfo,
        Dictionary<string, List<long>> group_IDInfo)
        {
            groupInfo.Clear();
            group_IDInfo.Clear();

            foreach (var collisionRelation in b2SCollisionInstance.CollisionRelations)
            {
                var b2stemp = new B2S_CollisionInstance();
                this.m_PrefabDic.B2S_CollisionsRelationDic.TryGetValue(collisionRelation, out b2stemp);
                if (!groupInfo.Exists(t => t == b2stemp.BelongGroup))
                    groupInfo.Add(b2stemp.BelongGroup);
            }

            foreach (var group in groupInfo)
            {
                foreach (KeyValuePair<long, B2S_CollisionInstance> VARIABLE2 in this.m_PrefabDic.B2S_CollisionsRelationDic)
                {
                    if (VARIABLE2.Value.BelongGroup == group)
                    {
                        if (group_IDInfo.ContainsKey(group))
                        {
                            group_IDInfo[group].Add(VARIABLE2.Value.nodeDataId);
                        }
                        else
                        {
                            group_IDInfo.Add(group, new List<long>() { VARIABLE2.Value.nodeDataId });
                        }
                    }
                }
            }
        }
    }
}