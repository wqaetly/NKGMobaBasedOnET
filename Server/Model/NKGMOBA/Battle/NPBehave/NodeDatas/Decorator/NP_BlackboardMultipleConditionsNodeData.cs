//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月14日 21:50:07
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    [BoxGroup("黑板多条件节点配置"), GUIColor(0.961f, 0.902f, 0.788f, 1f)]
    [HideLabel]
    public class NP_BlackboardMultipleConditionsNodeData: NP_NodeDataBase
    {
        [HideInEditorMode]
        private BlackboardMultipleConditions m_BlackboardMultipleConditions;

        [LabelText("对比内容")]
        public List<MatchInfo> MatchInfos = new List<MatchInfo>();

        [LabelText("逻辑类型")]
        public MatchType MatchType;

        [LabelText("终止条件")]
        public Stops Stop = Stops.IMMEDIATE_RESTART;

        public override Decorator CreateDecoratorNode(long unitId, NP_RuntimeTree runtimeTree, Node node)
        {
            this.m_BlackboardMultipleConditions = new BlackboardMultipleConditions(this.MatchInfos, this.MatchType, this.Stop, node);
            //此处的value参数可以随便设，因为我们在游戏中这个value是需要动态改变的
            return this.m_BlackboardMultipleConditions;
        }

        public override Node NP_GetNode()
        {
            return this.m_BlackboardMultipleConditions;
        }
    }
}