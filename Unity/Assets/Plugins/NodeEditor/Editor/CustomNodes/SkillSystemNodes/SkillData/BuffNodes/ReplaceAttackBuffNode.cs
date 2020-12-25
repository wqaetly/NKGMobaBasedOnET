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
    [Node(false, "技能数据部分/替换攻击流程Buff", typeof (NPBehaveCanvas))]
    public class ReplaceAttackBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "替换攻击流程Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "替换攻击流程Buff",
                    BuffData = new ReplaceAttackBuffData() { BelongBuffSystemType = BuffSystemType.ReplaceAttackBuffSystem }
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
