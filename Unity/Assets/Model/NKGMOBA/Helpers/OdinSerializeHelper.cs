//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月20日 22:00:37
//------------------------------------------------------------

using System.IO;
using Sirenix.Serialization;

namespace ETModel
{
    /// <summary>
    /// Odin序列化反序列化工具
    /// </summary>
    public static class OdinSerializeHelper
    {
        /// <summary>
        /// 序列化一个类
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        /// <typeparam name="T"></typeparam>
        public static void Serialize<T>(T obj, string filePath)
                where T : class
        {
            if (obj == null)
            {
                return;
            }

            byte[] filebBytes = SerializationUtility.SerializeValue(obj, DataFormat.Binary);
            File.WriteAllBytes(filePath, filebBytes);
        }

        /// <summary>
        /// 反序列化一个类
        /// </summary>
        /// <param name="filePath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeSerialize<T>(string filePath)
                where T : class
        {
            byte[] filebBytes = File.ReadAllBytes(filePath);
            if (filebBytes.Length == 0) return null;
            return SerializationUtility.DeserializeValue<T>(filebBytes, DataFormat.Binary);
        }
    }
}