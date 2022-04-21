//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 17:54:50
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Decorator/BlackboardCondition", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Decorator/BlackboardCondition", typeof (SkillGraph))]
    public class NP_BlackboardConditionNode: NP_DecoratorNodeBase
    {
        public override string name => "黑板条件结点";

        public NP_BlackboardConditionNodeData NP_BlackboardConditionNodeData =
                new NP_BlackboardConditionNodeData { NodeDes = "黑板条件结点" };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_BlackboardConditionNodeData;
        }
    }
}