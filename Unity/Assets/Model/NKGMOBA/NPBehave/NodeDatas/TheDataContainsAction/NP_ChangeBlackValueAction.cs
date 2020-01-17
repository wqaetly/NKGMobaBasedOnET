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
    public class NP_ChangeBlackValue: NP_ClassForStoreAction
    {
        [LabelText("黑板节点相关的数据")]
        public NP_BlackBoardRelationData m_NPBalckBoardRelationData;

        public override Action GetActionToBeDone()
        {
            this.m_Action = this.ChangeBlackBoard;
            return this.m_Action;
        }

        public void ChangeBlackBoard()
        {
            this.m_NPBalckBoardRelationData.SetBlackBoardValue(Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid)
                    .GetComponent<NP_RuntimeTreeManager>()
                    .GetTreeByRuntimeID(this.RuntimeTreeID)
                    .GetBlackboard());
            switch (m_NPBalckBoardRelationData.m_CompareType)
            {
                case CompareType._Bool:
                    Log.Info($"修改Bool黑板数据为{m_NPBalckBoardRelationData.theBoolValue}");
                    break;
                case CompareType._String:
                    Log.Info($"修改string黑板数据为{m_NPBalckBoardRelationData.theStringValue}");
                    break;
            }

        }
    }
}