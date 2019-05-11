#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class IndentExamples : MonoBehaviour
    {
        [LabelText("Nicely organize your properties.")]
        [Indent]
        public int A;

        [Indent(2)]
        public int B;

        [Indent(3)]
        public int C;

        [Indent(4)]
        public int D;

        [LabelText("Using the Indent attribute")]
        [Indent]
        public int E;

        [Indent(0)]
        public int F;

        [Indent(-1)]
        public int G;
    }
}
#endif
