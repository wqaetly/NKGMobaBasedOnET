using UnityEngine;
using System;

namespace Slate
{

    [ExecuteInEditMode]
    ///Handles subtitles, fades, crossfades etc. Acts as an event dispatcher.
    ///GUI should be implemented elsewhere by subscribing to these events.
    public class DirectorGUI : MonoBehaviour
    {

        //EVENT DELEGATES
        public delegate void SubtitlesGUIDelegate(string text, Color color);
        public delegate void TextOverlayGUIDelegate(string text, Color color, float size, TextAnchor alignment, Vector2 position);
        public delegate void ImageOverlayGUIDelegate(Texture texture, Color color, Vector2 scale, Vector2 position);
        public delegate void ScreenFadeGUIDelegate(Color color);
        public delegate void LetterboxGUIDelegate(float completion);
        public delegate void CameraDissolveDelegate(Texture texture, float completion);

        ///EVENTS
        ///Subscribe to any of these events to handle UI.
        public static event SubtitlesGUIDelegate OnSubtitlesGUI;
        public static event TextOverlayGUIDelegate OnTextOverlayGUI;
        public static event ImageOverlayGUIDelegate OnImageOverlayGUI;
        public static event ScreenFadeGUIDelegate OnScreenFadeGUI;
        public static event LetterboxGUIDelegate OnLetterboxGUI;
        public static event CameraDissolveDelegate OnCameraDissolve;

        public static event Action OnGUIEnable;
        public static event Action OnGUIDisable;
        public static event Action OnGUIUpdate;

        ///----------------------------------------------------------------------------------------------

        [NonSerialized]
        private static DirectorGUI _current;
        public static DirectorGUI current {
            get
            {
                if ( _current == null ) {
                    //we attach DirectorGUI instance on DirectorCamera for organization
                    _current = FindObjectOfType<DirectorGUI>();
                    if ( _current == null && DirectorCamera.current != null ) {
                        _current = DirectorCamera.current.gameObject.GetAddComponent<DirectorGUI>();
                    }
                }
                return _current;
            }
        }


        //...
        void Awake() {
            if ( _current != null && _current != this ) {
                DestroyImmediate(this);
                return;
            }
            _current = this;
        }

        //...
        void OnEnable() {
            if ( OnGUIEnable != null ) {
                OnGUIEnable();
            }
        }

        //Reset values whenever disabled. Thus for example fading out from a cutscene, the next cutscene does not remain faded.
        void OnDisable() {
            UpdateDissolve(null, 0);
            UpdateLetterbox(0);
            UpdateFade(Color.clear);
            UpdateSubtitles(null, Color.clear);
            UpdateOverlayText(null, Color.clear, 0, default(TextAnchor), Vector2.zero);
            UpdateOverlayImage(null, Color.clear, Vector2.zero, Vector2.zero);

            if ( OnGUIDisable != null ) {
                OnGUIDisable();
            }
        }

        ///----------------------------------------------------------------------------------------------

        //...
        public static void UpdateLetterbox(float completion) {
            if ( OnLetterboxGUI != null ) {
                OnLetterboxGUI(completion);
            }
        }

        //...
        public static void UpdateDissolve(Texture texture, float completion) {
            if ( OnCameraDissolve != null ) {
                OnCameraDissolve(texture, completion);
            }
        }

        //...
        public static Color lastFadeColor { get; private set; }
        public static void UpdateFade(Color color) {
            lastFadeColor = color;
            if ( OnScreenFadeGUI != null ) {
                OnScreenFadeGUI(color);
            }
        }

        //...
        public static void UpdateSubtitles(string text, Color color) {
            if ( OnSubtitlesGUI != null ) {
                OnSubtitlesGUI(text, color);
            }
        }

        //...
        public static void UpdateOverlayText(string text, Color color, float size, TextAnchor anchor, Vector2 pos) {
            if ( OnTextOverlayGUI != null ) {
                OnTextOverlayGUI(text, color, size, anchor, pos);
            }
        }

        //...
        public static void UpdateOverlayImage(Texture texture, Color color, Vector2 scale, Vector2 positionOffset) {
            if ( OnImageOverlayGUI != null ) {
                OnImageOverlayGUI(texture, color, scale, positionOffset);
            }
        }

#if UNITY_EDITOR
        ///we keep OnGUI only in editor cause it generates garbage. It's not required in runtime.
        void OnGUI() {

            if ( OnGUIUpdate != null ) {
                OnGUIUpdate();
            }

            ///----------------------------------------------------------------------------------------------
            ///This is a way to inform user of missing GUI implementations
            string missing = null;
            if ( OnLetterboxGUI == null ) missing += "\n - Letterbox";
            if ( OnCameraDissolve == null ) missing += "\n - CameraDissolve";
            if ( OnScreenFadeGUI == null ) missing += "\n - ScreenFade";
            if ( OnSubtitlesGUI == null ) missing += "\n - Subtitles";
            if ( OnTextOverlayGUI == null ) missing += "\n - OverlayText";
            if ( OnImageOverlayGUI == null ) missing += "\n - OverlayImage";

            if ( !string.IsNullOrEmpty(missing) ) {
                var rect = new Rect(10, 10, Screen.width - 10, Screen.height - 10);
                var text = string.Format("Missing GUI Implementations:{0}\n\nPlease add prefab named '@DirectorGUI' in the scene.\nAlternatively you can create your own GUI implementation.", missing);
                var size = GUI.skin.label.CalcSize(new GUIContent(text));
                var bgRect = new Rect(5, 5, size.x + 10, size.y + 10);
                GUI.color = new Color(0.2f, 0.2f, 0.2f);
                GUI.DrawTexture(bgRect, Texture2D.whiteTexture);
                GUI.color = Color.white;
                GUI.Label(rect, text);
            }

        }
#endif

    }
}