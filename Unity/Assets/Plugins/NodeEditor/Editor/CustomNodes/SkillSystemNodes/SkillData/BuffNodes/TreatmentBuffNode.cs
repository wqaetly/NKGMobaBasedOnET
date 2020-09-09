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
    [Node(false, "技能数据部分/治疗Buff", typeof (NPBehaveCanvas))]
    public class TreatmentBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "治疗Buff";

        public ForBuffNodeDataBuff SkillBuffBases =
                new ForBuffNodeDataBuff()
                {
                    BuffDes = "治疗Buff",
                    BuffData = new TreatmentBuffData() { BelongBuffSystemType = BuffSystemType.TreatmentBuffSystem }
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
