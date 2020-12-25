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
    [Node(false, "技能数据部分/临时替换动画资源Buff", typeof (NPBehaveCanvas))]
    public class ReplaceAnimBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "临时替换动画资源Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "临时替换动画资源Buff",
                    BuffData = new ReplaceAnimBuffData() { BelongBuffSystemType = BuffSystemType.ReplaceAnimBuffSystem }
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
