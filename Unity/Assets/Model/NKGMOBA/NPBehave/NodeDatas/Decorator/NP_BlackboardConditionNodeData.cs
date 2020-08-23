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
    public enum NP_BBValueType : byte
    {
        [LabelText("string")] _String,
        [LabelText("float")] _Float,
        [LabelText("int")] _Int,
        [LabelText("bool")] _Bool,
        [LabelText("Vector3")] _Vector3
    }
    
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
            switch (m_NPBalckBoardRelationData.NP_BBValueType)
            {
                case NP_BBValueType._String:
                    this.mBlackboardConditionNode = new BlackboardCondition(m_NPBalckBoardRelationData.DicKey,
                        this.mOpe,
                        this.m_NPBalckBoardRelationData.StringValue, this.stop, node);
                    break;
                case NP_BBValueType._Float:
                    this.mBlackboardConditionNode = new BlackboardCondition(m_NPBalckBoardRelationData.DicKey,
                        this.mOpe,
                        this.m_NPBalckBoardRelationData.FloatValue, this.stop, node);
                    break;
                case NP_BBValueType._Int:
                    this.mBlackboardConditionNode = new BlackboardCondition(m_NPBalckBoardRelationData.DicKey,
                        this.mOpe,
                        this.m_NPBalckBoardRelationData.IntValue, this.stop, node);
                    break;
                case NP_BBValueType._Bool:
                    this.mBlackboardConditionNode = new BlackboardCondition(m_NPBalckBoardRelationData.DicKey,
                        this.mOpe,
                        this.m_NPBalckBoardRelationData.Vector3Value, this.stop, node);
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