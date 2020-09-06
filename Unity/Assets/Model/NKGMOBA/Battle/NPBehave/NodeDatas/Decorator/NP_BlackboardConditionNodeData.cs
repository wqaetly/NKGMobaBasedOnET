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
    [BoxGroup("黑板条件节点配置"),GUIColor(0.961f, 0.902f, 0.788f, 1f)]
    [HideLabel]
    public class NP_BlackboardConditionNodeData : NP_NodeDataBase
    {
        [HideInEditorMode] public BlackboardCondition mBlackboardConditionNode;

        [LabelText("运算符号")] public Operator mOpe;

        [LabelText("终止条件")] public Stops stop;
        
        public NP_BlackBoardRelationData m_NPBalckBoardRelationData;

        public override Decorator CreateDecoratorNode(long UnitId, long RuntimeTreeID, Node node)
        {
            this.mBlackboardConditionNode = new BlackboardCondition(m_NPBalckBoardRelationData.BBKey,
                this.mOpe,
                this.m_NPBalckBoardRelationData.NP_BBValue, this.stop, node);
            //此处的value参数可以随便设，因为我们在游戏中这个value是需要动态改变的
            return mBlackboardConditionNode;
        }

        public override Node NP_GetNode()
        {
            return this.mBlackboardConditionNode;
        }
    }
}