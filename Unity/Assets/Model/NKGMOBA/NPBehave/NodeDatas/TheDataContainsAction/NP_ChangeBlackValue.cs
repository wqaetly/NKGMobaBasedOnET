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

        [LabelText("将要被改变的黑板键")]
        public string theKey;

        [LabelText("将要对比的值类型")]
        public CompareType m_CompareType;

        [ShowIf("m_CompareType", CompareType._String)]
        public string theValue_string;

        [ShowIf("m_CompareType", CompareType._Int)]
        public float theValue_int;

        [ShowIf("m_CompareType", CompareType._Float)]
        public int theValue_float;

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
            switch (m_CompareType)
            {
                case CompareType._String:
                    theBlackBoardWillBedo[theKey] = this.theValue_string;
                    break;
                case CompareType._Float:
                    theBlackBoardWillBedo[theKey] = this.theValue_float;
                    break;
                case CompareType._Int:
                    theBlackBoardWillBedo[theKey] = this.theValue_int;
                    break;
            }


        }
    }
}