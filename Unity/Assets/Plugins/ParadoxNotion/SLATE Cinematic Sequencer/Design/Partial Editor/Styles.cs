#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Slate
{

    ///Images and GUIStyles for the editor
    [InitializeOnLoad]
    public static class Styles
    {

        public static Texture2D expressionIcon;
        public static Texture2D stripes;
        public static Texture2D magnetIcon;
        public static Texture2D lockIcon;
        public static Texture2D hiddenIcon;
        public static Texture2D clockIcon;
        public static Texture2D keyIcon;
        public static Texture2D nextKeyIcon;
        public static Texture2D previousKeyIcon;
        public static Texture2D recordIcon;

        public static Texture2D playIcon;
        public static Texture2D playReverseIcon;
        public static Texture2D stepIcon;
        public static Texture2D stepReverseIcon;
        public static Texture2D stopIcon;
        public static Texture2D pauseIcon;
        public static Texture2D loopIcon;
        public static Texture2D pingPongIcon;

        public static Texture2D carretIcon;
        public static Texture2D carretInIcon;
        public static Texture2D carretOutIcon;
        public static Texture2D cutsceneIconOpen;
        public static Texture2D cutsceneIconClose;

        public static Texture2D cutsceneIcon;
        public static Texture2D slateIcon;

        public static Texture2D borderShadowsImage;

        public static Texture2D gearIcon;
        public static Texture2D plusIcon;
        public static Texture2D trashIcon;
        public static Texture2D curveIcon;

        public static Texture2D alembicIcon;

        public static Texture2D dopeKey;
        public static Texture2D dopeKeySmooth;
        public static Texture2D dopeKeyLinear;
        public static Texture2D dopeKeyConstant;

        public static Texture2D cameraIcon = AssetPreview.GetMiniTypeThumbnail(typeof(Camera));
        public static Texture2D sceneIcon = AssetPreview.GetMiniTypeThumbnail(typeof(SceneAsset));
        // public static Texture2D dopeKeyIconBig = EditorGUIUtility.FindTexture("blendKey");
        // public static Texture2D errorIconSmall = EditorGUIUtility.FindTexture("CollabError");

        public static Color recordingColor = new Color(1, 0.5f, 0.5f);

        private static GUISkin styleSheet;

        static Styles() {
            Load();
        }

        [InitializeOnLoadMethod]
        public static void Load() {
            dopeKey = (Texture2D)Resources.Load("DopeKey");
            dopeKeySmooth = (Texture2D)Resources.Load("DopeKeySmooth");
            dopeKeyLinear = (Texture2D)Resources.Load("DopeKeyLinear");
            dopeKeyConstant = (Texture2D)Resources.Load("DopeKeyConstant");

            keyIcon = (Texture2D)Resources.Load("KeyIcon");
            nextKeyIcon = (Texture2D)Resources.Load("NextKeyIcon");
            previousKeyIcon = (Texture2D)Resources.Load("PreviousKeyIcon");

            expressionIcon = (Texture2D)Resources.Load("ExpressionIcon");
            stripes = (Texture2D)Resources.Load("Stripes");
            magnetIcon = (Texture2D)Resources.Load("MagnetIcon");
            lockIcon = (Texture2D)Resources.Load("LockIcon");
            hiddenIcon = (Texture2D)Resources.Load("HiddenIcon");
            clockIcon = (Texture2D)Resources.Load("ClockIcon");
            recordIcon = (Texture2D)Resources.Load("RecordIcon");
            playIcon = (Texture2D)Resources.Load("PlayIcon");
            playReverseIcon = (Texture2D)Resources.Load("PlayReverseIcon");
            stepIcon = (Texture2D)Resources.Load("StepIcon");
            stepReverseIcon = (Texture2D)Resources.Load("StepReverseIcon");
            loopIcon = (Texture2D)Resources.Load("LoopIcon");
            pingPongIcon = (Texture2D)Resources.Load("PingPongIcon");
            stopIcon = (Texture2D)Resources.Load("StopIcon");
            pauseIcon = (Texture2D)Resources.Load("PauseIcon");
            carretIcon = (Texture2D)Resources.Load("CarretIcon");
            carretInIcon = (Texture2D)Resources.Load("CarretInIcon");
            carretOutIcon = (Texture2D)Resources.Load("CarretOutIcon");
            cutsceneIconOpen = (Texture2D)Resources.Load("CutsceneIconOpen");
            cutsceneIconClose = (Texture2D)Resources.Load("CutsceneIconClose");
            cutsceneIcon = (Texture2D)Resources.Load("Cutscene Icon");
            slateIcon = (Texture2D)Resources.Load("SLATEIcon");
            borderShadowsImage = (Texture2D)Resources.Load("BorderShadows");
            gearIcon = (Texture2D)Resources.Load("GearIcon");
            plusIcon = (Texture2D)Resources.Load("PlusIcon");
            trashIcon = (Texture2D)Resources.Load("TrashIcon");
            curveIcon = (Texture2D)Resources.Load("CurveIcon");
            alembicIcon = (Texture2D)Resources.Load("AlembicIcon");

            styleSheet = (GUISkin)Resources.Load("StyleSheet");
        }

        ///Get a white 1x1 texture
        public static Texture2D whiteTexture {
            get { return EditorGUIUtility.whiteTexture; }
        }

        private static GUIStyle _shadowBorderStyle;
        public static GUIStyle shadowBorderStyle {
            get { return _shadowBorderStyle != null ? _shadowBorderStyle : _shadowBorderStyle = styleSheet.GetStyle("ShadowBorder"); }
        }

        private static GUIStyle _clipBoxStyle;
        public static GUIStyle clipBoxStyle {
            get { return _clipBoxStyle != null ? _clipBoxStyle : _clipBoxStyle = styleSheet.GetStyle("ClipBox"); }
        }

        private static GUIStyle _clipBoxFooterStyle;
        public static GUIStyle clipBoxFooterStyle {
            get { return _clipBoxFooterStyle != null ? _clipBoxFooterStyle : _clipBoxFooterStyle = styleSheet.GetStyle("ClipBoxFooter"); }
        }

        private static GUIStyle _clipBoxHorizontalStyle;
        public static GUIStyle clipBoxHorizontalStyle {
            get { return _clipBoxHorizontalStyle != null ? _clipBoxHorizontalStyle : _clipBoxHorizontalStyle = styleSheet.GetStyle("ClipBoxHorizontal"); }
        }

        private static GUIStyle _timeBoxStyle;
        public static GUIStyle timeBoxStyle {
            get { return _timeBoxStyle != null ? _timeBoxStyle : _timeBoxStyle = styleSheet.GetStyle("TimeBox"); }
        }

        private static GUIStyle _headerBoxStyle;
        public static GUIStyle headerBoxStyle {
            get { return _headerBoxStyle != null ? _headerBoxStyle : _headerBoxStyle = styleSheet.GetStyle("HeaderBox"); }
        }

        private static GUIStyle _hollowFrameStyle;
        public static GUIStyle hollowFrameStyle {
            get { return _hollowFrameStyle != null ? _hollowFrameStyle : _hollowFrameStyle = styleSheet.GetStyle("HollowFrame"); }
        }

        private static GUIStyle _hollowFrameHorizontalStyle;
        public static GUIStyle hollowFrameHorizontalStyle {
            get { return _hollowFrameHorizontalStyle != null ? _hollowFrameHorizontalStyle : _hollowFrameHorizontalStyle = styleSheet.GetStyle("HollowFrameHorizontal"); }
        }

        private static GUIStyle _leftLabel;
        public static GUIStyle leftLabel {
            get
            {
                if ( _leftLabel != null ) {
                    return _leftLabel;
                }
                _leftLabel = new GUIStyle("label");
                _leftLabel.alignment = TextAnchor.MiddleLeft;
                return _leftLabel;
            }
        }

        private static GUIStyle _rightLabel;
        public static GUIStyle rightLabel {
            get
            {
                if ( _rightLabel != null ) {
                    return _rightLabel;
                }
                _rightLabel = new GUIStyle("label");
                _rightLabel.alignment = TextAnchor.MiddleRight;
                return _rightLabel;
            }
        }

        private static GUIStyle _centerLabel;
        public static GUIStyle centerLabel {
            get
            {
                if ( _centerLabel != null ) {
                    return _centerLabel;
                }
                _centerLabel = new GUIStyle("label");
                _centerLabel.alignment = TextAnchor.MiddleCenter;
                return _centerLabel;
            }
        }
    }
}

#endif