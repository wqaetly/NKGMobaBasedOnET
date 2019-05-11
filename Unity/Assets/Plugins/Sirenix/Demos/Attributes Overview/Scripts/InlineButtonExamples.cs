#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class InlineButtonExamples : MonoBehaviour
    {
        // Inline Buttons:
        [InlineButton("A")]
        public int InlineButton;

        [InlineButton("A")]
        [InlineButton("B", "Custom Button Name")]
        public int ChainedButtons;

        private void A()
        {
            Debug.Log("A");
        }

        private void B()
        {
            Debug.Log("B");
        }
    }
}
#endif
