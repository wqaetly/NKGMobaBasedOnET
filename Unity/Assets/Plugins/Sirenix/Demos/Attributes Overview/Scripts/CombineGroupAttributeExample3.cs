#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    // A perhaps extreme example showing you that the same property can exist in multiple groups.
    public class CombineGroupAttributeExample3 : MonoBehaviour
    {
        [Range(0, 10)]
        [LabelWidth(20)]
        [HorizontalGroup("Split", 0.5f)]
        [FoldoutGroup("Split/Alice")]
        [FoldoutGroup("Split/Bob")]
        [BoxGroup("Box")]
        [BoxGroup("Split/Alice/Box")]
        [BoxGroup("Split/Bob/Box")]
        public float A;

        [FoldoutGroup("Split/Alice")]
        [FoldoutGroup("Split/Bob")]
        private void Button()
        {
        }
    }
}
#endif
