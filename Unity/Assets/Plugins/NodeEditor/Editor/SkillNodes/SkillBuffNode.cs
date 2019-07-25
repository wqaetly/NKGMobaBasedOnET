//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月14日 18:12:12
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using ETModel;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
using NodeEditorFramework.Utilities;
using Sirenix.OdinInspector;
using SkillDemo;
using UnityEngine;
using ContextType = NodeEditorFramework.ContextType;

namespace SkillDemo
{
    [Node(false, "Skill/技能所衍生的Buff结点", typeof (SkillNodeCanvas))]
    public class SkillBuffNode: Node
    {
        public override string GetID => Id;

        public const string Id = "技能所衍生的Buff结点";

        [ValueConnectionKnob("PrevSkill", Direction.In,"PrevNodeDatas", NodeSide.Left, 30f)]
        public ValueConnectionKnob PrevSkill;

        [ValueConnectionKnob("NextSkill", Direction.Out, "NextNodeDatas", NodeSide.Right, 33)]
        public ValueConnectionKnob NextSkill;

        public NodeDataForSkillBuff SkillBuffBases;

        public override SkillBaseNodeData Skill_GetNodeData()
        {
            return SkillBuffBases;
        }

        public override void AutoSetNodeNextAndPreIDs()
        {
            this.SkillBuffBases.PreNodeIds.Clear();
            this.SkillBuffBases.NextNodeIds.Clear();

            if (this.NextSkill.connected())
            {
                foreach (var VARIABLE in this.NextSkill.connections)
                {
                    this.SkillBuffBases?.NextNodeIds.Add(VARIABLE.GetValue<SkillBaseNodeData>().NodeID);
                }
            }

            if (this.PrevSkill.connected())
            {
                foreach (var VARIABLE in this.PrevSkill.connections)
                {
                    this.SkillBuffBases?.PreNodeIds.Add(VARIABLE.GetValue<SkillBaseNodeData>().NodeID);
                }
            }
        }

        public override void SetBaseNodeData()
        {
            this.NextSkill.SetValue(this.SkillBuffBases);
            this.PrevSkill.SetValue(this.SkillBuffBases);
        }

        public override void NodeGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            PrevSkill.DisplayLayout(new GUIContent("上一个"));

            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            NextSkill.DisplayLayout(new GUIContent("下一个"));

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            RTEditorGUI.TextField("Buff为" + SkillBuffBases?.SkillBuffBases?.GetType().Name);
        }
    }
}