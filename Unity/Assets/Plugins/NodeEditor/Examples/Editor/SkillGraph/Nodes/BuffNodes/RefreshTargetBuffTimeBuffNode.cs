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
    [NodeMenuItem("技能数据部分/刷新指定Buff时间Buff", typeof (SkillGraph))]
    public class RefreshTargetBuffTimeBuffNode: BuffNodeBase
    {
        public override string name => "刷新指定Buff时间Buff";
        
        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "刷新指定Buff时间Buff",
                    BuffData = new RefreshTargetBuffTimeBuffData() { }
                };
        
        public override BuffNodeDataBase GetBuffNodeData()
        {
            return SkillBuffBases;
        }
    }
}
