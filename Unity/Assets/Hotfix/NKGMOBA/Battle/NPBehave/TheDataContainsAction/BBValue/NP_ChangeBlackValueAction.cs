//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 21:43:58
//------------------------------------------------------------

using NPBehave;
using Sirenix.OdinInspector;
using Action = System.Action;

namespace ET
{
    /// <summary>
    /// 这里默认修改自己的黑板值
    /// </summary>
    [Title("修改黑板值", TitleAlignment = TitleAlignments.Centered)]
    public class NP_ChangeBlackValueAction: NP_ClassForStoreAction
    {
        public NP_BlackBoardRelationData NPBalckBoardRelationData = new NP_BlackBoardRelationData() { WriteOrCompareToBB = true };

        public override Action GetActionToBeDone()
        {
            this.Action = this.ChangeBlackBoard;
            return this.Action;
        }

        public void ChangeBlackBoard()
        {
            //Log.Info($"修改黑板键{m_NPBalckBoardRelationData.DicKey} 黑板值类型 {m_NPBalckBoardRelationData.NP_BBValueType}  黑板值:Bool：{m_NPBalckBoardRelationData.BoolValue.GetValue()}\n");
            this.NPBalckBoardRelationData.SetBlackBoardValue(this.BelongtoRuntimeTree.GetBlackboard());
        }
    }
}