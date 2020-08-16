using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class ConfigAwakeSystem : AwakeSystem<ConfigComponent>
    {
        public override void Awake(ConfigComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class ConfigLoadSystem : LoadSystem<ConfigComponent>
    {
        public override void Load(ConfigComponent self)
        {
            self.Load();
        }
    }
    
    public static class ConfigComponentHelper
	{
		public static void Awake(this ConfigComponent self)
		{
			self.Load();
		}

		public static void Load(this ConfigComponent self)
		{
			AppType appType = StartConfigComponent.Instance.StartConfig.AppType;
			
			self.AllConfig.Clear();
			HashSet<Type> types = Game.EventSystem.GetTypes(typeof(ConfigAttribute));

			foreach (Type type in types)
			{
				object[] attrs = type.GetCustomAttributes(typeof (ConfigAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}
				
				ConfigAttribute configAttribute = attrs[0] as ConfigAttribute;
				// 只加载指定的配置
				if (!configAttribute.Type.Is(appType))
				{
					continue;
				}
				
				object obj = Activator.CreateInstance(type);

				ACategoryBase iCategory = obj as ACategoryBase;
				if (iCategory == null)
				{
					throw new Exception($"class: {type.Name} not inherit from ACategory");
				}
				iCategory.BeginInit();
				iCategory.EndInit();

				self.AllConfig[iCategory.ConfigType] = iCategory;
			}
		}

		public static T GetOne<T>(this ConfigComponent self) where T : IConfig
		{
			Type type = typeof(T);
			if (self.AllConfig.TryGetValue(type, out var aCategoryBase))
			{
				return ((ACategory<T>) aCategoryBase).GetOne();
			}
			else
			{
				throw new Exception($"ConfigComponent not found key: {type.FullName}");
			}

			return default;
		}

		public static T Get<T>(this ConfigComponent self,int id) where T : IConfig
		{
			Type type = typeof(T);
			if (self.AllConfig.TryGetValue(type, out var aCategoryBase))
			{
				return ((ACategory<T>) aCategoryBase).TryGet(id);
			}
			else
			{
				throw new Exception($"ConfigComponent not found key: {type.FullName}");
			}

			return default;
		}


		public static List<T> GetAll<T>(this ConfigComponent self) where T : IConfig
		{
			Type type = typeof(T);
			if (self.AllConfig.TryGetValue(type, out var aCategoryBase))
			{
				return ((ACategory<T>) aCategoryBase).GetAll();
			}
			else
			{
				throw new Exception($"ConfigComponent not found key: {type.FullName}");
			}

			return default;
		}
	}
}