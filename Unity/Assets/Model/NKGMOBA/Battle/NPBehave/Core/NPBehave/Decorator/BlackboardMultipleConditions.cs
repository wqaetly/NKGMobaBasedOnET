//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月14日 21:23:18
//------------------------------------------------------------

using System.Collections.Generic;
using ET;

namespace NPBehave
{
    /// <summary>
    /// 条件的匹配类型
    /// </summary>
    public enum MatchType: byte
    {
        /// <summary>
        /// 与
        /// </summary>
        AND,

        /// <summary>
        /// 或
        /// </summary>
        OR
    }

    /// <summary>
    /// 匹配信息
    /// </summary>
    public class MatchInfo
    {
        public NP_BlackBoardRelationData NPBalckBoardRelationData = new NP_BlackBoardRelationData() { WriteOrCompareToBB = true };
        public Operator Operator = Operator.IS_EQUAL;
    }

    /// <summary>
    /// 多条件的黑板条件结点
    /// </summary>
    public class BlackboardMultipleConditions: ObservingDecorator
    {
        private List<MatchInfo> matchInfos;
        private MatchType matchType;

        public BlackboardMultipleConditions(List<MatchInfo> matchInfos, MatchType matchType, Stops stopsOnChange,
        Node decoratee): base("BlackboardMultipleConditions",
            stopsOnChange, decoratee)
        {
            this.matchInfos = matchInfos;
            this.matchType = matchType;
            this.stopsOnChange = stopsOnChange;
        }

        override protected void StartObserving()
        {
            foreach (var matchInfo in this.matchInfos)
            {
                this.RootNode.Blackboard.AddObserver(matchInfo.NPBalckBoardRelationData.BBKey, onValueChanged);
            }
        }

        override protected void StopObserving()
        {
            foreach (var matchInfo in matchInfos)
            {
                this.RootNode.Blackboard.RemoveObserver(matchInfo.NPBalckBoardRelationData.BBKey, onValueChanged);
            }
        }

        private void onValueChanged(Blackboard.Type type, ANP_BBValue newValue)
        {
            Evaluate();
        }

        override protected bool IsConditionMet()
        {
            int realMatchCount = 0;
            foreach (var matchInfo in this.matchInfos)
            {
                if (CheckCondition(matchInfo.NPBalckBoardRelationData.BBKey, matchInfo.NPBalckBoardRelationData.NP_BBValue, matchInfo.Operator))
                {
                    realMatchCount++;
                }
            }

            if (this.matchType == MatchType.OR)
            {
                if (realMatchCount >= 1)
                {
                    return true;
                }

                return false;
            }

            if (matchType == MatchType.AND)
            {
                if (realMatchCount == this.matchInfos.Count)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public bool CheckCondition(string key, ANP_BBValue value, Operator op)
        {
            if (op == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!this.RootNode.Blackboard.Isset(key))
            {
                return op == Operator.IS_NOT_SET;
            }

            ANP_BBValue bbValue = this.RootNode.Blackboard.Get(key);

            return NP_BBValueHelper.Compare(value, bbValue, op);
        }
    }
}