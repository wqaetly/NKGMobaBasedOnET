#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

#if SLATE_USE_FRAMECAPTURER
using Slate.UTJ.FrameCapturer;
#endif


namespace Slate
{

    ///SLATE editor preferences
    public static class Prefs
    {

        public const string USE_POSTSTACK_DEFINE = "SLATE_USE_POSTSTACK";
        public const string USE_EXPRESSIONS_DEFINE = "SLATE_USE_EXPRESSIONS";
        public const string USE_FRAMECAPTURER_DEFINE = "SLATE_USE_FRAMECAPTURER";

        [System.Serializable]
        class SerializedData
        {
            public bool showTransforms = false;
            public bool compactMode = false;
            public TimeStepMode timeStepMode = TimeStepMode.Seconds;
            public float snapInterval = 0.1f;
            public int frameRate = 30;
            public bool lockHorizontalCurveEditing = true;
            public bool showDopesheetKeyValues = true;
            public bool autoFirstKey = false;
            public bool autoCleanKeysOffRange = false;
            public TangentMode defaultTangentMode = TangentMode.Smooth;
            public KeyframesStyle keyframesStyle = KeyframesStyle.PerTangentMode;
            public bool showShotThumbnails = true;
            public int thumbnailsRefreshInterval = 30;
            public bool scrollWheelZooms = true;
            public bool showDescriptions = true;
            public float gizmosLightness = 0f;
            public Color motionPathsColor = Color.black;
            public Prefs.RenderSettings renderSettings = new Prefs.RenderSettings();
            public bool autoKey = true;
            public bool magnetSnapping = true;
            public float trackListLeftMargin = 280f;
        }

        [System.Serializable]
        public enum KeyframesStyle
        {
            PerTangentMode,
            AlwaysDiamond
        }

        [System.Serializable]
        public enum TimeStepMode
        {
            Seconds,
            Frames
        }

        [System.Serializable]
        public class RenderSettings
        {

            public enum FileNameMode
            {
                UseCutsceneName,
                SpecifyFileName,
            }

#if SLATE_USE_FRAMECAPTURER
            public MovieEncoder.Type renderFormat = MovieEncoder.Type.PNG;
            public string folderName = "SlateRenders";
            public FileNameMode fileNameMode = FileNameMode.UseCutsceneName;
            public string fileName = "MyRender";
            public int framerate = 30;
            public bool renderPasses = false;
            public bool captureAudio = false;
#endif

        }

        private static SerializedData _data;
        private static SerializedData data {
            get
            {
                if ( _data == null ) {
                    _data = JsonUtility.FromJson<SerializedData>(EditorPrefs.GetString("Slate.EditorPreferences"));
                    if ( _data == null ) {
                        _data = new SerializedData();
                    }
                }
                return _data;
            }
        }

        public static float[] snapIntervals = new float[] { 0.001f, 0.01f, 0.1f };
        public static int[] frameRates = new int[] { 30, 60 };

        public static bool showTransforms {
            get { return data.showTransforms; }
            set { if ( data.showTransforms != value ) { data.showTransforms = value; Save(); } }
        }

        public static bool compactMode {
            get { return data.compactMode; }
            set { if ( data.compactMode != value ) { data.compactMode = value; Save(); } }
        }

        public static float gizmosLightness {
            get { return data.gizmosLightness; }
            set { if ( data.gizmosLightness != value ) { data.gizmosLightness = value; Save(); } }
        }

        public static Color gizmosColor {
            get { return new Color(data.gizmosLightness, data.gizmosLightness, data.gizmosLightness); }
        }

        public static bool showShotThumbnails {
            get { return data.showShotThumbnails; }
            set { if ( data.showShotThumbnails != value ) { data.showShotThumbnails = value; Save(); } }
        }

        public static bool showDopesheetKeyValues {
            get { return data.showDopesheetKeyValues; }
            set { if ( data.showDopesheetKeyValues != value ) { data.showDopesheetKeyValues = value; Save(); } }
        }

        public static KeyframesStyle keyframesStyle {
            get { return data.keyframesStyle; }
            set { if ( data.keyframesStyle != value ) { data.keyframesStyle = value; Save(); } }
        }

        public static bool scrollWheelZooms {
            get { return data.scrollWheelZooms; }
            set { if ( data.scrollWheelZooms != value ) { data.scrollWheelZooms = value; Save(); } }
        }

        public static bool showDescriptions {
            get { return data.showDescriptions; }
            set { if ( data.showDescriptions != value ) { data.showDescriptions = value; Save(); } }
        }

        public static Color motionPathsColor {
            get { return data.motionPathsColor; }
            set { if ( data.motionPathsColor != value ) { data.motionPathsColor = value; Save(); } }
        }

        public static int thumbnailsRefreshInterval {
            get { return data.thumbnailsRefreshInterval; }
            set { if ( data.thumbnailsRefreshInterval != value ) { data.thumbnailsRefreshInterval = value; Save(); } }
        }

        public static bool lockHorizontalCurveEditing {
            get { return data.lockHorizontalCurveEditing; }
            set { if ( data.lockHorizontalCurveEditing != value ) { data.lockHorizontalCurveEditing = value; Save(); } }
        }

        public static TangentMode defaultTangentMode {
            get { return data.defaultTangentMode; }
            set { if ( data.defaultTangentMode != value ) { data.defaultTangentMode = value; Save(); } }
        }

        public static Prefs.RenderSettings renderSettings {
            get { return data.renderSettings; }
            set { data.renderSettings = value; Save(); }
        }

        public static bool autoKey {
            get { return data.autoKey; }
            set { if ( data.autoKey != value ) { data.autoKey = value; Save(); } }
        }

        public static bool autoCleanKeysOffRange {
            get { return data.autoCleanKeysOffRange; }
            set { if ( data.autoCleanKeysOffRange != value ) { data.autoCleanKeysOffRange = value; Save(); } }
        }

        public static bool autoFirstKey {
            get { return data.autoFirstKey; }
            set { if ( data.autoFirstKey != value ) { data.autoFirstKey = value; Save(); } }
        }

        public static bool magnetSnapping {
            get { return data.magnetSnapping; }
            set { if ( data.magnetSnapping != value ) { data.magnetSnapping = value; Save(); } }
        }

        public static float trackListLeftMargin {
            get { return data.trackListLeftMargin; }
            set { if ( data.trackListLeftMargin != value ) { data.trackListLeftMargin = value; Save(); } }
        }

        public static TimeStepMode timeStepMode {
            get { return data.timeStepMode; }
            set
            {
                if ( data.timeStepMode != value ) {
                    data.timeStepMode = value;
                    frameRate = value == TimeStepMode.Frames ? 30 : 10;
                    Save();
                }
            }
        }

        public static int frameRate {
            get { return data.frameRate; }
            set { if ( data.frameRate != value ) { data.frameRate = value; snapInterval = 1f / value; Save(); } }
        }

        public static float snapInterval {
            get { return Mathf.Max(data.snapInterval, 0.001f); }
            set { if ( data.snapInterval != value ) { data.snapInterval = Mathf.Max(value, 0.001f); Save(); } }
        }

        static void Save() {
            EditorPrefs.SetString("Slate.EditorPreferences", JsonUtility.ToJson(data));
        }
    }
}

#endif