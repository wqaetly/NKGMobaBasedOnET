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
    [NodeMenuItem("技能数据部分/替换攻击流程Buff", typeof (SkillGraph))]
    public class ReplaceAttackBuffNode: BuffNodeBase
    {
        public override string name => "替换攻击流程Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "替换攻击流程Buff",
                    BuffData = new ReplaceAttackBuffData() { }
                };

        public override BuffNodeDataBase GetBuffNodeData()
        {
            return SkillBuffBases;
        }
    }
}
