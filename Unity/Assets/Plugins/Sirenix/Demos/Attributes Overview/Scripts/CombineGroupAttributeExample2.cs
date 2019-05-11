#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    public class CombineGroupAttributeExample2 : MonoBehaviour
    {
        [TabGroup("MyTabGroup", "Tab1")]
        public int[] A;

        [TabGroup("MyTabGroup", "Tab2")]
        public int C;

        [TabGroup("MyTabGroup", "Tab1")]
        public int[] B;

        [BoxGroup("MyTabGroup/Tab2/Box")]
        public int D, E, F;

        [HorizontalGroup("MyTabGroup/Tab1/Split", 0.5f, LabelWidth = 30)]
        public int[] G;

        [HorizontalGroup("MyTabGroup/Tab1/Split")]
        public int[] H;

        [TabGroup("MyTabGroup", "Tab2")]
        public int I;
    }
}
#endif
