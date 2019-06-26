using ETModel;

namespace ETHotfix
{
    // 测试行为树Demo
    [Event(EventIdType.TestBehavior)]
    public class TestBehavior_RunHandler : AEvent<string>
    {
        public override void Run(string name)
        {
			TestBehaviorTree(name);
        }

		private static void TestBehaviorTree(string name)
		{
			// 全局共享变量用法
			Game.Scene.GetComponent<BehaviorTreeVariableComponent>().SetVariable("全局变量", 1);

			var behaviorTree = BehaviorTreeFactory.Create(name); ;

			// 新增行为树共享变量用法
			var p1 = behaviorTree.GetComponent<BehaviorTreeVariableComponent>().GetVariable<int>("变量1");

			Log.Info($"行为树变量：{p1}");

			behaviorTree.GetComponent<BehaviorTreeVariableComponent>().SetVariable("变量1", 2);

			p1 = behaviorTree.GetComponent<BehaviorTreeVariableComponent>().GetVariable<int>("变量1");

			Log.Info($"行为树变量：{p1}");

			behaviorTree.GetComponent<BehaviorTreeVariableComponent>().SetVariable("变量2", "");
			behaviorTree.GetComponent<BehaviorTreeVariableComponent>().SetVariable("变量3", behaviorTree);
		}
	}
}
