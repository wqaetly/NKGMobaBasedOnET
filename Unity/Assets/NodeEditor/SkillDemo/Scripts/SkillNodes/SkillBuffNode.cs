//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月14日 18:12:12
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using NodeEditorFramework;
using NodeEditorFramework.Standard;
using NodeEditorFramework.Utilities;
using Sirenix.OdinInspector;
using SkillDemo;
using UnityEngine;

namespace SkillDemo
{
    [Node(false, "Skill/技能所衍生的Buff结点", typeof(SkillNodeCanvas))]
    public class SkillBuffNode : Node
    {
        public override string GetID => Id;

        public const string Id = "技能所衍生的Buff结点";

        [ValueConnectionKnob("PrevSkill", Direction.In, "NextSkill", NodeSide.Left, 30f)]
        public ValueConnectionKnob PrevSkill;

        [ValueConnectionKnob("NextSkill", Direction.Out, "NextSkill", NodeSide.Right, 33)]
        public ValueConnectionKnob NextSkill;

        public NodeDataForSkillBuff SkillBuffBases;

        public override BaseNodeData GetNodeData()
        {
            return SkillBuffBases;
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