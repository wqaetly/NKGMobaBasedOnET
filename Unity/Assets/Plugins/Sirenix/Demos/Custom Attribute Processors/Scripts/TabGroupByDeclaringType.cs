#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;

    [TypeInfoBox(
        "This example demonstrates how you could use AttributeProcessors to arrange properties " +
        "into different groups, based on where they were declared.")]
    public class TabGroupByDeclaringType : Bar // Bar inherits from Foo
    {
        public string A, B, C;
    }

    public class TabifyFooResolver<T> : OdinAttributeProcessor<T> where T : Foo
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var inheritanceDistance = member.DeclaringType.GetInheritanceDistance(typeof(object));
            var tabName = member.DeclaringType.Name;
            attributes.Add(new TabGroupAttribute(tabName));
            attributes.Add(new PropertyOrderAttribute(-inheritanceDistance));
        }
    }
}
#endif
