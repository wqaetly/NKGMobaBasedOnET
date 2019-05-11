#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using UnityEngine;

    public class TabExamples : MonoBehaviour
    {
        [TabGroup("Tab A")]
        public int One;

        [TabGroup("Tab A")]
        public int Two;

        [TabGroup("Tab A")]
        public int Three;

        [TabGroup("Tab B")]
        public string MyString;

        [TabGroup("Tab B")]
        public float MyFloat;

        [TabGroup("Tab C")]
        [HideLabel]
        public MyTabObject TabC;

        [TabGroup("New Group", "Tab A")]
        public int A;

        [TabGroup("New Group", "Tab A")]
        public int B;

        [TabGroup("New Group", "Tab A")]
        public int C;

        [TabGroup("New Group", "Tab B")]
        public string D;

        [TabGroup("New Group", "Tab B")]
        public float E;

        [TabGroup("New Group", "Tab C")]
        [HideLabel]
        public MyTabObject F;

        [Serializable]
        public class MyTabObject
        {
            public int A;
            public int B;
            public int C;
        }
    }
}
#endif
