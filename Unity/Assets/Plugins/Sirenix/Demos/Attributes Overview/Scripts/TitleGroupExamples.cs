#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class TitleGroupExamples : MonoBehaviour
    {
        [TitleGroup("Ints")]
        public int SomeInt1;

        [TitleGroup("$SomeString1", "Optional subtitle")]
        public string SomeString1;

        [TitleGroup("Vectors", "Optional subtitle", alignment: TitleAlignments.Centered, horizontalLine: true, boldTitle: true, indent: false)]
        public Vector2 SomeVector1;

        [TitleGroup("Ints","Optional subtitle", alignment: TitleAlignments.Split)]
        public int SomeInt2;

        [TitleGroup("$SomeString1", "Optional subtitle")]
        public string SomeString2;

        [TitleGroup("Vectors"),ShowInInspector]
        public Vector2 SomeVector2 { get; set; }
        
        [TitleGroup("Ints/Buttons", indent: false), Button]
        private void IntButton() { }

        [TitleGroup("$SomeString1/Buttons", indent: false), Button]
        private void StringButton() { }

        [TitleGroup("Vectors"), Button]
        private void VectorButton() { }
    }
}
#endif
