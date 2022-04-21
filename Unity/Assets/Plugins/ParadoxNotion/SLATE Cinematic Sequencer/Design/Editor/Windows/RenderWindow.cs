#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;

#if SLATE_USE_FRAMECAPTURER
using RenderSettings = Slate.Prefs.RenderSettings;
using RenderFormat = Slate.UTJ.FrameCapturer.MovieEncoder.Type;
using Slate.UTJ.FrameCapturer;
#endif

namespace Slate
{

    public class RenderWindow : EditorWindow
    {

#if !SLATE_USE_FRAMECAPTURER

		public static void Open(){ CreateInstance<RenderWindow>().ShowUtility(); }
		void OnGUI(){
			EditorGUILayout.HelpBox("To enable Rendering, please download and import the free Rendering Extension package from the website.", MessageType.Info);
			if (GUILayout.Button("DOWNLOAD")){ Help.BrowseURL("http://slate.paradoxnotion.com/downloads/"); }
		}

#else

        private Prefs.RenderSettings settings;
        private RecorderBase recorder;
        private bool isRendering;

        private Cutscene cutscene {
            get { return CutsceneEditor.current != null ? CutsceneEditor.current.cutscene : null; }
        }

        public static void Open() {
            CreateInstance<RenderWindow>().ShowUtility();
        }

        void OnEnable() {
            titleContent = new GUIContent("Slate Render Utility");
            settings = Prefs.renderSettings;
            minSize = new Vector2(450, 280);
        }

        void OnDisable() {
            Prefs.renderSettings = settings;
            Done();
        }

        void Update() {

            if ( !isRendering || cutscene == null ) {
                return;
            }

            cutscene.currentTime += 1f / settings.framerate;
            if ( cutscene.currentTime >= cutscene.cameraTrack.endTime ) {
                Done();
            }
        }

        void OnGUI() {

            if ( cutscene == null ) {
                EditorGUILayout.HelpBox("Cutscene is null or the Cutscene Editor is not open", MessageType.Error);
                return;
            }

            if ( cutscene.cameraTrack == null ) {
                EditorGUILayout.HelpBox("Cutscene has no Camera Track", MessageType.Error);
                return;
            }

            ///----------------------------------------------------------------------------------------------

            settings.renderFormat = (RenderFormat)EditorGUILayout.EnumPopup("Render Format", settings.renderFormat);
            GUILayout.BeginHorizontal();
            settings.folderName = EditorGUILayout.TextField("Root Folder Name", settings.folderName);
            if ( GUILayout.Button("O", GUILayout.Width(30), GUILayout.Height(14)) ) {
                OpenTargetFolder();
            }
            GUILayout.EndHorizontal();
            settings.fileNameMode = (RenderSettings.FileNameMode)EditorGUILayout.EnumPopup("File Name Mode", settings.fileNameMode);
            if ( settings.fileNameMode == RenderSettings.FileNameMode.SpecifyFileName ) {
                EditorGUI.indentLevel++;
                settings.fileName = EditorGUILayout.TextField("File Name", settings.fileName);
                EditorGUI.indentLevel--;
            }

            ///----------------------------------------------------------------------------------------------

            GUILayout.BeginVertical("box");
            if ( settings.renderFormat == RenderFormat.MP4 || settings.renderFormat == RenderFormat.WebM ) {
                settings.captureAudio = EditorGUILayout.Toggle("Capture Audio", settings.captureAudio);
            }
            settings.renderPasses = EditorGUILayout.Toggle("Render Passes", settings.renderPasses);
            settings.framerate = Mathf.Clamp(EditorGUILayout.IntField("Frame Rate", settings.framerate), 2, 60);
            EditorGUILayout.LabelField("Resolution", EditorTools.GetGameViewSize().ToString("0"));
            EditorGUILayout.HelpBox("Rendering Resolution is taken from the Game Window.\nYou can create custom resolutions with the '+' button through the second dropdown in the Game window toolbar (where it usually reads 'Free Aspect').", MessageType.None);
            GUILayout.EndVertical();

            ///----------------------------------------------------------------------------------------------

            GUI.enabled = !isRendering;
            if ( GUILayout.Button(isRendering ? "RENDERING..." : "RENDER", GUILayout.Height(50)) ) {
                Begin();
            }

            GUI.enabled = isRendering;
            if ( GUILayout.Button("CANCEL") ) {
                Done();
            }

            GUI.enabled = true;
            if ( isRendering ) {
                Repaint();
            }
        }

        ///----------------------------------------------------------------------------------------------
        void Begin() {

            if ( isRendering ) {
                return;
            }

            isRendering = true;
            cutscene.Rewind();
            EditorApplication.ExecuteMenuItem("Window/General/Game");
            cutscene.currentTime = cutscene.cameraTrack.startTime;
            cutscene.Sample();

            CutsceneEditor.OnStopInEditor += Done;

            if ( settings.renderPasses ) {
                recorder = DirectorCamera.renderCamera.GetAddComponent<GBufferRecorder>();
            } else {
                recorder = DirectorCamera.renderCamera.GetAddComponent<MovieRecorder>();
            }

            var config = new MovieEncoderConfigs(settings.renderFormat);
            recorder.encoderConfigs = config;
            recorder.captureControl = RecorderBase.CaptureControl.Manual;
            recorder.targetFramerate = settings.framerate;
            recorder.captureAudio = settings.captureAudio;
            recorder.fixDeltaTime = Application.isPlaying;
            recorder.waitDeltaTime = Application.isPlaying;

            recorder.outputDir = new DataPath(DataPath.Root.Current, GetFolderName(), GetFileName());
            recorder.BeginRecording();
        }

        ///----------------------------------------------------------------------------------------------
        void Done() {

            if ( !isRendering ) {
                return;
            }

            CutsceneEditor.OnStopInEditor -= Done;
            isRendering = false;

            if ( recorder != null ) {
                recorder.EndRecording();
                DestroyImmediate(recorder, true);
            }

            cutscene.Rewind();
            OpenTargetFolder();
        }

        string GetFolderName() {
            if ( settings.fileNameMode == RenderSettings.FileNameMode.UseCutsceneName ) {
                return settings.folderName + "/" + cutscene.name;
            } else {
                return settings.folderName;
            }
        }

        string GetFileName() {
            if ( settings.fileNameMode == RenderSettings.FileNameMode.UseCutsceneName ) {
                return cutscene.name;
            } else {
                return settings.fileName;
            }
        }

        void OpenTargetFolder() {
            var path = Application.dataPath.Replace("/Assets", "/" + GetFolderName() + "/");
            System.IO.Directory.CreateDirectory(path); //ensure folder exists
            Application.OpenURL(path);
        }
#endif


    }
}

#endif