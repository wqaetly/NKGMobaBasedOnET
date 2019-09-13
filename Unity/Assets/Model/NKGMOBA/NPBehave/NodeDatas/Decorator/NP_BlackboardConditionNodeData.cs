//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:12:13
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NP_BlackboardConditionNodeData: NP_NodeDataBase
    {
        [LabelText("黑板条件结点")]
        public BlackboardCondition mBlackboardConditionNode;

        [LabelText("字典键")]
        public string DicKey;

        [LabelText("运算符号")]
        public Operator mOpe;

        [LabelText("终止条件")]
        public Stops stop;

        public override Decorator CreateDecoratorNode(long UnitId, long RuntimeTreeID, Node node)
        {
            //此处的value参数可以随便设，因为我们在游戏中这个value是需要动态改变的
            this.mBlackboardConditionNode = new BlackboardCondition(DicKey, this.mOpe, true, this.stop, node);
            return mBlackboardConditionNode;
        }

        public override Node NP_GetNode()
        {
            return this.mBlackboardConditionNode;
        }
    }
}