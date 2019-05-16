#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Sirenix.OdinInspector.Editor;
    using UnityEngine;

    public class BasicAttributeProcessorExample : MonoBehaviour
    {
        public MyCustomClass Processed = new MyCustomClass();
    }

    [Serializable]
    public class MyCustomClass
    {
        public ScaleMode Mode;
        public float Size;
    }

    // This AttributeProcessor will be found and used to processor attributes for the MyCustomClass class.
    public class MyResolvedClassAttributeProcessor : OdinAttributeProcessor<MyCustomClass>
    {
        // This method will be called for any field or propety of the type MyCustomClass.
        // In this example, this will be run for the BasicAttributeProcessorExample.Processed field.
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InfoBoxAttribute("Dynamically added attributes."));
            attributes.Add(new InlinePropertyAttribute());
        }

        // This method will be called for any members of the type MyCustomClass.
        // In this example, this will be run for the fields MyCustomClass.Mode and MyCustomClass.Size.
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
            attributes.Add(new BoxGroupAttribute("Box", showLabel: false));

            if (member.Name == "Mode")
            {
                attributes.Add(new EnumToggleButtonsAttribute());
            }
            else if (member.Name == "Size")
            {
                attributes.Add(new RangeAttribute(0, 5));
            }
        }
    }
}
#endif
