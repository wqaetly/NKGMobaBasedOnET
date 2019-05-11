#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System.Collections.Generic;
    using UnityEngine;

    public class HideInEditorAndPlayModeExamples : MonoBehaviour
    {
        [Title("Hidden in play mode")]
        [HideInPlayMode]
        public int A;

        [HideInPlayMode]
        public int B;

        [Title("Hidden in editor mode")]
        [HideInEditorMode]
        public int C;

        [HideInEditorMode]
        public int D;

        [Title("Disable in play mode")]
        [DisableInPlayMode]
        public int E;

        [DisableInPlayMode]
        public int F;

        [Title("Disable in editor mode")]
        [DisableInEditorMode]
        public int G;

        [DisableInEditorMode]
        public int H;
    }
}
#endif
