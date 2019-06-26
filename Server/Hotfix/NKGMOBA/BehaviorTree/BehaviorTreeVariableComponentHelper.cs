using ETModel;
using System;
using System.Collections.Generic;

namespace ETHotfix
{
    public static class BehaviorTreeVariableComponentHelper
	{
		public static VariableComponent<T> GetVariable<T>(this BehaviorTreeVariableComponent self, string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new Exception($"name can not be empty");
			}

			var type = typeof(T);

			if (!self.BehaviorTreeVariables.ContainsKey(type))
			{
				self.BehaviorTreeVariables[type] = new Dictionary<string, Component>();
			}

			var variables = self.BehaviorTreeVariables[type];

			if (!variables.ContainsKey(name))
			{
				var variable = ComponentFactory.Create<VariableComponent<T>>();
				variable.Name = name;
				variables[name] = variable;
			}

			return (VariableComponent<T>)variables[name];
		}

		public static VariableComponent<T> SetVariable<T>(this BehaviorTreeVariableComponent self, string name, T value)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new Exception($"name can not be empty");
			}

			var type = typeof(T);

			if (!self.BehaviorTreeVariables.ContainsKey(type))
			{
				self.BehaviorTreeVariables[type] = new Dictionary<string, Component>();
			}

			var variables = self.BehaviorTreeVariables[type];

			if (!variables.ContainsKey(name))
			{
				var variable = ComponentFactory.Create<VariableComponent<T>>();
				variable.Name = name;
				variables[name] = variable;
			}

			var result = (VariableComponent<T>)variables[name];
			result.Value = value;

			return result;
		}

	}
}
