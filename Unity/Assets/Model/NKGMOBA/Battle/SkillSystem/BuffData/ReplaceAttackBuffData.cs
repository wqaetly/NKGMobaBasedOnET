//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月24日 14:31:46
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 替换攻击流程
    /// </summary>
    public class ReplaceAttackBuffData: BuffDataBase
    {
        [BoxGroup("自定义项")]
        [LabelText("替换攻击")]
        public NP_BlackBoardRelationData AttackReplaceInfo = new NP_BlackBoardRelationData();

        [BoxGroup("自定义项")]
        [LabelText("替换取消攻击")]
        public NP_BlackBoardRelationData CancelReplaceInfo = new NP_BlackBoardRelationData();
    }
}