#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Slate.ActionClips
{

    [Category("Composition")]
    [Description("Additive load a Scene for a duration of time, or permanentely if length is zero.\n\nIf 'Update Root Cutscenes' is true, all root cutscenes objects of that scene will also be updated for the duration of the clip with an optional time offset provided.")]
    public class AdditiveScene : DirectorActionClip, ISubClipContainable
    {

#if UNITY_EDITOR
        [Required]
        public UnityEditor.SceneAsset sceneAsset;
#endif

        [SerializeField]
        [HideInInspector]
        private float _length = 5;
        [SerializeField]
        [HideInInspector]
        protected string _scenePath;

        [Tooltip("The position to spawn the SubScene at")]
        public Vector3 scenePosition;
        [Tooltip("The rotation to spawn the SubScene at")]
        public Vector3 sceneRotation;
        [Tooltip("The Space used for ScenePosition and SceneRotation")]
        public MiniTransformSpace space;

        [Tooltip("Should root cutscenes found be updated?")]
        public bool updateRootCutscenes = true;
        [ShowIf("updateRootCutscenes", 1)]
        [Tooltip("Update offset of root cutscenes")]
        public float timeOffset;

        private Scene subScene;

        ///----------------------------------------------------------------------------------------------

        private List<Cutscene> rootCutscenes;
        private bool temporary;
        private bool waitLoad;


        float ISubClipContainable.subClipOffset {
            get { return timeOffset; }
            set { timeOffset = value; }
        }

        float ISubClipContainable.subClipLength {
            get { return 0; }
        }

        float ISubClipContainable.subClipSpeed {
            get { return 0; }
        }

        public override bool isValid {
            get { return !string.IsNullOrEmpty(_scenePath); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }


#if UNITY_EDITOR

        public override string info {
            get { return string.Format("        SubScene\n        '{0}'", sceneAsset ? sceneAsset.name : "NONE"); }
        }

        protected override void OnAfterValidate() {
            _scenePath = AssetDatabase.GetAssetPath(sceneAsset);
        }

#endif

        ///----------------------------------------------------------------------------------------------

        protected override void OnEnter() { temporary = length > 0; Activate(); }
        protected override void OnReverseEnter() { if ( temporary ) { Activate(); } }

        protected override void OnUpdate(float time) {

            if ( Application.isPlaying ) { //SceneManger.sceneLoaded doesn't really work
                if ( waitLoad && subScene.isLoaded ) {
                    waitLoad = false;
                    InitializeSubSceneCutscenes();
                }
            }

            if ( temporary && updateRootCutscenes && rootCutscenes != null ) {
                for ( var i = 0; i < rootCutscenes.Count; i++ ) {
                    rootCutscenes[i].Sample(time - timeOffset);
                }
            }
        }

        protected override void OnExit() { if ( temporary ) { DenitializeSubSceneCutscenes(true); Deactivate(); } }
        protected override void OnReverse() { DenitializeSubSceneCutscenes(false); Deactivate(); }


        void Activate() {

            if ( string.IsNullOrEmpty(_scenePath) ) {
                return;
            }

#if UNITY_EDITOR
            if ( !Application.isPlaying ) {
                subScene = EditorSceneManager.OpenScene(_scenePath, OpenSceneMode.Additive);
                InitializeSubSceneCutscenes();
                return;
            }
#endif

            //because scene loading in runtime is async, we need to double check if we still are within clip range.
            if ( !RootTimeWithinRange() ) {
                return;
            }

            waitLoad = true;
            SceneManager.LoadSceneAsync(CleanPath(_scenePath), LoadSceneMode.Additive);
            subScene = SceneManager.GetSceneByPath(_scenePath);
        }

        void Deactivate() {

            if ( string.IsNullOrEmpty(_scenePath) ) {
                return;
            }

#if UNITY_EDITOR
            if ( !Application.isPlaying ) {
                EditorSceneManager.CloseScene(subScene, true);
                return;
            }
#endif

            SceneManager.UnloadSceneAsync(CleanPath(_scenePath));
            waitLoad = false;
        }

        string CleanPath(string path) {
            return path.Replace("Assets/", "").Replace(".unity", "");
        }

        void InitializeSubSceneCutscenes() {

            rootCutscenes = new List<Cutscene>();
            if ( subScene.isLoaded && subScene.IsValid() ) {
                foreach ( var go in subScene.GetRootGameObjects() ) {

                    go.transform.position += TransformPosition(scenePosition, (TransformSpace)space);
                    go.transform.rotation *= TransformRotation(sceneRotation, (TransformSpace)space);

                    //clean up cameras
                    var cam = go.GetComponent(typeof(IDirectableCamera)) as IDirectableCamera;
                    if ( cam != null ) {
                        cam.gameObject.SetActive(false);
                        continue;
                    }

                    //cache root cutscenes
                    var cutscene = go.GetComponent<Cutscene>();
                    if ( cutscene != null ) {
                        rootCutscenes.Add(cutscene);
                    }
                }
            }
        }

        void DenitializeSubSceneCutscenes(bool forward) {
            if ( rootCutscenes != null ) {
                foreach ( var cutscene in rootCutscenes ) {
                    if ( cutscene != null ) {
                        if ( forward ) { cutscene.SkipAll(); } else { cutscene.Rewind(); }
                    }
                }
            }
            rootCutscenes = null;
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnClipGUI(Rect rect) {
            GUI.DrawTexture(new Rect(0, 0, rect.height, rect.height), Slate.Styles.sceneIcon);
        }

        protected override void OnSceneGUI() {
            if ( !RootTimeWithinRange() ) {
                DoVectorPositionHandle((TransformSpace)space, ref scenePosition);
                DoVectorRotationHandle((TransformSpace)space, scenePosition, ref sceneRotation);
            }
        }

#endif

    }
}