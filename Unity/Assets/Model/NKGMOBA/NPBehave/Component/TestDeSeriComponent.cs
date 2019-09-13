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

    public class TestDeSeriComponent: Component
    {
        public void Awake()
        {
            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();
            Unit unit = ComponentFactory.CreateWithId<Unit>(1000003);
            unitComponent.Add(unit);
            unit.AddComponent<NP_RuntimeTreeManager>();
            NP_RuntimeTreeFactory.CreateNpRuntimeTree(unit, 102785120075787).m_NPRuntimeTreeRootNode.Start();
        }
    }
}