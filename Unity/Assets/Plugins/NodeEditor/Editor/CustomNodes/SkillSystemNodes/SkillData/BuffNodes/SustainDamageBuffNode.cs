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

namespace SkillDemo
{
    [Node(false, "技能数据部分/持续伤害Buff", typeof (NPBehaveCanvas))]
    public class SustainDamageBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "持续伤害Buff";

        public ForBuffNodeDataBuff SkillBuffBases =
                new ForBuffNodeDataBuff()
                {
                    BuffDes = "持续伤害Buff",
                    BuffData = new SustainDamageBuffData() { BelongBuffSystemType = BuffSystemType.SustainDamageBuffSystem }
                };


        public override BuffNodeDataBase Skill_GetNodeData()
        {
            return SkillBuffBases;
        }

        public override void NodeGUI()
        {
            RTEditorGUI.TextField(SkillBuffBases?.BuffDes);
        }
    }
}
