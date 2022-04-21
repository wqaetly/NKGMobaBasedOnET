//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 21:30:55
//------------------------------------------------------------

using ET;
using GraphProcessor;
using Sirenix.OdinInspector;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/Decorator/Repeater", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Decorator/Repeater", typeof (SkillGraph))]
    public class NP_RepeaterNode: NP_DecoratorNodeBase
    {
        public override string name => "重复执行结点";

        [BoxGroup("重复执行结点数据")]
        [HideReferenceObjectPicker]
        [HideLabel]
        public NP_RepeaterNodeData NpRepeaterNodeData = new NP_RepeaterNodeData { NodeDes = "重复执行结点数据" };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NpRepeaterNodeData;
        }
    }
}