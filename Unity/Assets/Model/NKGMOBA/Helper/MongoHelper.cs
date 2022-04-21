using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace ET
{
    public class NKGMongoSerializationProvider : IBsonSerializationProvider
    {
        private static Dictionary<Type, IBsonSerializer> s_AllCustomBsonSerializer =
            new Dictionary<Type, IBsonSerializer>();


        public IBsonSerializer GetSerializer(Type type)
        {
            if (s_AllCustomBsonSerializer.TryGetValue(type, out var serializer))
            {
                return serializer;
            }
            else
            {
            }

            return type == typeof(DateTime) ? DateTimeSerializer.LocalInstance : null;
        }
    }

    /// <summary>
    /// Bson序列化反序列化辅助类
    /// </summary>
    public class MongoHelper
    {
        /// <summary>
        /// 这里之所以使用Unity特性执行一次静态初始化，是因为Mongo.Bson要求在使用Bson之前（不管是序列化还是反序列化）必须准备就绪所有的序列化器，否则就会出问题
        /// 比如我在执行序列化的时候没有注册序列化器，在执行反序列化的时候才想起来注册序列化器，那么就会报错
        /// </summary>
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        public static void Init()
        {
            
        }
        
        static MongoHelper()
        {
            // 自动注册IgnoreExtraElements
            ConventionPack conventionPack = new ConventionPack {new IgnoreExtraElementsConvention(true)};
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);
#if SERVER
            BsonSerializer.RegisterSerializer(typeof(System.Numerics.Vector2),
                new StructBsonSerialize<System.Numerics.Vector2>());
            BsonSerializer.RegisterSerializer(typeof(Vector2), new StructBsonSerialize<Vector2>());
            BsonSerializer.RegisterSerializer(typeof(Vector3), new StructBsonSerialize<Vector3>());
            BsonSerializer.RegisterSerializer(typeof(Vector4), new StructBsonSerialize<Vector4>());
            BsonSerializer.RegisterSerializer(typeof(Quaternion), new StructBsonSerialize<Quaternion>());
            BsonSerializer.RegisterSerializer(typeof(VTD_Id), new StructBsonSerialize<VTD_Id>());
            BsonSerializer.RegisterSerializer(typeof(VTD_EventId), new StructBsonSerialize<VTD_EventId>());
#elif ROBOT
			BsonSerializer.RegisterSerializer(typeof(Quaternion), new StructBsonSerialize<Quaternion>());
            BsonSerializer.RegisterSerializer(typeof(Vector3), new StructBsonSerialize<Vector3>());
            BsonSerializer.RegisterSerializer(typeof(Vector4), new StructBsonSerialize<Vector4>());
#else

            BsonSerializer.RegisterSerializer(typeof(System.Numerics.Vector2),
                new StructBsonSerialize<System.Numerics.Vector2>());
            BsonSerializer.RegisterSerializer(typeof(Vector2), new StructBsonSerialize<Vector2>());
            BsonSerializer.RegisterSerializer(typeof(Vector3), new StructBsonSerialize<Vector3>());
            BsonSerializer.RegisterSerializer(typeof(Vector4), new StructBsonSerialize<Vector4>());

            BsonSerializer.RegisterSerializer(typeof(VTD_Id), new StructBsonSerialize<VTD_Id>());
            BsonSerializer.RegisterSerializer(typeof(VTD_EventId), new StructBsonSerialize<VTD_EventId>());
#endif
            //技能配置反序列化相关(manually because these type cannot Automatically register)
            BsonClassMap.LookupClassMap(typeof(NP_BBValue_Int));
            BsonClassMap.LookupClassMap(typeof(NP_BBValue_Bool));
            BsonClassMap.LookupClassMap(typeof(NP_BBValue_Float));
            BsonClassMap.LookupClassMap(typeof(NP_BBValue_String));
            BsonClassMap.LookupClassMap(typeof(NP_BBValue_Vector3));
            BsonClassMap.LookupClassMap(typeof(NP_BBValue_Long));
            BsonClassMap.LookupClassMap(typeof(NP_BBValue_List_Long));

#if UNITY_EDITOR
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.Contains("Unity.Model") || assembly.FullName.Contains("Unity.ModelView") ||
                    assembly.FullName.Contains("Unity.Hotfix") || assembly.FullName.Contains("Unity.HotfixView"))
                {
                    types.AddRange(assembly.GetTypes());
                }
            }
