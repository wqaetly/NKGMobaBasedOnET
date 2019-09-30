//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:10:51
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NP_ParallelNodeData: NP_NodeDataBase
    {
        [LabelText("并行结点")]
        public Parallel mParallelNode;

        public override Composite CreateComposite(Node[] nodes)
        {
            this.mParallelNode = new Parallel(Parallel.Policy.ALL, Parallel.Policy.ALL, nodes);
            return mParallelNode;
        }

        public override Node NP_GetNode()
        {
            return this.mParallelNode;
        }
    }
}