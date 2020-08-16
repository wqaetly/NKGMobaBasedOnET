using System;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using Sirenix.Utilities;  
#endif


namespace ETModel
{
    public static class ObjectHelper
    {
        public static void Swap<T>(ref T t1, ref T t2)
        {
            T t3 = t1;
            t1 = t2;
            t2 = t3;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 对象转换为字典
        /// </summary>
        /// <param name="obj">待转化的对象</param>
        /// <returns></returns>
        public static Dictionary<string, object> ObjectToMap(this object obj, int reflectDepth = 0)
        {
            if (obj == null) return null;
            Dictionary<string, object> map = new Dictionary<string, object>();
            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型
            //是数组就不算一次递归深度
            if (t.IsSubclassOf(typeof(Array)))
            {
                int index = 0;
                foreach (var item in obj as Array)
                {
                    map[$"{index++}"] = item.ObjectToMap(reflectDepth);
                }
            }
            else
            {
                reflectDepth--;
                FieldInfo[] pi = t.GetFields(BindingFlags.NonPublic | BindingFlags.Public |
                    BindingFlags.Instance); // 获取当前type字段
                foreach (FieldInfo p in pi)
                {
                    if (reflectDepth >= 0)
                    {
                        map[p.GetNiceName()] = p.GetValue(obj).ObjectToMap(reflectDepth);
                    }
                    else
                    {
                        map[p.GetNiceName()] = p.GetValue(obj);
                    }
                }
            }

            return map;
        }
#endif
    }
}