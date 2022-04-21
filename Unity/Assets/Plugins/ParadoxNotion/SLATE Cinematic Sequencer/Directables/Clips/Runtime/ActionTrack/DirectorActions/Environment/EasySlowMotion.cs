using UnityEngine;
using System.Collections;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("Environment")]
    [Description("A quick way to create a slow-motion time effect.")]
    [System.Obsolete]
    public class EasySlowMotion : DirectorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1.2f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.5f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0.5f;

        [Range(0.05f, 1)]
        public float timeScale = 0.1f;
        public EaseType interpolation = EaseType.QuadraticInOut;

        private float lastTimeScale;

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

        public override string info {
            get { return "Slow Motion\n" + timeScale.ToString("0.00"); }
        }

        protected override void OnEnter() {
            lastTimeScale = Time.timeScale;
        }

        protected override void OnUpdate(float deltaTime) {
            Time.timeScale = Easing.Ease(interpolation, lastTimeScale, timeScale, GetClipWeight(deltaTime));
            Time.timeScale = Mathf.Max(Time.timeScale, 0.05f);
        }

        protected override void OnReverse() {
            Time.timeScale = lastTimeScale;
        }
    }
}