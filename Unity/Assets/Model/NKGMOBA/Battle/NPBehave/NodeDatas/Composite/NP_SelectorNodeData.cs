//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:11:12
//------------------------------------------------------------

using MongoDB.Bson.Serialization.Attributes;
using NPBehave;
using Sirenix.OdinInspector;

namespace ET
{
    public class NP_SelectorNodeData:NP_NodeDataBase
    {
        [HideInEditorMode]
        private Selector m_SelectorNode;

        public override Composite CreateComposite(Node[] nodes)
        {
            this.m_SelectorNode = new Selector(nodes);
            return this.m_SelectorNode;
        }

        public override Node NP_GetNode()
        {
            return this.m_SelectorNode;
        }
    }
}