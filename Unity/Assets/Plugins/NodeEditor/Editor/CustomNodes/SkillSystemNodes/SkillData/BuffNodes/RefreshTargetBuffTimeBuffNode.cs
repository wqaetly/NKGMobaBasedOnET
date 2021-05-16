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
    [Node(false, "技能数据部分/刷新指定Buff时间Buff", typeof (NPBehaveCanvas))]
    public class RefreshTargetBuffTimeBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "刷新指定Buff时间Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "刷新指定Buff时间Buff",
                    BuffData = new RefreshTargetBuffTimeBuffData() { BelongBuffSystemType = BuffSystemType.RefreshTargetBuffTimeBuffSystem }
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
