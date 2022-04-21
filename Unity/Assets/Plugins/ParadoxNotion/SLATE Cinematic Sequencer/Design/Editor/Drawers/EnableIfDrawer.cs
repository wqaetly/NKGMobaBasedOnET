#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomPropertyDrawer(typeof(EnabledIfAttribute))]
    public class EnableIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var att = (EnabledIfAttribute)attribute;
            var parent = ReflectionTools.GetRelativeMemberParent(property.serializedObject.targetObject, property.propertyPath);
            var member = ReflectionTools.RTGetFieldOrProp(parent.GetType(), att.propertyName);
            if ( member != null ) {
                var objectValue = member.RTGetFieldOrPropValue(parent);
                var intValue = 1;
                if ( property.propertyType == SerializedPropertyType.ObjectReference ) {
                    intValue = objectValue != null ? 1 : 0;
                } else {
                    intValue = (int)System.Convert.ChangeType(objectValue, typeof(int));
                }
                GUI.enabled = intValue == att.value;
            }
            EditorGUI.PropertyField(position, property);
            GUI.enabled = true;
        }
    }
}

#endif