//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月25日 11:11:47
//------------------------------------------------------------

using ETModel;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;
using UnityEditor;
using UnityEngine;

namespace SkillDemo
{
    [Node(false, "HeroData/英雄属性结点", typeof (HeroDataNodeCanvas))]
    public class HeroDataNode: Node
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

        public override Vector2 DefaultSize => new Vector2(150, 60);

        /// <summary>
        /// 英雄数据
        /// </summary>
        public NodeDataForHero m_HeroData;

        public override NodeDataForHero HeroData_GetNodeData()
        {
            return this.m_HeroData;
        }

        public override void NodeGUI()
        {
            EditorGUILayout.TextField("英雄名称：" + this.m_HeroData?.HeroName);
        }
    }
}