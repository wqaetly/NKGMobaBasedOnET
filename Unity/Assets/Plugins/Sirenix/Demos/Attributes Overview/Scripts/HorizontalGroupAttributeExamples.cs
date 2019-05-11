#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    // The width can either be specified as percentage or pixels.
    // All values between 0 and 1 will be treated as a percentage.
    // If the width is 0 the column will be automatically sized.
    // Margin-left and right can only be specified in pixels.

    public class HorizontalGroupAttributeExamples : MonoBehaviour
    {
        [HorizontalGroup]
        public int A;

        [HideLabel, LabelWidth (150)]
        [HorizontalGroup(150)]
        public LayerMask B;

        // LabelWidth can be helpfull when dealing with HorizontalGroups.
        [HorizontalGroup("Group 1", LabelWidth = 20)]
        public int C;

        [HorizontalGroup("Group 1")]
        public int D;

        [HorizontalGroup("Group 1")]
        public int E;

        // Having multiple properties in a column can be achived using multiple groups. Checkout the "Combining Group Attributes" example.
        [HorizontalGroup("Split", 0.5f, LabelWidth = 20)]
        [BoxGroup("Split/Left")]
        public int L;

        [BoxGroup("Split/Right")]
        public int M;

        [BoxGroup("Split/Left")]
        public int N;

        [BoxGroup("Split/Right")]
        public int O;

        // Horizontal Group also has supprot for: Title, MarginLeft, MarginRight, PaddingLeft, PaddingRight, MinWidth and MaxWidth.
        [HorizontalGroup("MyButton", MarginLeft = 0.25f, MarginRight = 0.25f)]
        public void SomeButton()
        {

        }
    }
}
#endif
