//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月14日 22:48:48
//------------------------------------------------------------

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ETModel
{
    public static class DeepCloneHelper
    {
        /// <summary>
        /// 深拷贝
        /// 注意：T必须标识为可序列化[Serializable]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T obj)
                where T : class
        {
            try
            {
                if (obj == null)
                {
                    return null;
                }

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    binaryFormatter.Serialize(stream, obj);
                    stream.Position = 0;
                    return (T) binaryFormatter.Deserialize(stream);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}