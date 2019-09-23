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
    public enum CompareType
    {
        _String,
        _Float,
        _Int,
    }

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

        [LabelText("将要对比的值类型")]
        public CompareType m_CompareType;

        [ShowIf("m_CompareType", CompareType._String)]
        public string theStringWillBeCompare;

        [ShowIf("m_CompareType", CompareType._Float)]
        public float theFloatWillBeCompare;

        [ShowIf("m_CompareType", CompareType._Int)]
        public int theIntWillBeCompare;

        
        public override Decorator CreateDecoratorNode(long UnitId, long RuntimeTreeID, Node node)
        {
            switch (m_CompareType)
            {
                case CompareType._String:
                    this.mBlackboardConditionNode = new BlackboardCondition(DicKey, this.mOpe, this.theStringWillBeCompare, this.stop, node);
                    break;
                case CompareType._Float:
                    this.mBlackboardConditionNode = new BlackboardCondition(DicKey, this.mOpe, this.theFloatWillBeCompare, this.stop, node);
                    break;
                case CompareType._Int:
                    this.mBlackboardConditionNode = new BlackboardCondition(DicKey, this.mOpe, this.theIntWillBeCompare, this.stop, node);
                    break;
            }
            //此处的value参数可以随便设，因为我们在游戏中这个value是需要动态改变的
            return mBlackboardConditionNode;
        }

        public override Node NP_GetNode()
        {
            return this.mBlackboardConditionNode;
        }
    }
}