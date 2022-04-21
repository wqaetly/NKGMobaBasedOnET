using System;
using System.Collections.Generic;
using System.Reflection;

namespace GraphProcessor
{
    public static partial class UtilityAttribute
    {
        #region Class
        /// <summary> 保存类的特性，在编译时重载 </summary>
        private static readonly Dictionary<Type, Attribute[]> TypeAttributes = new Dictionary<Type, Attribute[]>();

        /// <summary> 尝试获取目标类型的目标特性 </summary>
        public static bool TryGetTypeAttribute<TAttributeType>(Type type, out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            if (TryGetTypeAttributes(type, out Attribute[] attributes))
            {
                foreach (var tempAttribute in attributes)
                {
                    attribute = tempAttribute as TAttributeType;
                    if (attribute != null)
                        return true;
                }
            }

            attribute = null;
            return false;
        }

        /// <summary> 尝试获取目标类型的所有特性 </summary>
        public static bool TryGetTypeAttributes(Type type, out Attribute[] attributes)
        {
            if (TypeAttributes.TryGetValue(type, out attributes))
                return attributes == null || attributes.Length > 0;

            attributes = type.GetCustomAttributes() as Attribute[];
            TypeAttributes[type] = attributes;
            return attributes == null || attributes.Length > 0;
        }
        #endregion

        #region Field
        /// <summary> 保存字段的特性，在编译时重载 </summary>
        private static readonly Dictionary<Type, Dictionary<string, Attribute[]>> TypeFieldAttributes =
            new Dictionary<Type, Dictionary<string, Attribute[]>>();

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetFieldInfoAttribute<TAttributeType>(FieldInfo fieldInfo,
            out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            attribute = null;
            if (fieldInfo == null) return false;
            if (TryGetFieldInfoAttributes(fieldInfo, out Attribute[] attributes))
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    attribute = attributes[i] as TAttributeType;
                    if (attribute != null)
                        return true;
                }
            }
            return false;
        }

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetFieldAttribute<TAttributeType>(Type type, string fieldName,
            out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            return TryGetFieldInfoAttribute(UtilityRefelection.GetFieldInfo(type, fieldName), out attribute);
        }

        /// <summary> 尝试获取目标类型的目标字段的所有特性 </summary>
        public static bool TryGetFieldInfoAttributes(FieldInfo fieldInfo,
            out Attribute[] attributes)
        {
            Dictionary<string, Attribute[]> fieldTypes;
            if (TypeFieldAttributes.TryGetValue(fieldInfo.DeclaringType, out fieldTypes))
            {
                if (fieldTypes.TryGetValue(fieldInfo.Name, out attributes))
                {
                    if (attributes != null && attributes.Length > 0)
                        return true;
                    return false;
                }
            }
            else
                fieldTypes = new Dictionary<string, Attribute[]>();

            attributes = fieldInfo.GetCustomAttributes(typeof(Attribute), true) as Attribute[];
            fieldTypes[fieldInfo.Name] = attributes;
            TypeFieldAttributes[fieldInfo.DeclaringType] = fieldTypes;
            if (attributes.Length > 0)
                return true;
            return false;
        }
        #endregion

        #region Method
        /// <summary> 保存方法的特性，在编译时重载 </summary>
        private static readonly Dictionary<Type, Dictionary<string, Attribute[]>> TypeMethodAttributes =
            new Dictionary<Type, Dictionary<string, Attribute[]>>();

        public static bool TryGetMethodInfoAttribute<TAttributeType>(MethodInfo methodInfo,
            out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            if (TryGetMethodInfoAttributes(methodInfo, out Attribute[] attributes))
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    attribute = attributes[i] as TAttributeType;
                    if (attribute != null)
                        return true;
                }
            }

            attribute = null;
            return false;
        }

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetMethodAttribute<TAttributeType>(Type type, string methodName,
            out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            return TryGetMethodInfoAttribute(UtilityRefelection.GetMethodInfo(type, methodName), out attribute);
        }

        /// <summary> 尝试获取目标类型的目标字段的所有特性 </summary>
        public static bool TryGetMethodInfoAttributes(MethodInfo methodInfo,
            out Attribute[] attributes)
        {
            Dictionary<string, Attribute[]> methodTypes;
            if (TypeFieldAttributes.TryGetValue(methodInfo.DeclaringType, out methodTypes))
            {
                if (methodTypes.TryGetValue(methodInfo.Name, out attributes))
                {
                    if (attributes != null && attributes.Length > 0)
                        return true;
                    return false;
                }
            }
            else
                methodTypes = new Dictionary<string, Attribute[]>();

            attributes = methodInfo.GetCustomAttributes(typeof(Attribute), true) as Attribute[];
            methodTypes[methodInfo.Name] = attributes;
            TypeFieldAttributes[methodInfo.DeclaringType] = methodTypes;
            if (attributes.Length > 0)
                return true;
            return false;
        }

        /// <summary> 尝试获取目标类型的目标字段的所有特性 </summary>
        public static bool TryGetMethodAttributes(Type type, string methodName,
            out Attribute[] attributes)
        {
            return TryGetMethodInfoAttributes(UtilityRefelection.GetMethodInfo(type, methodName), out attributes);
        }
        #endregion
    }
}