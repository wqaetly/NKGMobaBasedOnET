#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Reflection;

namespace Slate
{

    [CustomPropertyDrawer(typeof(SortingLayerAttribute))]
    public class SortingLayerPropertyDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) { return -2; }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if ( property.propertyType != SerializedPropertyType.Integer ) {
                GUILayout.Label("SortedLayer property should be an integer ( the layer id )");
                return;
            }
            SortingLayerField(new GUIContent("Sorting Layer"), property, EditorStyles.popup, EditorStyles.label);
        }

        public static void SortingLayerField(GUIContent label, SerializedProperty layerID, GUIStyle style, GUIStyle labelStyle) {
            MethodInfo methodInfo = typeof(EditorGUILayout).GetMethod("SortingLayerField", BindingFlags.Static | BindingFlags.NonPublic, null, new[] { typeof(GUIContent), typeof(SerializedProperty), typeof(GUIStyle), typeof(GUIStyle) }, null);
            if ( methodInfo != null ) {
                object[] parameters = new object[] { label, layerID, style, labelStyle };
                methodInfo.Invoke(null, parameters);
            }
        }
    }
}

#endif