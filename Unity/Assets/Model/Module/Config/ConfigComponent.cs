using System;
using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class ConfigComponentAwakeSystem: AwakeSystem<ConfigComponent>
    {
        public override void Awake(ConfigComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class ConfigComponentLoadSystem: LoadSystem<ConfigComponent>
    {
        public override void Load(ConfigComponent self)
        {
            self.Load();
        }
    }

    /// <summary>
    /// Config组件会扫描所有的有ConfigAttribute标签的配置,加载进来
    /// </summary>
    public class ConfigComponent: Component
    {
        private Dictionary<Type, ACategoryBase> allConfig = new Dictionary<Type, ACategoryBase>();

        public void Awake()
        {
            this.Load();
        }

        public void Load()
        {
            this.allConfig.Clear();
            HashSet<Type> types = Game.EventSystem.GetTypes(typeof (ConfigAttribute));

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (ConfigAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                ConfigAttribute configAttribute = attrs[0] as ConfigAttribute;
                // 只加载指定的配置
                if (!configAttribute.Type.Is(AppType.ClientM))
                {
                    continue;
                }

                object obj = Activator.CreateInstance(type);

                ACategoryBase iCategoryBase = obj as ACategoryBase;
                if (iCategoryBase == null)
                {
                    throw new Exception($"class: {type.Name} not inherit from ACategory");
                }

                iCategoryBase.BeginInit();
                iCategoryBase.EndInit();

                this.allConfig[iCategoryBase.ConfigType] = iCategoryBase;
            }
        }

        public T GetOne<T>() where T : IConfig
        {
            Type type = typeof (T);
            if (this.allConfig.TryGetValue(type, out var aCategoryBase))
            {
                return ((ACategory<T>) aCategoryBase).GetOne();
            }
            else
            {
                throw new Exception($"ConfigComponent not found key: {type.FullName}");
            }

            return default;
        }

        public T Get<T>(int id) where T : IConfig
        {
            Type type = typeof (T);
            if (this.allConfig.TryGetValue(type, out var aCategoryBase))
            {
                return ((ACategory<T>) aCategoryBase).TryGet(id);
            }
            else
            {
                throw new Exception($"ConfigComponent not found key: {type.FullName}");
            }

            return default;
        }

        public ACategoryBase Get(Type type)
        {
            if (this.allConfig.TryGetValue(type, out var aCategoryBase))
            {
                return aCategoryBase;
            }
            else
            {
                throw new Exception($"ConfigComponent not found key: {type.FullName}");
            }

            return default;
        }

        public List<T> GetAll<T>() where T : IConfig
        {
            Type type = typeof (T);
            if (this.allConfig.TryGetValue(type, out var aCategoryBase))
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