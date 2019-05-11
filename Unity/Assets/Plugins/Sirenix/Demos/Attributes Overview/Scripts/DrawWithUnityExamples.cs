#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class DrawWithUnityExamples : MonoBehaviour
    {
        [InfoBox("If you ever experience trouble with one of Odin's attributes, there is a good chance that the DrawWithUnity will come in handy.")]
        public GameObject ObjectDrawnWithOdin;

        [DrawWithUnity]
        public GameObject ObjectDrawnWithUnity;
    }

    public class UnityObjectFields : MonoBehaviour
    {

    }
}
#endif
