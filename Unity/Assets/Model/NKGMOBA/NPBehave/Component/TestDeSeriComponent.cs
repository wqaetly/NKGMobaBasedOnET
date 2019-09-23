//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 9:19:21
//------------------------------------------------------------


namespace ETModel
{
    [ObjectSystem]
    public class TestDeSeriComponentAwakeSystem: AwakeSystem<TestDeSeriComponent>
    {
        public override void Awake(TestDeSeriComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class TestDeSeriComponentUpdateSystem: UpdateSystem<TestDeSeriComponent>
    {
        public override void Update(TestDeSeriComponent self)
        {
            self.Update();
        }
    }
    
    public class TestDeSeriComponent: Component
    {
        private UserInputComponent userInputComponent;
        private Unit m_Unit;
        private NP_RuntimeTree m_NP_RuntimeTree;
        
        public void Awake()
        {
            this.userInputComponent = Game.Scene.GetComponent<UserInputComponent>();
            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();
            m_Unit = ComponentFactory.CreateWithId<Unit>(1000003);
            unitComponent.Add(m_Unit);
            m_Unit.AddComponent<NP_RuntimeTreeManager>();
            m_NP_RuntimeTree = NP_RuntimeTreeFactory.CreateNpRuntimeTree(m_Unit, 102841833095187);
            m_NP_RuntimeTree.m_NPRuntimeTreeRootNode.Start();
        }

        public void Update()
        {
            if (this.userInputComponent.QDown)
            {
                this.m_NP_RuntimeTree.GetBlackboard()["UserSkillInput"] = "Q";
            }
            if (this.userInputComponent.WDown)
            {
                this.m_NP_RuntimeTree.GetBlackboard()["UserSkillInput"] = "W";
            }
            if (this.userInputComponent.EDown)
            {
                this.m_NP_RuntimeTree.GetBlackboard()["UserSkillInput"] = "E";
            }
            if (this.userInputComponent.RDown)
            {
                this.m_NP_RuntimeTree.GetBlackboard()["UserSkillInput"] = "R";
            }
        }
    }
}