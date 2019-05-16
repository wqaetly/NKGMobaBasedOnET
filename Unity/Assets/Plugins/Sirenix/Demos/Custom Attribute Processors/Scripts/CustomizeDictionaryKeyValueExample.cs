#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using System.Collections.Generic;
    using Sirenix.OdinInspector.Editor;
    using System;
    using System.Reflection;

    public class CustomizeDictionaryKeyValueExample : SerializedMonoBehaviour
    {
        public Dictionary<MyKey, int> MyDictionary = new Dictionary<MyKey, int>();
    }

    public enum MyKey
    {
        A,
        B,
        C
    }

    public class AddKeyValueResolver : OdinAttributeProcessor<TempKeyValuePair<MyKey, int>>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Key")
            {
                attributes.Add(new EnumToggleButtonsAttribute());
            }
            else if (member.Name == "Value")
            {
                attributes.Add(new ProgressBarAttribute(0, 10));
            }
        }
    }

    public class EditKeyValueResolver : OdinAttributeProcessor<EditableKeyValuePair<MyKey, int>>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Key")
            {
                attributes.Add(new DisplayAsStringAttribute());
            }
            else if (member.Name == "Value")
            {
                attributes.Add(new RangeAttribute(0, 10));
            }
        }
    }
}
#endif
