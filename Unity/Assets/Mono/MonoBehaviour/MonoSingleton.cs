//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月12日 18:40:47
//------------------------------------------------------------

using UnityEngine;

namespace ET
{
    public abstract class MonoSingleton<T>: MonoBehaviour where T : MonoBehaviour
    {
        private static T s_Instance;
        
        public static T Instance
        {
            get
            {
                if (s_Instance != null)
                {
                    return s_Instance;
                }

                s_Instance = FindObjectOfType<T>();
                return s_Instance;
            }
        }
    }
}