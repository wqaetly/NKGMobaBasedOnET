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
    [NodeMenuItem("技能数据部分/临时替换动画资源Buff", typeof (SkillGraph))]
    public class ReplaceAnimBuffNode: BuffNodeBase
    {
        public override string name => "临时替换动画资源Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "临时替换动画资源Buff",
                    BuffData = new ReplaceAnimBuffData() { }
                };

        public override BuffNodeDataBase GetBuffNodeData()
        {
            return SkillBuffBases;
        }
    }
}
