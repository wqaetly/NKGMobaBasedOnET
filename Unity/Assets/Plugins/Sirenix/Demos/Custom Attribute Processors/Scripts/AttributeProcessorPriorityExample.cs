#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;
    using Sirenix.OdinInspector.Editor;
    using UnityEngine;

    [TypeInfoBox("This example demonstrates how AttributeProcessors are ordered by priority.")]
    public class AttributeProcessorPriorityExample : MonoBehaviour
    {
        public PrioritizedProcessed Processed;
    }

    [Serializable]
    public class PrioritizedProcessed
    {
        public int A;
    }

    // This processor has the highest priority and is therefore executed first.
    // It adds a Range attribute the child members of the PrioritizedResolved class.
    // The range attribute will be removed by the SecondAttributeProcessor.
    [ResolverPriority(100)]
    public class FirstAttributeProcessor : OdinAttributeProcessor<PrioritizedProcessed>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            attributes.Add(new BoxGroupAttribute("First"));
            attributes.Add(new RangeAttribute(0, 10));
        }
    }

    // This processor has a default priority of 0, and is therefore executed second.
    // It clears the attributes list and therefore removes all attributes from the members of the PrioritizedResolved class.
    public class SecondAttributeProcessor : OdinAttributeProcessor<PrioritizedProcessed>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            attributes.RemoveAttributeOfType<RangeAttribute>();

            var boxGroup = attributes.OfType<BoxGroupAttribute>().FirstOrDefault();
            boxGroup.GroupName = boxGroup.GroupName + " - Second";
        }
    }

    // This processor has the lowest priority and is therefore executed last.
    // It adds a BoxGroup to the child members of the PrioritizedResolved class.
    // Since this is executed after the SecondAttributeProcessor, the BoxGroup attribute is not removed.
    [ResolverPriority(-100)]
    public class ThirdAttributeProcessor : OdinAttributeProcessor<PrioritizedProcessed>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var boxGroup = attributes.OfType<BoxGroupAttribute>().FirstOrDefault();
            boxGroup.GroupName = boxGroup.GroupName + " - Third";
        }
    }
}
#endif
