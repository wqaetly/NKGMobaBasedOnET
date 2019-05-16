#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Sirenix.OdinInspector.Editor;
    using UnityEngine;

    public class SelfAndChildrenExample : MonoBehaviour
    {
        public SelfAndChildrenProcessed A;

        public SelfAndChildrenProcessed B;

        public SelfAndChildrenProcessed C;
    }

    [Serializable]
    public class SelfAndChildrenProcessed
    {
        public int Id;
        public string Name;
    }

    public class SelfAndChildrenAttributeProcessor : OdinAttributeProcessor<SelfAndChildrenProcessed>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
            attributes.Add(new BoxGroupAttribute("Box", false));
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            attributes.Add(new HorizontalGroupAttribute() { LabelWidth = 40 });
        }
    }
}
#endif
