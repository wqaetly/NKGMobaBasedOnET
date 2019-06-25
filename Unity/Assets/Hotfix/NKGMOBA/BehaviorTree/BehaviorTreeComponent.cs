using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
    [ObjectSystem]
    public class BehaviorTreeComponentAwakeSystem : AwakeSystem<BehaviorTreeComponent>
    {
        public override void Awake(BehaviorTreeComponent self)
        {
            self.Awake();
        }
    }

    public class BehaviorTreeComponent : Component
    {
        private Dictionary<HotfixAction, Component> behaviorTreeActionComponents = new Dictionary<HotfixAction, Component>();
        private Dictionary<HotfixComposite, Component> behaviorTreeCompositeComponents = new Dictionary<HotfixComposite, Component>();
        private Dictionary<HotfixConditional, Component> behaviorTreeConditionalComponents = new Dictionary<HotfixConditional, Component>();
        private Dictionary<HotfixDecorator, Component> behaviorTreeDecoratorComponents = new Dictionary<HotfixDecorator, Component>();

        public void Awake()
        {
            var behavior = GetParent<BehaviorTree>();

            if (behavior.IsEmptyOrDisposed())
            {
                return;
            }

            var behaviorTree = behavior.Behavior;

            if (!behaviorTree)
            {
                return;
            }

            behaviorTree.StartWhenEnabled = false;
            behaviorTree.ResetValuesOnRestart = false;

            var behaviorTreeController = behaviorTree.Ensure<BehaviorTreeController>();

            behaviorTreeController.Init();

            BindHotfixActions(behaviorTreeController, behavior.Entity);
            BindHotfixComposites(behaviorTreeController, behavior.Entity);
            BindHotfixConditionals(behaviorTreeController, behavior.Entity);
            BindHotfixDecorators(behaviorTreeController, behavior.Entity);

            behaviorTree.EnableBehavior();
        }

        private void BindHotfixActions(BehaviorTreeController tasks, Entity parent)
        {
            foreach (var hotfixAction in tasks.hotfixActions)
            {
                var component = BehaviorTreeFactory.Create(parent, hotfixAction);

                if (component != null)
                {
                    behaviorTreeActionComponents.Add(hotfixAction, component);
                }
            }
        }

        private void BindHotfixComposites(BehaviorTreeController tasks, Entity parent)
        {
            foreach (var hotfixComposite in tasks.hotfixComposites)
            {
                var component = BehaviorTreeFactory.Create(parent, hotfixComposite);

                if (component != null)
                {
                    behaviorTreeCompositeComponents.Add(hotfixComposite, component);
                }
            }
        }

        private void BindHotfixConditionals(BehaviorTreeController tasks, Entity parent)
        {
            foreach (var hotfixConditional in tasks.hotfixConditionals)
            {
                var component = BehaviorTreeFactory.Create(parent, hotfixConditional);

                if (component != null)
                {
                    behaviorTreeConditionalComponents.Add(hotfixConditional, component);
                }
            }
        }

        private void BindHotfixDecorators(BehaviorTreeController tasks, Entity parent)
        {
            foreach (var hotfixDecorator in tasks.hotfixDecorators)
            {
                var component = BehaviorTreeFactory.Create(parent, hotfixDecorator);

                if (component != null)
                {
                    behaviorTreeDecoratorComponents.Add(hotfixDecorator, component);
                }
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();
            
            foreach (var item in behaviorTreeActionComponents)
            {
                item.Value.Dispose();
            }

            behaviorTreeActionComponents.Clear();

            foreach (var item in behaviorTreeCompositeComponents)
            {
                item.Value.Dispose();
            }

            behaviorTreeCompositeComponents.Clear();

            foreach (var item in behaviorTreeConditionalComponents)
            {
                item.Value.Dispose();
            }

            behaviorTreeConditionalComponents.Clear();

            foreach (var item in behaviorTreeDecoratorComponents)
            {
                item.Value.Dispose();
            }

            behaviorTreeDecoratorComponents.Clear();
        }
    }
}