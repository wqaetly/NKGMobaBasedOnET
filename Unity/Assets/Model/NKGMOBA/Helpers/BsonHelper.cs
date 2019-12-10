//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年12月10日 12:53:38
//------------------------------------------------------------

using MongoDB.Bson.Serialization;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// Bson序列化反序列化辅助类
    /// </summary>
    public static class BsonHelper
    {
        /// <summary>
        /// 注册所有需要使用Bson序列化反序列化的结构体
        /// </summary>
        public static void RegisterStructSerializer()
        {
            BsonSerializer.RegisterSerializer(typeof(Vector3), new StructBsonSerialize<Vector3>());
        }
        
        /// <summary>
        /// 初始化BsonHelper
        /// </summary>
        public static void Init()
        {
            
        }
    }
}