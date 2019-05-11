#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class ToggleLeftExamples : MonoBehaviour
    {
        [InfoBox("Draws the toggle button before the label for a bool property.")]
        [ToggleLeft]
        public bool LeftToggled;

        [EnableIf("LeftToggled")]
        public int A;

        [EnableIf("LeftToggled")]
        public bool B;

        [EnableIf("LeftToggled")]
        public bool C;
    }
}
#endif
