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
    [NodeMenuItem("技能数据部分/持续伤害Buff", typeof (SkillGraph))]
    public class SustainDamageBuffNode: BuffNodeBase
    {
        public override string name => "持续伤害Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "持续伤害Buff",
                    BuffData = new SustainDamageBuffData() { }
                };

        public override BuffNodeDataBase GetBuffNodeData()
        {
            return SkillBuffBases;
        }
    }
}
