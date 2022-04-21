#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomPropertyDrawer(typeof(ActorGroupPopupAttribute))]
    public class ActorGroupPopupDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var att = (ActorGroupPopupAttribute)attribute;
            var directable = property.serializedObject.targetObject as IDirectable;
            if ( directable != null ) {
                var allActorGroups = ( directable.root as Cutscene ).groups.OfType<ActorGroup>().ToList();
                var overrideCurrent = (ActorGroup)property.objectReferenceValue;
                var overrideNew = EditorTools.Popup<ActorGroup>(position, label.text, overrideCurrent, allActorGroups);
                if ( overrideNew != overrideCurrent ) {
                    property.objectReferenceValue = overrideNew;
                }
            }
        }
    }
}

#endif