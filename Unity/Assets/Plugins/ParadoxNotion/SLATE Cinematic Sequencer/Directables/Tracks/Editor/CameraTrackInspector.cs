#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Linq;

namespace Slate
{

    [CustomEditor(typeof(CameraTrack))]
    public class CameraTrackInspector : CutsceneTrackInspector
    {

        private CameraTrack track {
            get { return (CameraTrack)target; }
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUILayout.BeginVertical("box");
            GUILayout.Label("Active Time Offset");
            var _in = track.startTime;
            var _out = track.endTime;
            EditorGUILayout.MinMaxSlider(ref _in, ref _out, track.parent.startTime, track.parent.endTime);
            track.startTime = _in;
            track.endTime = _out;
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            var length = track.endTime - track.startTime;
            track._blendIn = EditorGUILayout.Slider("Gameplay Blend In", track._blendIn, 0, length / 2);
            track._blendOut = EditorGUILayout.Slider("Gameplay Blend Out", track._blendOut, 0, length / 2);
            track.interpolation = (EaseType)EditorGUILayout.EnumPopup("Interpolation", track.interpolation);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            track.cineBoxFadeTime = EditorGUILayout.Slider("CineBox Fade Time", track.cineBoxFadeTime, 0, 1f);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            track.appliedSmoothing = EditorGUILayout.Slider("Post Smoothing", track.appliedSmoothing, 0, DirectorCamera.MAX_DAMP);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            track.exitCameraOverride = (Camera)EditorGUILayout.ObjectField("Exit Camera Override", track.exitCameraOverride, typeof(Camera), true);
            if ( track.exitCameraOverride == Camera.main && Camera.main != null ) {
                EditorGUILayout.HelpBox("The Main Camera is already the default exit camera. No need to be assigned as an override.", MessageType.Warning);
            }
            GUILayout.EndVertical();
        }
    }
}

#endif