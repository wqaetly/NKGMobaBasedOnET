#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomEditor(typeof(ActionClips.FollowPath))]
    public class FollowPathInspector : ActionClipInspector<ActionClips.FollowPath>
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if ( action.path == null ) {
                if ( GUILayout.Button("Create New Path") ) {
                    action.path = BezierPath.Create(action.root.context.transform);
                    UnityEditor.Selection.activeObject = action.path;
                }
            }
        }
    }
}

#endif