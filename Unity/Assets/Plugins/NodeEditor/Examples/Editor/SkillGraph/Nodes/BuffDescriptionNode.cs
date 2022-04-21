//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月17日 19:24:42
//------------------------------------------------------------

using ET;
using GraphProcessor;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("技能数据部分/技能描述结点", typeof (SkillGraph))]
    public class BuffDescriptionNode: BuffNodeBase
    {
        [LabelText("技能描述数据")]
        public SkillDesNodeData mSkillDesNodeData = new SkillDesNodeData();

        public override string name => "技能描述结点";

        public override BuffNodeDataBase GetBuffNodeData()
        {
            return this.mSkillDesNodeData;
        }
    }
}