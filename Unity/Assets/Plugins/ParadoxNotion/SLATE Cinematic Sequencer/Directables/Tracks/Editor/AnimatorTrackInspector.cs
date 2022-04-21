#if UNITY_EDITOR && UNITY_2017_1_OR_NEWER

using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomEditor(typeof(AnimatorTrack))]
    public class AnimatorTrackInspector : CutsceneTrackInspector
    {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var animatorTrack = (AnimatorTrack)target;

            if ( animatorTrack.isMasterTrack && animatorTrack.useRootMotion ) {

                if ( !animatorTrack.isRootMotionPreBaked && GUILayout.Button("Pre-Bake Root Motion And Lock Track") ) { animatorTrack.PreBakeRootMotion(); }
                if ( animatorTrack.isRootMotionPreBaked && GUILayout.Button("Clear Baked Root Motion And Unlock Track") ) { animatorTrack.ClearPreBakeRootMotion(); }

                UnityEditor.EditorGUILayout.HelpBox("Pre-Baking Root Motion will speed up initialization of the Animator Track and thus the cutscene, but the runtime position/rotation of the actor when the cutscene starts will not be taken into account. When Root Motion is pre-baked, the track will also be locked to changes, but it can be unlocked by clearing the baked data.", MessageType.None);
            }
        }
    }
}

#endif