using Sirenix.OdinInspector;

namespace NPBehave
{
    public enum Stops
    {
        [LabelText("永远不会停止任何正在运行的节点")]
        NONE,
        [LabelText("不满足时停止自身")]
        SELF,
        [LabelText("满足时将停止比此结点优先级较低的节点")]
        LOWER_PRIORITY,
        [LabelText("不满足时将同时停止自身和优先级较低的节点")]
        BOTH,
        [LabelText("满足时，它将停止优先级较低的节点")]
        IMMEDIATE_RESTART,
        [LabelText("满足时，它将停止优先级较低的节点(基本不用)")]
        LOWER_PRIORITY_IMMEDIATE_RESTART
    }
}