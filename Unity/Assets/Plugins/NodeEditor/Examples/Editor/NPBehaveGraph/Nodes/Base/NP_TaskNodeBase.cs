//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月2日 16:17:50
//------------------------------------------------------------

using GraphProcessor;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public abstract class NP_TaskNodeBase : NP_NodeBase
    {
        [Input("NPBehave_PreNode"), Vertical]
        [HideInInspector]
        public NP_NodeBase PrevNode;
    }
}