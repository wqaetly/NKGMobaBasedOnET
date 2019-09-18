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
using Plugins.NodeEditor.Editor.Canvas;
using Sirenix.OdinInspector;
using SkillDemo;
using UnityEngine;
using ContextType = NodeEditorFramework.ContextType;

namespace SkillDemo
{
    [Node(false, "技能数据部分/技能所衍生的Buff结点", typeof (NPBehaveCanvas))]
    public class SkillBuffNode: Node
    {
        public override string GetID => Id;

        public const string Id = "技能所衍生的Buff结点";

        public NodeDataForSkillBuff SkillBuffBases;

        public override Vector2 DefaultSize => new Vector2(150, 60);
        
        public override SkillBaseNodeData Skill_GetNodeData()
        {
            return SkillBuffBases;
        }
        
        public override void NodeGUI()
        {
            RTEditorGUI.TextField(SkillBuffBases?.BuffDes);
        }
    }
}