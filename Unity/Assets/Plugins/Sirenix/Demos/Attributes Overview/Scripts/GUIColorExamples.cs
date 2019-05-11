#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class GUIColorExamples : MonoBehaviour
    {
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public int ColoredInt1;

        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public int ColoredInt2;

        [ButtonGroup]
        [GUIColor(0, 1, 0)]
        private void Apply()
        {
        }

        [ButtonGroup]
        [GUIColor(1, 0.6f, 0.4f)]
        private void Cancel()
        {
        }

        [InfoBox("You can also reference a color member to dynamically change the color of a property.")]
        [GUIColor("GetButtonColor")]
        [Button(ButtonSizes.Gigantic)]
        private static void IAmFabulous()
        {
        }

#if UNITY_EDITOR
        private static Color GetButtonColor()
        {
            Utilities.Editor.GUIHelper.RequestRepaint();
            return Color.HSVToRGB(Mathf.Cos((float)UnityEditor.EditorApplication.timeSinceStartup + 1f) * 0.225f + 0.325f, 1, 1);
        }
#endif
    }

}
#endif