#else
            var types = Game.EventSystem.GetTypes();
#endif

            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(Object)))
                {
                    continue;
                }

                if (type.IsGenericType)
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            RegisterAllSubClassForDeserialize(types);
        }

        /// <summary>
        /// 注册所有供反序列化的子类
        /// </summary>
        public static void RegisterAllSubClassForDeserialize(List<Type> allTypes)
        {
            List<Type> parenTypes = new List<Type>();
            List<Type> childrenTypes = new List<Type>();
            // registe by BsonDeserializerRegisterAttribute Automatically
            foreach (Type type in allTypes)
            {
                BsonDeserializerRegisterAttribute[] bsonDeserializerRegisterAttributes =
                    type.GetCustomAttributes(typeof(BsonDeserializerRegisterAttribute), false) as
                        BsonDeserializerRegisterAttribute[];
                if (bsonDeserializerRegisterAttributes.Length > 0)
                {
                    parenTypes.Add(type);
                }

                BsonDeserializerRegisterAttribute[] bsonDeserializerRegisterAttributes1 =
                    type.GetCustomAttributes(typeof(BsonDeserializerRegisterAttribute), true) as
                        BsonDeserializerRegisterAttribute[];
                if (bsonDeserializerRegisterAttributes1.Length > 0)
                {
                    childrenTypes.Add(type);
                }
            }

            foreach (Type type in childrenTypes)
            {
                foreach (var parentType in parenTypes)
                {
                    if (parentType.IsAssignableFrom(type) && parentType != type)
                    {
                        BsonClassMap.LookupClassMap(type);
                    }
                }
            }
        }

        public static string ToJson(object obj)
        {
            return obj.ToJson();
        }

        public static string ToJson(object obj, JsonWriterSettings settings)
        {
            return obj.ToJson(settings);
        }

        public static T FromJson<T>(string str)
        {
            try
            {
                return BsonSerializer.Deserialize<T>(str);
            }
            catch (Exception e)
            {
                throw new Exception($"{str}\n{e}");
            }
        }

        public static object FromJson(Type type, string str)
        {
            return BsonSerializer.Deserialize(str, type);
        }

        public static byte[] ToBson(object obj)
        {
            return obj.ToBson();
        }

        public static void ToStream(object message, MemoryStream stream)
        {
            using (BsonBinaryWriter bsonWriter = new BsonBinaryWriter(stream, BsonBinaryWriterSettings.Defaults))
            {
                BsonSerializationContext context = BsonSerializationContext.CreateRoot(bsonWriter);
                BsonSerializationArgs args = default;
                args.NominalType = typeof(object);
                IBsonSerializer serializer = BsonSerializer.LookupSerializer(args.NominalType);
                serializer.Serialize(context, args, message);
            }
        }

        public static object FromBson(Type type, byte[] bytes)
        {
            try
            {
                return BsonSerializer.Deserialize(bytes, type);
            }
            catch (Exception e)
            {
                throw new Exception($"from bson error: {type.Name}", e);
            }
        }

        public static object FromBson(Type type, byte[] bytes, int index, int count)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(bytes, index, count))
                {
                    return BsonSerializer.Deserialize(memoryStream, type);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"from bson error: {type.Name}", e);
            }
        }

        public static object FromStream(Type type, Stream stream)
        {
            try
            {
                return BsonSerializer.Deserialize(stream, type);
            }
            catch (Exception e)
            {
                throw new Exception($"from bson error: {type.Name}", e);
            }
        }

        public static T FromBson<T>(byte[] bytes)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    return (T) BsonSerializer.Deserialize(memoryStream, typeof(T));
                }
            }
            catch (Exception e)
            {
                throw new Exception($"from bson error: {typeof(T).Name}", e);
            }
        }

        public static T FromBson<T>(byte[] bytes, int index, int count)
        {
            return (T) FromBson(typeof(T), bytes, index, count);
        }

        public static T Clone<T>(T t)
        {
            return FromBson<T>(ToBson(t));
        }
    }
}