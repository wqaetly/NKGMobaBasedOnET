#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.OdinInspector;

    // Example script from the Unity asset store.
    public class NoBoilerplate : MonoBehaviour
    {
        [TabGroup("Tab", "Tab 1")]
        [AssetList, InlineEditor(InlineEditorModes.SmallPreview)]
        public GameObject GameObject;

        [TabGroup("Tab", "Tab 1")]
        private Quaternion Quaternion;

        [TabGroup("Tab", "Tab 2")]
        private Vector3 Vector3;

        [FoldoutGroup("Tab/Tab 2/Vector4s")]
        private Vector4 A, B, C;

        [Multiline, HideLabel, Title("Enter text:", bold: false)]
        public string MyTextArea;

        [ColorPalette("Fall"), HorizontalGroup(0.5f, marginRight: 3)]
        public List<Color> ColorArray1;

        [ColorPalette("Fall"), HorizontalGroup]
        public List<Color> ColorArray2;

        [Range(0, 1)]
        public float[] FloatRange;

        [ShowInInspector, DisplayAsString]
        public string Property { get { return "Support!"; } }

        [Button]
        private void SayHi()
        {
            Debug.Log("Yo!");
        }
    }
}
#endif
