//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月19日 18:23:15
//------------------------------------------------------------

using NPBehave;

namespace ETModel
{
    [ObjectSystem]
    public class TestHelloworldAwakeSystem: AwakeSystem<TestHelloWorldComponent>
    {
        public override void Awake(TestHelloWorldComponent self)
        {
            self.Awake();
        }
    }
    
    [ObjectSystem]
    public class TestHelloWorldFixedUpdateSystem:FixedUpdateSystem<TestHelloWorldComponent>
    {
        public override void FixedUpdate(TestHelloWorldComponent self)
        {
            self.FixedUpdate();
        }
    }

    public class TestHelloWorldComponent: Component
    {
        public Root behaviorTree1;
        
        public Root behaviorTree2;

        public bool hasAddSecond;
        
        public void Awake()
        {
            behaviorTree1 = new Root(
                new Sequence(

                    // print out a message ...
                    new Action(() => Log.Info("Awake行为树")),

                    // ... and stay here until the `BlackboardValue`-node stops us because the toggled flag went false.
                    new WaitUntilStopped()
                )
            );
            behaviorTree1.Start();
        }

        public void FixedUpdate()
        {
            if (!this.hasAddSecond)
            {
                this.hasAddSecond = true;
                behaviorTree2 = new Root(

                    // toggle the 'toggled' blackboard boolean flag around every 500 milliseconds
                    new Service(0.5f, () => { behaviorTree2.Blackboard["foo"] = !behaviorTree2.Blackboard.Get<bool>("foo"); },

                        new Selector(

                            // Check the 'toggled' flag. Stops.IMMEDIATE_RESTART means that the Blackboard will be observed for changes 
                            // while this or any lower priority branches are executed. If the value changes, the corresponding branch will be
                            // stopped and it will be immediately jump to the branch that now matches the condition.
                            new BlackboardCondition("foo", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,

                                // when 'toggled' is true, this branch will get executed.
                                new Sequence(

                                    // print out a message ...
                                    new Action(this.TimeTest_foo),

                                    // ... and stay here until the `BlackboardValue`-node stops us because the toggled flag went false.
                                    new WaitUntilStopped()
                                )
                            ),

                            // when 'toggled' is false, we'll eventually land here
                            new Sequence(
                                new Action(this.TimeTest_bar),
                                new WaitUntilStopped()
                            )
                        )
                    )
                );
                behaviorTree2.Start();
            }
        }

        public void TimeTest_foo()
        {
            CodeTimeCostObserver.StartObserve();
            Log.Info("FixedUpdate行为树：foo");
        }

        public void TimeTest_bar()
        {
            CodeTimeCostObserver.StopObserve();
            Log.Info("FixedUpdate行为树：bar");
        }
    }
}