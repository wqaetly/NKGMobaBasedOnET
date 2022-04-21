//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月14日 21:48:24
//------------------------------------------------------------

using ET;
using GraphProcessor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Decorator/BlackboardMultipleConditions", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Decorator/BlackboardMultipleConditions", typeof (SkillGraph))]
    public class NP_BlackboardMultipleConditionsNode: NP_DecoratorNodeBase
    {
        public override string name => "黑板多条件节点";

        public NP_BlackboardMultipleConditionsNodeData NP_BlackboardMultipleConditionsNodeData =
                new NP_BlackboardMultipleConditionsNodeData { NodeDes = "黑板多条件节点" };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_BlackboardMultipleConditionsNodeData;
        }
        
    }
}