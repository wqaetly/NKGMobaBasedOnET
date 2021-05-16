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
    [Node(false, "技能数据部分/播放特效Buff", typeof (NPBehaveCanvas))]
    public class PlayEffectBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "播放特效Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "播放特效Buff",
                    BuffData = new PlayEffectBuffData() { BelongBuffSystemType = BuffSystemType.PlayEffectBuffSystem }
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
