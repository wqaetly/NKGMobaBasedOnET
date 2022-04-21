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
    [NodeMenuItem("技能数据部分/修改属性Buff", typeof (SkillGraph))]
    public class ChangePropertyBuffNode: BuffNodeBase
    {
        public override string name => "修改属性Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "修改属性Buff",
                    BuffData = new ChangePropertyBuffData() { }
                };
        
        public override BuffNodeDataBase GetBuffNodeData()
        {
            return SkillBuffBases;
        }
    }
}
