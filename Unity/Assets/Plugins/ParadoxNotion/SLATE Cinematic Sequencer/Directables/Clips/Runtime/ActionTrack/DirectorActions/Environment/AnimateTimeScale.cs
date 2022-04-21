using UnityEngine;

namespace Slate.ActionClips
{

    [Category("Environment")]
    public class AnimateTimeScale : DirectorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn;
        [SerializeField]
        [HideInInspector]
        private float _blendOut;

        [AnimatableParameter(0.01f, 10)]
        public float timeScale;
        public EaseType interpolation = EaseType.QuadraticInOut;

        private float wasScale;

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

        protected override void OnCreate() {
            timeScale = Time.timeScale;
        }

        protected override void OnEnter() {
            wasScale = Time.timeScale;
        }

        protected override void OnUpdate(float time) {
            var value = Easing.Ease(interpolation, wasScale, timeScale, GetClipWeight(time));
            value = Mathf.Clamp(value, 0.01f, 100);
            Time.timeScale = value;
        }

        protected override void OnReverse() {
            Time.timeScale = wasScale;
        }
    }
}