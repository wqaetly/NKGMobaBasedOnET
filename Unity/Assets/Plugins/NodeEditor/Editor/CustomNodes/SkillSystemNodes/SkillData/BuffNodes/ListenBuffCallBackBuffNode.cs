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
    [Node(false, "技能数据部分/监听Buff", typeof (NPBehaveCanvas))]
    public class ListenBuffCallBackBuffNode: SkillNodeBase
    {
        public override string GetID => Id;

        public const string Id = "监听Buff";

        public NodeDataForSkillBuff SkillBuffBases =
                new NodeDataForSkillBuff()
                {
                    BuffDes = "监听Buff",
                    BuffData = new ListenBuffCallBackBuffData() { BelongBuffSystemType = BuffSystemType.ListenBuffCallBackBuffSystem }
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
