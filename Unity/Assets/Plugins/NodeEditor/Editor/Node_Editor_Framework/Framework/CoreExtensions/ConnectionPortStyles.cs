using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework
{
    /// <summary>
    /// 连线端口相关操作
    /// </summary>
    public static class ConnectionPortStyles
    {
        /// <summary>
        /// 连线端口风格，主要用于绘制
        /// </summary>
        private static Dictionary<string, ConnectionPortStyle> s_ConnectionPortStyles;

        /// <summary>
        /// 连线Value类型，主要用于取值
        /// </summary>
        private static Dictionary<string, ValueConnectionType> s_ConnectionValueTypes;

        /// <summary>
        /// Fetches every ConnectionPortStyle, ConnectionKnobStyle or ValueConnectionType declaration in the script assemblies to provide the framework with custom connection port styles
        /// </summary>
        public static void FetchConnectionPortStyles()
        {
            s_ConnectionPortStyles = new Dictionary<string, ConnectionPortStyle>();
            s_ConnectionValueTypes = new Dictionary<string, ValueConnectionType>();
            foreach (Type type in ReflectionUtility.getSubTypes(typeof (ConnectionPortStyle)))
            {
                ConnectionPortStyle portStyle = (ConnectionPortStyle) Activator.CreateInstance(type);
                if (portStyle == null)
                    throw new UnityException("Error with Connection Port Style Declaration " + type.FullName);
                if (!portStyle.isValid())
                    throw new Exception(type.BaseType.Name + " declaration " + portStyle.Identifier + " is invalid!");
                if (s_ConnectionPortStyles.ContainsKey(portStyle.Identifier))
                    throw new Exception("Duplicate ConnectionPortStyle declaration " + portStyle.Identifier + "!");

                s_ConnectionPortStyles.Add(portStyle.Identifier, portStyle);
                if (type.IsSubclassOf(typeof (ValueConnectionType)))
                    s_ConnectionValueTypes.Add(portStyle.Identifier, (ValueConnectionType) portStyle);
                if (!portStyle.isValid())
                    Debug.LogError("Style " + portStyle.Identifier + " is invalid!");
            }
        }

        /// <summary>
        /// Gets the ValueConnectionType type the specified type name representates or creates it if not defined
        /// </summary>
        public static Type GetValueType(string typeName)
        {
            return ((ValueConnectionType) GetPortStyle(typeName, typeof (ValueConnectionType))).Type ?? typeof (void);
        }

        /// <summary>
        /// Gets the ConnectionPortStyle for the specified style name or creates it if not defined
        /// </summary>
        public static ConnectionPortStyle GetPortStyle(string styleName, Type baseStyleClass = null)
        {
            if (s_ConnectionPortStyles == null || s_ConnectionPortStyles.Count == 0)
                FetchConnectionPortStyles();
            if (baseStyleClass == null || !typeof (ConnectionPortStyle).IsAssignableFrom(typeof (ConnectionPortStyle)))
                baseStyleClass = typeof (ConnectionPortStyle);
            ConnectionPortStyle portStyle;
            if (!s_ConnectionPortStyles.TryGetValue(styleName, out portStyle))
            {
                // No port style with the exact name exists
                if (typeof (ValueConnectionType).IsAssignableFrom(baseStyleClass))
                {
                    // A ValueConnectionType is searched, try by type name
                    Type type = Type.GetType(styleName);
                    if (type == null) // No type matching the name found either
                    {
                        Debug.LogError("No ValueConnectionType could be found or created with name '" + styleName + "'!");
                        return null;
                    }
                    else // Matching type found, search or create type data based on type
                        portStyle = GetValueConnectionType(type);
                }
                else
                {
                    portStyle = (ConnectionPortStyle) Activator.CreateInstance(baseStyleClass, styleName);
                    s_ConnectionPortStyles.Add(styleName, portStyle);
                    Debug.LogWarning("Created style from name " + styleName + "!");
                }
            }

            if (!baseStyleClass.IsAssignableFrom(portStyle.GetType()))
                throw new Exception("Cannot use Connection Style: '" + styleName + "' is not of type " + baseStyleClass.Name + "!");
            if (!portStyle.isValid())
                Debug.LogError("Fetched style " + portStyle.Identifier + " is invalid!");
            return portStyle;
        }

        /// <summary>
        /// Gets the ValueConnectionType for the specified type or creates it if not defined
        /// </summary>
        public static ValueConnectionType GetValueConnectionType(Type type)
        {
            if (s_ConnectionPortStyles == null || s_ConnectionPortStyles.Count == 0)
                FetchConnectionPortStyles();
            ValueConnectionType valueType =
                    s_ConnectionValueTypes.Values.FirstOrDefault((ValueConnectionType data) => data.isValid() && data.Type == type);
            if (valueType == null) // ValueConnectionType with type does not exist, create it
            {
                valueType = new ValueConnectionType(type);
                s_ConnectionPortStyles.Add(type.FullName, valueType);
                s_ConnectionValueTypes.Add(type.FullName, valueType);
            }

            return valueType;
        }
    }
}