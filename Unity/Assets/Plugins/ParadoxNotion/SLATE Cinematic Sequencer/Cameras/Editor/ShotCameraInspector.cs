#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Slate
{

    [CustomEditor(typeof(ShotCamera))]
    public class ShotCameraInspector : Editor
    {

        private SerializedProperty focalDistanceProp;
        private SerializedProperty focalLengthProp;
        private SerializedProperty focalApertureProp;
        private SerializedProperty controllerProp;

        private ShotCamera shot {
            get { return (ShotCamera)target; }
        }

        void OnEnable() {
            focalDistanceProp = serializedObject.FindProperty("_focalDistance");
            focalLengthProp = serializedObject.FindProperty("_focalLength");
            focalApertureProp = serializedObject.FindProperty("_focalAperture");
            controllerProp = serializedObject.FindProperty("_dynamicController");
        }

        void OnSceneGUI() {
            shot.OnSceneGUI();
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.HelpBox("The Camera Component attached above is mostly used for Editor Previews and thus the reason why it's not editable.\n\nYou can instead change the 'Render Camera' settings if so required found under the 'Director Camera Root' GameObject, and which is the only Camera Cutscenes are rendered from within.\n\nFor more options and for animating this Shot, please select a Shot Clip that makes use of this Shot Camera in a Slate Editor Window Camera Track.", MessageType.Info);
            shot.fieldOfView = EditorGUILayout.Slider("Field Of View", shot.fieldOfView, 5, 170);
            serializedObject.Update();
            EditorGUILayout.PropertyField(focalDistanceProp);
            EditorGUILayout.PropertyField(focalLengthProp);
            EditorGUILayout.PropertyField(focalApertureProp);
            EditorGUILayout.PropertyField(controllerProp);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif