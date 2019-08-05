//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 16:45:46
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ETMode;
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

        [TabGroup("数据部分")]
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

        [TabGroup("数据部分")]
        [Button("自动扫描所需Node并添加到字典", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            this.m_MainDataDic.B2S_CollisionsRelationDic.Clear();
            this.m_PrefabDic.B2S_CollisionsRelationDic.Clear();
            foreach (var VARIABLE in this.groups)
            {
                if (VARIABLE.title == "GenerateCollision" || VARIABLE.title == "NoGenerateCollision")
                    foreach (var VARIABLE1 in VARIABLE.pinnedNodes)
                    {
                        this.m_MainDataDic.B2S_CollisionsRelationDic.Add(VARIABLE1.B2SCollisionRelation_GetNodeData().nodeDataId,
                            VARIABLE1.B2SCollisionRelation_GetNodeData());
                    }
                else
                {
                    foreach (var VARIABLE2 in VARIABLE.pinnedNodes)
                    {
                        this.m_PrefabDic.B2S_CollisionsRelationDic.Add(VARIABLE2.B2SCollisionRelation_GetNodeData().nodeDataId,
                            VARIABLE2.B2SCollisionRelation_GetNodeData());
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
        public Dictionary<(string, B2S_CollisionInstance), string> className = new Dictionary<(string, B2S_CollisionInstance), string>();

        [TabGroup("自动生成代码部分")]
        [LabelText("保存路径")]
        [FolderPath]
        public string thePathWillBeSaved;

        [TabGroup("自动生成代码部分")]
        [Button("读取所有结点flag信息(配置保存类名用)", 25), GUIColor(0.4f, 0.8f, 1)]
        public void ReadAllNodeFlag()
        {
            this.AddAllNodeData();
            if (className == null) className = new Dictionary<(string, B2S_CollisionInstance), string>();
            var tempSave = new Dictionary<string, string>();
            foreach (KeyValuePair<(string, B2S_CollisionInstance), string> VARIABLE in className)
            {
                tempSave.Add(VARIABLE.Key.Item1, VARIABLE.Value);
            }

            this.className.Clear();
            foreach (KeyValuePair<long, B2S_CollisionInstance> VARIABLE in this.m_MainDataDic.B2S_CollisionsRelationDic)
            {
                if (VARIABLE.Value.BelongGroup == "GenerateCollision")
                    if (tempSave.ContainsKey(VARIABLE.Value.Flag))
                    {
                        this.className.Add((VARIABLE.Value.Flag, VARIABLE.Value), tempSave[VARIABLE.Value.Flag]);
                    }
                    else
                    {
                        this.className.Add((VARIABLE.Value.Flag, VARIABLE.Value), "");
                    }
            }
        }

        [TabGroup("自动生成代码部分")]
        [Button("开始自动生成代码", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AutoGenerateCollisionCode()
        {
            foreach (KeyValuePair<(string, B2S_CollisionInstance), string> VARIABLE in this.className)
            {
                if (VARIABLE.Value == "") continue;
                List<string> GroupInfo = new List<string>();
                Dictionary<string, List<long>> Group_IDInfo = new Dictionary<string, List<long>>();
                this.CollectGroupInfos(VARIABLE.Key.Item2, GroupInfo, Group_IDInfo);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("//------------------------------------------------------------");
                sb.AppendLine("// Author: 烟雨迷离半世殇");
                sb.AppendLine("// Mail: 1778139321@qq.com");
                sb.AppendLine($"// Data: {DateTime.Now}");
                sb.AppendLine("// Description: 此代码switch case部分由工具生成，请勿进行增减操作");
                sb.AppendLine("//------------------------------------------------------------");
                sb.AppendLine();
                sb.AppendLine("using ETModel;");
                sb.AppendLine();
                sb.AppendLine("namespace ETHotfix");
                sb.AppendLine("{");
                sb.AppendLine($"    public class {VARIABLE.Value} : Component");
                sb.AppendLine("    {");
                sb.AppendLine("        public void OnCollideStart(B2S_FixtureUserData b2SFixtureUserData)");
                sb.AppendLine("        {");

                sb.AppendLine($"            switch (b2SFixtureUserData.m_B2S_CollisionInstance.BelongGroup)");
                sb.AppendLine("            {");
                foreach (var VARIABLE1 in GroupInfo)
                {
                    sb.AppendLine($"                case \"{VARIABLE1}\":");

                    sb.AppendLine("                    switch (b2SFixtureUserData.m_B2S_CollisionInstance.nodeDataId)");
                    sb.AppendLine("                    {");
                    foreach (var VARIABLE2 in Group_IDInfo)
                    {
                        if (VARIABLE2.Key == VARIABLE1)
                        {
                            foreach (var VARIABLE3 in VARIABLE2.Value)
                            {
                                if (VARIABLE.Key.Item2.CollisionRelations.Exists(t => t == VARIABLE3))
                                {
                                    sb.AppendLine(
                                        $"                        case {VARIABLE3}: //{this.m_PrefabDic.B2S_CollisionsRelationDic[VARIABLE3].Flag}");
                                    sb.AppendLine("                            break;");
                                }
                            }
                        }
                    }

                    sb.AppendLine("                    }");
                    sb.AppendLine("                    break;");
                }

                sb.AppendLine("            }");
                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine("        public void OnCollideSustain(B2S_FixtureUserData b2SFixtureUserData)");
                sb.AppendLine("        {");
                sb.AppendLine();
                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine("        public void OnCollideFinish(B2S_FixtureUserData b2SFixtureUserData)");
                sb.AppendLine("        {");
                sb.AppendLine();
                sb.AppendLine("        }");
                sb.AppendLine("    }");
                sb.AppendLine("}");
                File.WriteAllText($"{this.thePathWillBeSaved}/{VARIABLE.Value}.cs", sb.ToString());
            }
        }

        private void GenerateBasedOnGroupInfo(StringBuilder stringBuilder)
        {
        }

        /// <summary>
        /// 收集Group信息
        /// </summary>
        private void CollectGroupInfos(B2S_CollisionInstance b2SCollisionInstance, List<string> groupInfo,
        Dictionary<string, List<long>> group_IDInfo)
        {
            groupInfo.Clear();
            group_IDInfo.Clear();

            foreach (var VARIABLE in b2SCollisionInstance.CollisionRelations)
            {
                var b2stemp = new B2S_CollisionInstance();
                Debug.Log(VARIABLE);
                this.m_PrefabDic.B2S_CollisionsRelationDic.TryGetValue(VARIABLE, out b2stemp);
                if (!groupInfo.Exists(t => t == b2stemp.BelongGroup))
                    groupInfo.Add(b2stemp.BelongGroup);
            }

            foreach (var VARIABLE in groupInfo)
            {
                foreach (KeyValuePair<long, B2S_CollisionInstance> VARIABLE2 in this.m_PrefabDic.B2S_CollisionsRelationDic)
                {
                    if (VARIABLE2.Value.BelongGroup == VARIABLE)
                    {
                        if (group_IDInfo.ContainsKey(VARIABLE))
                        {
                            group_IDInfo[VARIABLE].Add(VARIABLE2.Value.nodeDataId);
                        }
                        else
                        {
                            group_IDInfo.Add(VARIABLE, new List<long>() { VARIABLE2.Value.nodeDataId });
                        }
                    }
                }
            }
        }
    }
}