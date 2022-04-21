using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ProtoBuf;
using ProtoBuf.Meta;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// PB基类注册器，不用再手写各种ProtoInclude了
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ProtobufBaseTypeRegisterAttribute : Attribute
    {
    }

    public static class ProtobufHelper
    {
        static ProtobufHelper()
        {
        }

        public static void Init()
        {
            List<Type> types = Game.EventSystem.GetTypes();

            foreach (Type type in types)
            {
                if (type.GetCustomAttributes(typeof(ProtoContractAttribute), false).Length == 0 &&
                    type.GetCustomAttributes(typeof(ProtoMemberAttribute), false).Length == 0 &&
                    type.GetCustomAttributes(typeof(ProtobufBaseTypeRegisterAttribute), false).Length == 0)
                {
                    continue;
                }

                PBType.RegisterType(type.FullName, type);

                if (type.GetCustomAttribute<ProtobufBaseTypeRegisterAttribute>() != null)
                {
                    RuntimeTypeModel.Default.Add(type, true);

                    int flag = 100;
                    foreach (var type1 in types)
                    {
                        if (type1 != type && type1.IsSubclassOf(type))
                        {
                            RuntimeTypeModel.Default[type].AddSubType(flag++, type1);
                        }
                    }
                }
            }
        }

        public static object FromBytes(Type type, byte[] bytes, int index, int count)
        {
            using (MemoryStream stream = new MemoryStream(bytes, index, count))
            {
                return ProtoBuf.Serializer.Deserialize(type, stream);
            }
        }

        public static T FromBytes<T>(byte[] bytes, int index, int count)
        {
            using (MemoryStream stream = new MemoryStream(bytes, index, count))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        }


        public static byte[] ToBytes(object message)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, message);
                return stream.ToArray();
            }
        }

        public static void ToStream(object message, MemoryStream stream)
        {
            ProtoBuf.Serializer.Serialize(stream, message);
        }

        public static object FromStream(Type type, MemoryStream stream)
        {
            return ProtoBuf.Serializer.Deserialize(type, stream);
        }
    }
}