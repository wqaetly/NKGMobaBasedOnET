//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月17日 18:07:49
//------------------------------------------------------------

using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SkillDemo
{
    [Node(false, "Skill/检查技能结点", typeof(SkillNodeCanvas))]
    public class CheckSkillNode : Node
    {
        public override Vector2 DefaultSize => new Vector2(200, 160);

        public override string GetID => Id;

        public const string Id = "检查技能结点";

        /// <summary>
        /// 检查结点数据
        /// </summary>
        public NodeDataForCheckSkill m_SkillData;

        [ValueConnectionKnob("PrevSkill", Direction.In, "NextSkill", NodeSide.Left, 30f)]
        public ValueConnectionKnob PrevSkill;

        [ValueConnectionKnob("NextSkill", Direction.Out, "NextSkill", NodeSide.Right, 33)]
        public ValueConnectionKnob NextSkill;

        public override BaseNodeData GetNodeData()
        {
            return m_SkillData;
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

            RTEditorGUI.TextField("技能代价为" + m_SkillData?.SkillCostTypes);
        }
    }
}