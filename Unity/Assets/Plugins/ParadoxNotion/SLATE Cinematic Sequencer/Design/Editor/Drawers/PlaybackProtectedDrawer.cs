#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomPropertyDrawer(typeof(PlaybackProtectedAttribute))]
    public class PlaybackProtectedDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var directable = property.serializedObject.targetObject as IDirectable;
            var wasEnable = GUI.enabled;
            GUI.enabled = directable == null || directable.root.currentTime <= 0;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = wasEnable;
        }
    }
}

#endif