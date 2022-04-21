#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        private bool show;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return show ? base.GetPropertyHeight(property, label) : -2;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var att = (ShowIfAttribute)attribute;
            var parent = ReflectionTools.GetRelativeMemberParent(property.serializedObject.targetObject, property.propertyPath);
            var member = ReflectionTools.RTGetFieldOrProp(parent.GetType(), att.propertyName);
            var value = (int)System.Convert.ChangeType(member.RTGetFieldOrPropValue(parent), typeof(int));
            show = value == att.value;
            if ( show ) {
                EditorGUI.PropertyField(position, property);
            }
        }
    }
}

#endif