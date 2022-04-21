#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if ( property.propertyType == SerializedPropertyType.ObjectReference ) {
                GUI.backgroundColor = property.objectReferenceValue == null ? new Color(1, 0.4f, 0.4f) : Color.white;
            } else if ( property.propertyType == SerializedPropertyType.String ) {
                GUI.backgroundColor = string.IsNullOrEmpty(property.stringValue) ? new Color(1, 0.4f, 0.4f) : Color.white;
            }

            EditorGUI.PropertyField(position, property);
            GUI.backgroundColor = Color.white;
        }
    }
}

#endif