//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月2日 16:17:50
//------------------------------------------------------------

using NodeEditorFramework;
using Plugins.NodeEditor.Editor.Canvas;

namespace Plugins.NodeEditor.Editor.NPBehaveNodes
{
    [Node(false, "NPBehave行为树结点/Task基类结点", typeof(NeverbeUsedCanvas))]
    public abstract class NP_TaskNodeBase : NP_NodeBase
    {
        [ValueConnectionKnob("NPBehave_PreNode", Direction.In, "NPBehave_PrevNodeDatas", NodeSide.Top, 75)]
        public ValueConnectionKnob PrevNode;

        public override ValueConnectionKnob GetNextNodes()
        {
            return null;
        }

        public override void ApplyNodeSize()
        {
            PrevNode.sidePosition = NodeSize.x / 2;
        }
    }
}