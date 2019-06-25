using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class BehaviorTreeAwakeSystem : AwakeSystem<BehaviorTree, BehaviorDesigner.Runtime.BehaviorTree>
    {
        public override void Awake(BehaviorTree self, BehaviorDesigner.Runtime.BehaviorTree behaviorTree)
        {
            self.Awake(behaviorTree);
        }
    }

    public class BehaviorTree : Entity
    {
        public BehaviorDesigner.Runtime.BehaviorTree Behavior { get; private set; }

        public void Awake(BehaviorDesigner.Runtime.BehaviorTree behaviorTree)
        {
            Behavior = behaviorTree;
        }

        public void EnableBehaior()
        {
            Behavior?.EnableBehavior();
        }

        public void DisableBehavior()
        {
            Behavior?.DisableBehavior();
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();
            
            Behavior = null;
        }
    }
}