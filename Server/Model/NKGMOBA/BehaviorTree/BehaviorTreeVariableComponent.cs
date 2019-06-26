using System;
using System.Collections.Generic;

namespace ETModel
{
	public class VariableComponent<T> : Component
	{
		public string Name;
		public T Value;

		public override void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			base.Dispose();

			Name = null;
			Value = default(T);
		}
	}

	public class BehaviorTreeVariableComponent : Component
	{
		public Dictionary<Type, Dictionary<string, Component>> BehaviorTreeVariables = new Dictionary<Type, Dictionary<string, Component>>();

		public override void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			base.Dispose();

			foreach (var item in BehaviorTreeVariables.Values)
			{
				foreach (var variable in item.Values)
				{
					variable.Dispose();
				}
			}

			BehaviorTreeVariables.Clear();
		}
	}
}