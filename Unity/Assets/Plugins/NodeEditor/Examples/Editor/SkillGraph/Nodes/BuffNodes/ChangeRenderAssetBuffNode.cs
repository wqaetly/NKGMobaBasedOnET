//------------------------------------------------------------
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
//------------------------------------------------------------

using ET;
using GraphProcessor;
using UnityEditor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("技能数据部分/修改RenderAsset的内容", typeof (SkillGraph))]
    public class ChangeRenderAssetBuffNode: BuffNodeBase
    {
        public override string name => "修改RenderAsset的内容";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "修改RenderAsset的内容",
                    BuffData = new ChangeRenderAssetBuffData() { }
                };
        
        public override BuffNodeDataBase GetBuffNodeData()
        {
            return SkillBuffBases;
        }
    }
}
