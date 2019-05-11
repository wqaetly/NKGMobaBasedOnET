#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class HideLabelExamples : MonoBehaviour
    {
        [Title("Wide Colors")]
        [HideLabel]
        [ColorPalette("Fall")]
        public Color WideColor1;

        [HideLabel]
        [ColorPalette("Fall")]
        public Color WideColor2;

        [Title("Wide Vector")]
        [HideLabel]
        public Vector3 WideVector1;

        [HideLabel]
        public Vector4 WideVector2;

        [Title("Wide String")]
        [HideLabel]
        public string WideString;

        [Title("Wide Multiline Text Field")]
        [HideLabel]
        [MultiLineProperty]
        public string WideMultilineTextField = "";
    }
}
#endif
