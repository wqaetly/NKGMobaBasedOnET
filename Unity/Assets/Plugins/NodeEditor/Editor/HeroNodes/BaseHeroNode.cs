//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月25日 11:11:47
//------------------------------------------------------------

using ETModel;
using NodeEditorFramework;
using UnityEditor;
using UnityEngine;

namespace SkillDemo
{
    [Node(false, "Skill/英雄属性结点", typeof (SkillNodeCanvas))]
    public class BaseHeroNode: Node
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "英雄属性结点";

        /// <summary>
        /// 内部ID
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        public override Vector2 DefaultSize => new Vector2(200, 160);

        [ValueConnectionKnob("NextSkill", Direction.Out, "NextSkill", NodeSide.Right)]
        public ValueConnectionKnob NextSkill;

        [ValueConnectionKnob("PrevSkill", Direction.In, "NextSkill", NodeSide.Left)]
        public ValueConnectionKnob PrevSkill;

        /// <summary>
        /// 技能数据
        /// </summary>
        public NodeDataForHero m_HeroData;

        public override BaseNodeData GetNodeData()
        {
            return m_HeroData;
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

            GUILayout.BeginVertical();

            EditorGUILayout.TextField("英雄名称：" + this.m_HeroData?.HeroName);

            GUILayout.EndVertical();
        }
    }
}