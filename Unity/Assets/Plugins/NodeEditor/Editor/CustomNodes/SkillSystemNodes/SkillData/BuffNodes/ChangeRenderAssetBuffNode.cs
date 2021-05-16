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
    [Node(false, "技能数据部分/修改RenderAsset的内容", typeof (NPBehaveCanvas))]
    public class ChangeRenderAssetBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "修改RenderAsset的内容";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "修改RenderAsset的内容",
                    BuffData = new ChangeRenderAssetBuffData() { BelongBuffSystemType = BuffSystemType.ChangeRenderAssetBuffSystem }
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
