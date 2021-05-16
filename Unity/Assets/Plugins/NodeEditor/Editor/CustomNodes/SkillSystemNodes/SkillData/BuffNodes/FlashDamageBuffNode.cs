//------------------------------------------------------------
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
//------------------------------------------------------------

using ETModel;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using Plugins;
using Plugins.NodeEditor.Editor.Canvas;
using UnityEditor;

namespace SkillDemo
{
    [Node(false, "技能数据部分/瞬时伤害Buff", typeof (NPBehaveCanvas))]
    public class FlashDamageBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "瞬时伤害Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "瞬时伤害Buff",
                    BuffData = new FlashDamageBuffData() { BelongBuffSystemType = BuffSystemType.FlashDamageBuffSystem }
                };


        public override BuffNodeDataBase Skill_GetNodeData()
        {
            return SkillBuffBases;
        }

        public override void NodeGUI()
        {
            EditorGUILayout.TextField(SkillBuffBases?.BuffDes);
        }
    }
}
