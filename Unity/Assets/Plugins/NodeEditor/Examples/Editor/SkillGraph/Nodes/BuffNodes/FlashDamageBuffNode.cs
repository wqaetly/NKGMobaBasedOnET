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
    [NodeMenuItem("技能数据部分/瞬时伤害Buff", typeof (SkillGraph))]
    public class FlashDamageBuffNode: BuffNodeBase
    {
        public override string name => "瞬时伤害Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "瞬时伤害Buff",
                    BuffData = new FlashDamageBuffData() {}
                };
        
        public override BuffNodeDataBase GetBuffNodeData()
        {
            return SkillBuffBases;
        }
    }
}
