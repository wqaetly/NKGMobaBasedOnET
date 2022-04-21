#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : DecoratorDrawer
    {
        public override float GetHeight() { return -2; }
        public override void OnGUI(Rect position) {
            if ( Prefs.showDescriptions ) {
                EditorGUILayout.HelpBox(( attribute as HelpBoxAttribute ).text, MessageType.None);
            }
        }
    }
}

#endif