//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月7日 12:30:38
//------------------------------------------------------------

using ET;
using GraphProcessor;
using Sirenix.OdinInspector;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Composite/Parallel", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Composite/Parallel", typeof (SkillGraph))]
    public class NP_ParallelNode: NP_CompositeNodeBase
    {
        public override string name => "并行节点";

        [BoxGroup("并行结点数据")]
        [HideReferenceObjectPicker]
        [HideLabel]
        public NP_ParallelNodeData NP_ParallelNodeData = new NP_ParallelNodeData { NodeDes = "并行组合器" };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_ParallelNodeData;
        }
    }
}