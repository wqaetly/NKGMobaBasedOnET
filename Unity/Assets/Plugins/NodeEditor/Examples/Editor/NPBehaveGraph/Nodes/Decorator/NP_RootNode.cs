//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月20日 8:00:48
//------------------------------------------------------------

using ET;
using GraphProcessor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("NPBehave行为树/根结点", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/根结点", typeof (SkillGraph))]
    public class NP_RootNode: NP_NodeBase
    {
        public override string name => "行为树根节点";

        public override Color color => Color.cyan;

        [Output("NPBehave_NextNode"), Vertical]
        [HideInInspector]
        public NP_NodeBase NextNode;

        [BoxGroup("根结点数据")]
        [HideReferenceObjectPicker]
        [HideLabel]
        public NP_RootNodeData MRootNodeData = new NP_RootNodeData { };

        public override NP_NodeDataBase NP_GetNodeData()
        {
            return this.MRootNodeData;
        }
    }
}