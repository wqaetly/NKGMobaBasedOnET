using System;
using UnityEngine;

namespace ETModel
{
    public static class GameObjectHelper
    {
        /// <summary>
        /// 从ReferenceCollector中获取类型为T的目标对象
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetTargetObjectFromRC<T>(this GameObject gameObject, string key) where T : class
        {
            T result = gameObject.GetComponent<ReferenceCollector>().Get<T>(key);
            if (result == null)
            {
                Log.Error($"获取{gameObject.name}的ReferenceCollector key失败, key: {key}");
                return null;
            }

            return result;
        }

        /// <summary>
        /// 获取ReferenceCollector中key对应GameObject的Component，请确保key对应Object为GameObject且这个GameObject身上有T类型的组件
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T GetRCInternalComponent<T>(this GameObject gameObject, string key) where T : UnityEngine.Component
        {
            GameObject targetGo = gameObject.GetComponent<ReferenceCollector>().Get<GameObject>(key);
            if (targetGo == null)
            {
                Log.Error("RC Error：获取目标GameObject失败");
                return null;
            }

            T result = targetGo.GetComponent<T>();
            if (result == null)
            {
                Log.Error($"RC Error：获取的组件类型:{typeof (T)}不存在");
                return null;
            }

            return targetGo.GetComponent<T>();
        }
    }
}