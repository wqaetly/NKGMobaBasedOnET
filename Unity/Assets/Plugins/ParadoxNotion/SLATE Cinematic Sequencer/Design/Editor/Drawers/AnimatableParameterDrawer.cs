#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomPropertyDrawer(typeof(AnimatableParameterAttribute))]
    public class AnimatableParameterDrawer : PropertyDrawer
    {

        private float height;
        private float width;

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) { return height; }
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label) {

            var keyable = prop.serializedObject.targetObject as IKeyable;
            if ( keyable == null ) {
                GUILayout.Label("Parameter is not serialized within a IKeyable object");
                return;
            }
            var animParam = keyable.animationData != null ? keyable.animationData.GetParameterOfName(prop.propertyPath) : null;
            if ( animParam == null ) {
                GUILayout.Label(string.Format("No AnimatedParameter '{0}' was created or found in IKeyable object", prop.propertyPath));
                return;
            }

            //move xmin acording to indent level
            rect.xMin += ( EditorGUI.indentLevel * 10 );
            //zero out intend level so that sub properties (serialized within a nested serialized class) show correctly
            EditorGUI.indentLevel = 0;
            //real width is only available in repaint
            if ( Event.current.type == EventType.Repaint ) {
                width = rect.width;
            }

            GUI.BeginGroup(rect);
            GUILayout.BeginArea(new Rect(0, 0, width, height));
            AnimatableParameterEditor.ShowParameter(animParam, keyable, prop);
            //real height is only available in repaint
            if ( Event.current.type == EventType.Repaint ) {
                height = GUILayoutUtility.GetLastRect().yMax;
            }
            GUILayout.EndArea();
            GUI.EndGroup();

            //if you know a better way to show proper GUILayout stuff in nested serialized classes let me know :)

        }
    }
}

#endif