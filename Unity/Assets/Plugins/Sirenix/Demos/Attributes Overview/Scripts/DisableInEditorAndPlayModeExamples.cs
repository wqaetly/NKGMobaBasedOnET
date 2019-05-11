#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System.Collections.Generic;
    using UnityEngine;

    public class DisableInEditorAndPlayModeExamples : MonoBehaviour
    {
        [Title("Disabled in play mode")]
        [DisableInPlayMode]
        public int A;

        [DisableInPlayMode]
        public Material B;

        [Title("Disabled in edit mode")]
        [DisableInEditorMode]
        public GameObject C;

        [DisableInEditorMode]
        public GameObject D;
    }
}
#endif
