#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomEditor(typeof(ActionClips.AnimateProperties))]
    public class AnimatePropertiesInspector : ActionClipInspector<ActionClips.AnimateProperties>
    {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUILayout.Space(10);
            if ( GUILayout.Button("Add Property") ) {
                EditorTools.ShowAnimatedPropertySelectionMenu(action.actor.gameObject, action.TryAddParameter);
            }
        }
    }
}

#endif