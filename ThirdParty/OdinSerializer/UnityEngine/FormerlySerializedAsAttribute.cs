// Decompiled with JetBrains decompiler
// Type: UnityEngine.Serialization.FormerlySerializedAsAttribute
// Assembly: UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AF218701-FE32-4032-B521-DDD91F13662B
// Assembly location: D:\Work\Unity\2020.3.17f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll

using System;

namespace UnityEngine.Serialization
{
    /// <summary>
    ///   <para>Use this attribute to rename a field without losing its serialized value.</para>
    /// </summary>
    /// <footer><a href="https://docs.unity3d.com/2020.3/Documentation/ScriptReference/30_search.html?q=FormerlySerializedAsAttribute">`FormerlySerializedAsAttribute` on docs.unity3d.com</a></footer>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class FormerlySerializedAsAttribute : Attribute
    {
        private string m_oldName;

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <param name="oldName">The name of the field before renaming.</param>
        /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/Serialization.FormerlySerializedAsAttribute.html">External documentation for `FormerlySerializedAsAttribute`</a></footer>
        public FormerlySerializedAsAttribute(string oldName) => this.m_oldName = oldName;

        /// <summary>
        ///   <para>The name of the field before the rename.</para>
        /// </summary>
        /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/Serialization.FormerlySerializedAsAttribute-oldName.html">External documentation for `FormerlySerializedAsAttribute.oldName`</a></footer>
        public string oldName => this.m_oldName;
    }
}