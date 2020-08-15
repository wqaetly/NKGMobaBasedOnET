//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 21:43:58
//------------------------------------------------------------

using NPBehave;
using Sirenix.OdinInspector;
using Action = System.Action;

namespace ETModel.TheDataContainsAction
{
    [Title("修改黑板值",TitleAlignment = TitleAlignments.Centered)]
    public class NP_ChangeBlackValue: NP_ClassForStoreAction
    {
        public NP_BlackBoardRelationData m_NPBalckBoardRelationData;

        public override Action GetActionToBeDone()
        {
            this.m_Action = this.ChangeBlackBoard;
            return this.m_Action;
        }

        public void ChangeBlackBoard()
        {
            //Log.Info($"修改黑板键{m_NPBalckBoardRelationData.DicKey} 黑板值类型 {m_NPBalckBoardRelationData.m_CompareType}  黑板值:Bool：{m_NPBalckBoardRelationData.theBoolValue}\n");
            this.m_NPBalckBoardRelationData.SetBlackBoardValue(Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid)
                    .GetComponent<NP_RuntimeTreeManager>()
                    .GetTreeByRuntimeID(this.RuntimeTreeID)
                    .GetBlackboard());
        }
    }
}