using UnityEngine;
using UnityEngine.UI;

namespace Slate
{

    [ExecuteInEditMode]
    ///A GUI implementation for subs, fades, dissolves etc. You can modify it, expand it or base your own on this.
    public class DirectorGUITemplate : MonoBehaviour
    {
        public bool dontDestroyOnLoad = true;

        public CanvasScaler absScaler;
        public CanvasScaler refScaler;

        public CanvasGroup cameraDissolveGroup;
        public RawImage dissolverImage;

        public CanvasGroup letterboxGroup;
        public RawImage letterboxTop;
        public RawImage letterboxBottom;

        public CanvasGroup screenFadeGroup;
        public RawImage screenFadeImage;

        public CanvasGroup overlayImageGroup;
        public RawImage overlayImage;

        public CanvasGroup overlayTextGroup;
        public Text overlayText;

        public CanvasGroup subtitlesGroup;
        public Text subtitlesText;

        private static DirectorGUITemplate _current;

        ///----------------------------------------------------------------------------------------------

        //...
        void Awake() {

            if ( !Application.isPlaying ) { return; }

            if ( _current != null && _current != this ) {
                DestroyImmediate(this.gameObject);
                return;
            }

            _current = this;
            if ( dontDestroyOnLoad ) {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        [ContextMenu("Show All")]
        void ShowAll() {
            subtitlesGroup.alpha = 1;
            overlayImageGroup.alpha = 1;
            overlayImageGroup.alpha = 1;
            screenFadeGroup.alpha = 1;
            letterboxGroup.alpha = 1;
            cameraDissolveGroup.alpha = 1;
        }

        [ContextMenu("Hide All")]
        void HideAll() {
            subtitlesGroup.alpha = 0;
            overlayImageGroup.alpha = 0;
            overlayImageGroup.alpha = 0;
            screenFadeGroup.alpha = 0;
            letterboxGroup.alpha = 0;
            cameraDissolveGroup.alpha = 0;
        }

        ///----------------------------------------------------------------------------------------------

        void OnEnable() {
            DirectorGUI.OnSubtitlesGUI += OnSubtitlesGUI;
            DirectorGUI.OnTextOverlayGUI += OnTextOverlayGUI;
            DirectorGUI.OnImageOverlayGUI += OnImageOverlayGUI;
            DirectorGUI.OnScreenFadeGUI += OnScreenFadeGUI;
            DirectorGUI.OnLetterboxGUI += OnLetterboxGUI;
            DirectorGUI.OnCameraDissolve += OnCameraDissolve;

            if ( Application.isPlaying ) {
                HideAll();
            }
        }

        void OnDisable() {
            DirectorGUI.OnSubtitlesGUI -= OnSubtitlesGUI;
            DirectorGUI.OnTextOverlayGUI -= OnTextOverlayGUI;
            DirectorGUI.OnImageOverlayGUI -= OnImageOverlayGUI;
            DirectorGUI.OnScreenFadeGUI -= OnScreenFadeGUI;
            DirectorGUI.OnLetterboxGUI -= OnLetterboxGUI;
            DirectorGUI.OnCameraDissolve -= OnCameraDissolve;

            if ( Application.isPlaying ) {
                HideAll();
            }
        }

        ///----------------------------------------------------------------------------------------------

        void Update() {
            absScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        }

        void OnCameraDissolve(Texture texture, float completion) {
            if ( texture == null ) {
                cameraDissolveGroup.alpha = 0;
                return;
            }
            cameraDissolveGroup.alpha = 1 - completion;
            dissolverImage.color = Color.white;
            dissolverImage.texture = texture;
            dissolverImage.SetNativeSize();
        }

        void OnImageOverlayGUI(Texture texture, Color color, Vector2 scale, Vector2 position) {
            if ( texture == null ) {
                overlayTextGroup.alpha = 0;
                return;
            }
            overlayImageGroup.alpha = color.a;
            overlayImage.color = color;
            overlayImage.texture = texture;
            overlayImage.rectTransform.localScale = new Vector3(scale.x, scale.y, 1);
            overlayImage.rectTransform.anchoredPosition = position;
            overlayImage.SetNativeSize();
        }

        void OnLetterboxGUI(float completion) {
            letterboxGroup.alpha = completion;
            var scaleTop = letterboxTop.rectTransform.localScale;
            var scaleBottom = letterboxBottom.rectTransform.localScale;
            scaleTop.y = Mathf.Max(Easing.Ease(EaseType.QuadraticInOut, 0, 1, completion), 0.01f);
            scaleBottom.y = Mathf.Max(Easing.Ease(EaseType.QuadraticInOut, 0, 1, completion), 0.01f);
            letterboxTop.rectTransform.localScale = scaleTop;
            letterboxBottom.rectTransform.localScale = scaleBottom;
        }

        void OnScreenFadeGUI(Color color) {
            screenFadeGroup.alpha = color.a;
            screenFadeImage.color = color;
        }

        void OnTextOverlayGUI(string text, Color color, float size, TextAnchor anchor, Vector2 position) {
            overlayTextGroup.alpha = color.a;
            overlayText.color = color;
            overlayText.text = text;
            overlayText.fontSize = (int)size;
            overlayText.alignment = anchor;
            overlayText.rectTransform.anchoredPosition = position;
        }

        void OnSubtitlesGUI(string text, Color color) {
            subtitlesGroup.alpha = color.a;
            subtitlesText.color = color;
            subtitlesText.text = text;
        }
    }
}