#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class CustomContextMenuExamples : MonoBehaviour
    {
        [InfoBox("A custom context menu is added on this property. Right click the property to view the custom context menu.")]
        [CustomContextMenu("Say Hello/Twice", "SayHello")]
        public int MyProperty;

        private void SayHello()
        {
            Debug.Log("Hello Twice");
        }
    }
}
#endif
