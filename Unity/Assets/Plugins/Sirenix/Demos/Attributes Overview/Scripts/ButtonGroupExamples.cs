#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class ButtonGroupExamples : MonoBehaviour
    {
        [ButtonGroup]
        private void A()
        {
        }

        [ButtonGroup]
        private void B()
        {
        }

        [ButtonGroup]
        private void C()
        {
        }

        [ButtonGroup]
        private void D()
        {
        }

        [Button(ButtonSizes.Large)]
        [ButtonGroup("My Button Group")]
        private void E()
        {
        }

        [GUIColor(0, 1, 0)]
        [ButtonGroup("My Button Group")]
        private void F()
        {
        }
    }
}
#endif
