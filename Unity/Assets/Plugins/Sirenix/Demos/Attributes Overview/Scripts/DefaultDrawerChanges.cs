#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class DefaultDrawerChanges : MonoBehaviour
    {
        [InfoBox("All enums will now have a more feature-rich dropdown that introduces search.")]
        public KeyCode NormalEnum;
        public Sirenix.Utilities.AssemblyTypeFlags FlagEnum;

        [InfoBox("Quaternions now have a more user-friendly drawer with a nifty context menu, that can be customized in the Odin Preferences window.")]
        public Quaternion Quaternion;

        [InfoBox("All vector fields are responsive, meaning that labels are removed when the inspector is narrow. This can be disabled in the Odin Preferences window.")]
        [InfoBox(
            "Vectors have an extra right-click context menu option to quickly set the vector to common values such as Left, Right, Zero, etc.."
            + "\nYou can also scale the magnitude of a vector, by dragging on its respective label.")]
        public Vector3 Vector3;

        [InfoBox("Bounds and rects are drawn using vector fields, so all of the above functionality works here as well.")]
        public Bounds Bounds;

        public Rect Rect;

        [InfoBox("All object fields now have a small pen icon that when left-clicked opens the object in a new, locked inspector window. You can also do this through the right-click context menu, if the drawing of the object field itself is changed by a custom drawer." +
            "\n\nRight-clicking the pen-icon instead opens the object in an inline popup that vanishes when deselected, unless it's a GameObject - those always open full windows." +
            "")]
        public UnityEngine.Object UnityObject;

        [Title("Small Facts")]
        [VerticalGroup(PaddingTop = 10)]
        [InfoBox("- All properties now have a right-click context menu.\n" +
                 "- The state of what's expanded or collapsed in the inspector is stored in a cache file and saved across reloads.\n" +
                 "- You can configure the cache, as well as uncountable other settings and drawer configurations, through Odin's preferences window.")]
        [Button(ButtonSizes.Large)]
        private void OpenGeneralDrawerSettings()
        {
#if UNITY_EDITOR
            Sirenix.OdinInspector.Editor.GeneralDrawerConfig.Instance.OpenInEditor();
#endif
        }
    }
}
#endif
