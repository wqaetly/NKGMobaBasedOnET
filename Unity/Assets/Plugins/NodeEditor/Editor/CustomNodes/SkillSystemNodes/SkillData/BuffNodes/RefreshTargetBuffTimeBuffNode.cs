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
    [Node(false, "技能数据部分/刷新指定Buff时间Buff", typeof (NPBehaveCanvas))]
    public class RefreshTargetBuffTimeBuffNode: SkillNodeBase
    {
        public override string GetID => Id;

        public const string Id = "刷新指定Buff时间Buff";

        public NodeDataForSkillBuff SkillBuffBases =
                new NodeDataForSkillBuff()
                {
                    BuffDes = "刷新指定Buff时间Buff",
                    BuffData = new RefreshTargetBuffTimeBuffData() { BelongBuffSystemType = BuffSystemType.RefreshTargetBuffTimeBuffSystem }
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
