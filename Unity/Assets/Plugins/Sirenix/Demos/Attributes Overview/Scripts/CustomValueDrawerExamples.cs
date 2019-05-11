#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using Sirenix.OdinInspector;
    using UnityEngine;

#if UNITY_EDITOR

    using UnityEditor;

#endif

    public class CustomValueDrawerExamples : MonoBehaviour
    {
        public float From = 2, To = 7;

        [CustomValueDrawer("MyStaticCustomDrawerStatic")]
        public float CustomDrawerStatic;

        [CustomValueDrawer("MyStaticCustomDrawerInstance")]
        public float CustomDrawerInstance;

        [CustomValueDrawer("MyStaticCustomDrawerArray")]
        public float[] CustomDrawerArray;

#if UNITY_EDITOR

        private static float MyStaticCustomDrawerStatic(float value, GUIContent label)
        {
            return EditorGUILayout.Slider(label, value, 0f, 10f);
        }

        private float MyStaticCustomDrawerInstance(float value, GUIContent label)
        {
            return EditorGUILayout.Slider(label, value, this.From, this.To);
        }

        private float MyStaticCustomDrawerArray(float value, GUIContent label)
        {
            return EditorGUILayout.Slider(value, this.From, this.To);
        }

#endif
    }
}
#endif
