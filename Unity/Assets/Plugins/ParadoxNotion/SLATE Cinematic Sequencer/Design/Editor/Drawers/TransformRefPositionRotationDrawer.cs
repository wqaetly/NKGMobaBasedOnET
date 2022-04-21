#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Slate
{

    [CustomPropertyDrawer(typeof(TransformRefPositionRotation))]
    public class TransformRefPositionRotationDrawer : PropertyDrawer
    {

        private float height;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent content) {
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent content) {
            var lines = 0;
            var rect = new Rect();
            var lineHeight = EditorGUIUtility.singleLineHeight;

            GUI.color = new Color(1, 1, 1, 0.3f);
            GUI.Box(position, "", (GUIStyle)"flow node 0");
            GUI.color = Color.white;

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, lineHeight), content.text);
            lines++;


            var groupProp = prop.FindPropertyRelative("_group");
            var transformProp = prop.FindPropertyRelative("_transform");
            var posProp = prop.FindPropertyRelative("_position");
            var rotProp = prop.FindPropertyRelative("_rotation");
            var spaceProp = prop.FindPropertyRelative("_space");


            EditorGUI.indentLevel++;

            if ( transformProp.objectReferenceValue == null ) {

                rect = new Rect(position.x, position.y + lineHeight * lines, position.width - 16, lineHeight);
                EditorGUI.PropertyField(rect, groupProp, new GUIContent("Actor Override (?)"));
                lines++;
            }

            if ( groupProp.objectReferenceValue == null ) {

                rect = new Rect(position.x, position.y + lineHeight * lines, position.width - 16, lineHeight);
                EditorGUI.PropertyField(rect, transformProp, new GUIContent("Transform Override (?)"));
                lines++;
            }

            if ( groupProp.objectReferenceValue == null && transformProp.objectReferenceValue == null ) {

                rect = new Rect(position.x, position.y + lineHeight * lines, position.width - 16, lineHeight);
                EditorGUI.PropertyField(rect, posProp);
                lines++;

                rect = new Rect(position.x, position.y + lineHeight * lines, position.width - 16, lineHeight);
                EditorGUI.PropertyField(rect, rotProp);
                lines++;

                rect = new Rect(position.x, position.y + lineHeight * lines, position.width - 16, lineHeight);
                EditorGUI.PropertyField(rect, spaceProp);
                lines++;

            } else {

                GUI.enabled = false;
                rect = new Rect(position.x, position.y + lineHeight * lines, position.width - 16, lineHeight);
                EditorGUI.EnumPopup(rect, "Space", TransformSpace.WorldSpace);
                lines++;
                GUI.enabled = true;
            }


            EditorGUI.indentLevel--;

            height = lines * lineHeight;
            height += 2;
        }
    }
}

#endif