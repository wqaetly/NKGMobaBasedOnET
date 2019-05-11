#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    [TypeInfoBox(
        "You can nest groups inside other groups using group paths - here, for example, three box " +
        "groups 'Left', 'Center' and 'Right' are nested inside a horizontal group called 'Split', " +
        "and the box groups are nested by virtue of having the respective paths 'Split/Left', " +
        "'Split/Center' and 'Split/Right'.")]
    public class CombineGroupAttributeExample1 : MonoBehaviour
    {
        [HorizontalGroup("Split", width: 0.4f, LabelWidth = 20)]
        [BoxGroup("Split/Left")]
        public int[] A;

        [BoxGroup("Split/Left")]
        public int[] C;

        [BoxGroup("Split/Center")]
        public int[] B;

        [BoxGroup("Split/Center")]
        public int[] D;

        [HorizontalGroup("Split", width: 0.4f)]
        [BoxGroup("Split/Right")]
        public int[] E;

        [BoxGroup("Split/Right")]
        public int[] F;
    }
}
#endif
