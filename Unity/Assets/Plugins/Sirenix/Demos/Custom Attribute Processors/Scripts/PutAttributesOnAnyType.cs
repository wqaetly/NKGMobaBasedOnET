#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Sirenix.OdinInspector.Editor;
    using UnityEngine;

    [TypeInfoBox(
        "With Odin 2.0 it is now possible to decorate types indirectly through the use of AttributeProcessors.\n" +
        "This means that you can even put attributes on types that you can't access the source code of.")]
    public class PutAttributesOnAnyType : MonoBehaviour
    {
        public Matrix4x4 Matrix;
    }

    public class Matrix4x4AttributeProcessor : OdinAttributeProcessor<Matrix4x4>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member is FieldInfo)
            {
                //attributes.Add(new IndentAttribute(-1));
                attributes.Add(new LabelWidthAttribute(30));
                attributes.Add(new HorizontalGroupAttribute(member.Name.Substring(0, 2)));
            }

            if (member.Name == "determinant")
            {
                //attributes.Add(new IndentAttribute(-1));
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
#endif
