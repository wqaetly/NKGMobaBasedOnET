#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class MultiLinePropertyExamples : SerializedMonoBehaviour
    {
        [Multiline(10)]
        public string UnityMultilineField = "";

        [Title("Wide Multiline Text Field", bold: false)]
        [HideLabel]
        [MultiLineProperty(10)]
        public string WideMultilineTextField = "";

        [InfoBox("Odin supports properties, but Unity's own Multiline attribute only works on fields.")]
        [ShowInInspector]
        [MultiLineProperty(10)]
        public string OdinMultilineProperty { get; set; }
    }
}
#endif
