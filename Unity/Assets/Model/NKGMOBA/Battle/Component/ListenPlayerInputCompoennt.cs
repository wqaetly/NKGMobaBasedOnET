//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 9:19:21
//------------------------------------------------------------


namespace ETModel
{
    [ObjectSystem]
    public class TestDeSeriComponentAwakeSystem: AwakeSystem<ListenPlayerInputCompoennt>
    {
        public override void Awake(ListenPlayerInputCompoennt self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class TestDeSeriComponentUpdateSystem: UpdateSystem<ListenPlayerInputCompoennt>
    {
        public override void Update(ListenPlayerInputCompoennt self)
        {
            self.Update();
        }
    }
    
    public class ListenPlayerInputCompoennt: Component
    {
        private UserInputComponent userInputComponent;
        private NP_RuntimeTree m_NP_RuntimeTree;
        
        public void Awake()
        {
            this.userInputComponent = Game.Scene.GetComponent<UserInputComponent>();
            this.m_NP_RuntimeTree = this.Entity.GetComponent<NP_RuntimeTreeManager>().GetTreeByPrefabID(102859210555481);
        }

        public void Update()
        {
            if (this.userInputComponent.QDown)
            {
                this.m_NP_RuntimeTree.GetBlackboard()["PlayerInput"] = "QInput";
            }
            if (this.userInputComponent.WDown)
            {
                this.m_NP_RuntimeTree.GetBlackboard()["PlayerInput"] = "WInput";
            }
            if (this.userInputComponent.EDown)
            {
                this.m_NP_RuntimeTree.GetBlackboard()["PlayerInput"] = "EInput";
            }
            if (this.userInputComponent.RDown)
            {
                this.m_NP_RuntimeTree.GetBlackboard()["PlayerInput"] = "RInput";
            }
        }
    }
}