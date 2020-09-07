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
    [Node(false, "技能数据部分/播放特效Buff", typeof (NPBehaveCanvas))]
    public class PlayEffectBuffNode: SkillNodeBase
    {
        public override string GetID => Id;

        public const string Id = "播放特效Buff";

        public NodeDataForSkillBuff SkillBuffBases =
                new NodeDataForSkillBuff()
                {
                    BuffDes = "播放特效Buff",
                    BuffData = new PlayEffectBuffData() { BelongBuffSystemType = BuffSystemType.PlayEffectBuffSystem }
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
