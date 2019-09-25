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
        public Blackboard theBlackBoardWillBedo;

        [LabelText("黑板相关数据")]
        public NP_BlackBoardRelationData m_NPBalckBoardRelationData;

        public override Action GetActionToBeDone()
        {
            this.m_Action = this.TestChangeBlackBoard;
            return this.m_Action;
        }

        public void TestChangeBlackBoard()
        {
            theBlackBoardWillBedo = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid).GetComponent<NP_RuntimeTreeManager>()
                    .GetTree(this.RuntimeTreeID)
                    .GetBlackboard();
            switch (m_NPBalckBoardRelationData.m_CompareType)
            {
                case CompareType._String:
                    theBlackBoardWillBedo[m_NPBalckBoardRelationData.DicKey] = this.m_NPBalckBoardRelationData.theStringValue;
                    break;
                case CompareType._Float:
                    theBlackBoardWillBedo[m_NPBalckBoardRelationData.DicKey] = this.m_NPBalckBoardRelationData.theFloatValue;
                    break;
                case CompareType._Int:
                    theBlackBoardWillBedo[m_NPBalckBoardRelationData.DicKey] = this.m_NPBalckBoardRelationData.theIntValue;
                    break;
            }


        }
    }
}