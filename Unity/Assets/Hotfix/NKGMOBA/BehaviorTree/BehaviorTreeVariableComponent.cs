using System;
using System.Collections.Generic;

namespace ETHotfix
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
        private Dictionary<Type, Dictionary<string, Component>> behaviorTreeVariables = new Dictionary<Type, Dictionary<string, Component>>();

        public VariableComponent<T> GetVariable<T>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception($"name can not be empty");
            }

            var type = typeof(T);

            if (!behaviorTreeVariables.ContainsKey(type))
            {
                behaviorTreeVariables[type] = new Dictionary<string, Component>();
            }

            var variables = behaviorTreeVariables[type];

            if (!variables.ContainsKey(name))
			{
				var variable = ComponentFactory.Create<VariableComponent<T>>();
				variable.Name = name;
				variables[name] = variable;
			}

            return (VariableComponent<T>)variables[name];
        }

        public VariableComponent<T> SetVariable<T>(string name, T value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception($"name can not be empty");
            }

            var type = typeof(T);

            if (!behaviorTreeVariables.ContainsKey(type))
            {
                behaviorTreeVariables[type] = new Dictionary<string, Component>();
            }

            var variables = behaviorTreeVariables[type];

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

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach(var item in behaviorTreeVariables.Values)
            {
                foreach(var variable in item.Values)
                {
                    variable.Dispose();
                }
            }

            behaviorTreeVariables.Clear();
        }
    }
}