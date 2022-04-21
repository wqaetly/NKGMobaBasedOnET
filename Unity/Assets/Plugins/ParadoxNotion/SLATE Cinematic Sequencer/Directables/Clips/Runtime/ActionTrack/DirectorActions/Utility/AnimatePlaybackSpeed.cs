using UnityEngine;

namespace Slate.ActionClips
{

    [Category("Utility")]
    [Description("Animates the playback speed of the cutscene")]
    public class AnimatePlaybackSpeed : DirectorActionClip
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
        public float speed;
        public EaseType interpolation = EaseType.QuadraticInOut;

        private float wasSpeed;

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
            speed = root.playbackSpeed;
        }

        protected override void OnEnter() { wasSpeed = root.playbackSpeed; }
        protected override void OnUpdate(float time) {
            var value = Easing.Ease(interpolation, wasSpeed, speed, GetClipWeight(time));
            value = Mathf.Clamp(value, 0.01f, 100);
            root.playbackSpeed = value;
        }
        protected override void OnReverse() { root.playbackSpeed = wasSpeed; }
    }
}