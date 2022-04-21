using System;
using System.Collections.Generic;
using System.Reflection;

namespace GraphProcessor
{

    public static partial class UtilityRefelection
    {
        static readonly Dictionary<string, Assembly> AssemblyCache = new Dictionary<string, Assembly>();
        static readonly Dictionary<string, Type> FullNameTypeCache = new Dictionary<string, Type>();
        static readonly List<Type> AllTypeCache = new List<Type>();
        static readonly Dictionary<Type, IEnumerable<Type>> ChildrenTypeCache = new Dictionary<Type, IEnumerable<Type>>();

        static UtilityRefelection()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("Unity")) continue;
                if (!assembly.FullName.Contains("Version=0.0.0")) continue;
                AllTypeCache.AddRange(assembly.GetTypes());
            }
        }

        public static List<Type> GetAllTypes()
        {
            return AllTypeCache;
        }
        
        public static IEnumerable<Type> GetChildrenTypes<T>()
        {
            return GetChildrenTypes(typeof(T));
        }

        public static IEnumerable<Type> GetChildrenTypes(Type baseType)
        {
            if (ChildrenTypeCache.TryGetValue(baseType, out IEnumerable<Type> childrenTypes))
            {
                foreach (var item in childrenTypes)
                {
                    yield return item;
                }
                yield break;
            }

            ChildrenTypeCache[baseType] = childrenTypes = BuildCache(baseType);
            foreach (var type in childrenTypes)
            {
                yield return type;
            }
        }

        private static IEnumerable<Type> BuildCache(Type baseType)
        {
            foreach (var type in AllTypeCache)
            {
                if (baseType.IsAssignableFrom(type))
                    yield return type;
            }
            //var selfAssembly = Assembly.GetAssembly(_baseType);
            //if (selfAssembly.FullName.StartsWith("Assembly-CSharp") && !selfAssembly.FullName.Contains("-firstpass"))
            //{
            //    foreach (var type in selfAssembly.GetTypes())
            //    {
            //        if (!type.IsAbstract && _baseType.IsAssignableFrom(type))
            //        {
            //            yield return type;
            //        }
            //    }
            //}
            //else
            //{
            //    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //    foreach (Assembly assembly in assemblies)
            //    {
            //        if (assembly.FullName.StartsWith("Unity")) continue;
            //        if (!assembly.FullName.Contains("Version=0.0.0")) continue;
            //        foreach (var type in assembly.GetTypes())
            //        {
            //            if (type != null && !type.IsAbstract && _baseType.IsAssignableFrom(type))
            //                yield return type;
            //        }
            //    }
            //}
        }

        public static Assembly LoadAssembly(string assemblyString)
        {
            Assembly assembly;
            if (!AssemblyCache.TryGetValue(assemblyString, out assembly))
                AssemblyCache[assemblyString] = assembly = Assembly.Load(assemblyString);
            return assembly;
        }

        public static Type GetType(string fullName, string assemblyString)
        {
            Type type;
            if (FullNameTypeCache.TryGetValue(fullName, out type))
                return type;
            Assembly assembly = LoadAssembly(assemblyString);
            if (assembly == null) return null;
            foreach (var tempType in assembly.GetTypes())
            {
                FullNameTypeCache[tempType.FullName] = tempType;
            }
            if (FullNameTypeCache.TryGetValue(fullName, out type))
                return type;
            return null;
        }

        #region GetMemberInfo
        static Dictionary<Type, List<FieldInfo>> _typeFieldInfoCache = new Dictionary<Type, List<FieldInfo>>();

        /// <summary> 获取字段，包括基类的私有字段 </summary>
        public static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            return GetFieldInfos(type).Find(f => f.Name == fieldName);
        }

        public static List<FieldInfo> GetFieldInfos(Type type)
        {
            if (_typeFieldInfoCache.TryGetValue(type, out List<FieldInfo> fieldInfos))
                return fieldInfos;
            _typeFieldInfoCache[type] = fieldInfos = new List<FieldInfo>(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            // 获取类包含的所有字段(包含私有)
            while ((type = type.BaseType) != null)
            {
                fieldInfos.InsertRange(0, type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            }
            return fieldInfos;
        }

        public static IEnumerable<FieldInfo> GetFieldInfos(Type type, Func<FieldInfo, bool> patern)
        {
            foreach (var field in GetFieldInfos(type))
            {
                if (patern(field))
                    yield return field;
            }
        }

        /// <summary> 获取方法，包括基类的私有方法 </summary>
        public static MethodInfo GetMethodInfo(Type type, string methodName)
        {
            // 如果第一次没有找到，那么这个变量可能是基类的私有字段
            MethodInfo method = type.GetMethod(methodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            // 只搜索基类的私有方法
            while (method == null && (type = type.BaseType) != null)
            {
                method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            }

            return method;
        }

        #endregion
    }
}