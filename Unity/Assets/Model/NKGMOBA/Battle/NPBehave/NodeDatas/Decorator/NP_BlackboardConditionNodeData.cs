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
        [HideInEditorMode] private BlackboardCondition m_BlackboardConditionNode;

        [LabelText("运算符号")] public Operator Ope;

        [LabelText("终止条件")] public Stops Stop;
        
        public NP_BlackBoardRelationData NPBalckBoardRelationData = new NP_BlackBoardRelationData();

        public override Decorator CreateDecoratorNode(long UnitId, long RuntimeTreeID, Node node)
        {
            this.m_BlackboardConditionNode = new BlackboardCondition(this.NPBalckBoardRelationData.BBKey,
                this.Ope,
                this.NPBalckBoardRelationData.NP_BBValue, this.Stop, node);
            //此处的value参数可以随便设，因为我们在游戏中这个value是需要动态改变的
            return this.m_BlackboardConditionNode;
        }

        public override Node NP_GetNode()
        {
            return this.m_BlackboardConditionNode;
        }
    }
}