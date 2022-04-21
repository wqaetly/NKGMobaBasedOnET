using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Rendering")]
    [Description("Shows closed captions at the bottom of the screen. Note that the Play Audio clips of the Audio Track are also able to show subtitles in sync with the audio. Use this for non audible subtitles or captions.")]
    public class Captions : DirectorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 2;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.25f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0.25f;

        [Multiline(5)]
        public string text = "[wind blowing]";
        public Color color = Color.white;
        public EaseType interpolation = EaseType.QuadraticInOut;

        public override string info {
            get { return string.Format("<i>'{0}'</i>", text); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return _blendIn; }
            set { _blendIn = value; }
        }

        public override float blendOut {
            get { return _blendOut; }
            set { _blendOut = value; }
        }

        protected override void OnUpdate(float deltaTime) {
            var lerpColor = color;
            lerpColor.a = Easing.Ease(interpolation, 0, color.a, GetClipWeight(deltaTime));
            DirectorGUI.UpdateSubtitles(text, lerpColor);
        }
    }
}
