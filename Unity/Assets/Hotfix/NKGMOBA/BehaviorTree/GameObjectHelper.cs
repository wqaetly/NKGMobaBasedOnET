using UnityEngine;

namespace ETHotfix
{
    public static class GameObjectHelper
    {
        public static T Ensure<T>(this GameObject gameObject) where T : UnityEngine.Component
        {
            if (gameObject)
            {
                var t = gameObject.GetComponent<T>();

                if (!t)
                {
                    t = gameObject.AddComponent<T>();
                }

                return t;
            }

            return null;
        }

        public static T Ensure<T>(this UnityEngine.Component component) where T : UnityEngine.Component
        {
            if (component)
            {
                return Ensure<T>(component.gameObject);
            }

            return null;
        }
    }
}
