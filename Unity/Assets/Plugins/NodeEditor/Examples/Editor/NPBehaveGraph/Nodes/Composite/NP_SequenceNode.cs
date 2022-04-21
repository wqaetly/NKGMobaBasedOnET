//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 17:30:16
//------------------------------------------------------------

using ET;
using GraphProcessor;
using Sirenix.OdinInspector;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Composite/Sequence", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Composite/Sequence", typeof (SkillGraph))]
    public class NP_SequenceNode: NP_CompositeNodeBase
    {
        public override string name => "序列结点";

        [BoxGroup("Selector结点数据")]
        [HideReferenceObjectPicker]
        [HideLabel]
        public NP_SequenceNodeData NP_SequenceNodeData = new NP_SequenceNodeData { NodeDes = "序列组合器"};

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_SequenceNodeData;
        }
    }
}