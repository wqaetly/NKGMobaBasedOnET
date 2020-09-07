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
    [Node(false, "技能数据部分/绑定一个状态Buff", typeof (NPBehaveCanvas))]
    public class BindStateBuffNode: SkillNodeBase
    {
        public override string GetID => Id;

        public const string Id = "绑定一个状态Buff";

        public NodeDataForSkillBuff SkillBuffBases =
                new NodeDataForSkillBuff()
                {
                    BuffDes = "绑定一个状态Buff",
                    BuffData = new BindStateBuffData() { BelongBuffSystemType = BuffSystemType.BindStateBuffSystem }
                };


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
