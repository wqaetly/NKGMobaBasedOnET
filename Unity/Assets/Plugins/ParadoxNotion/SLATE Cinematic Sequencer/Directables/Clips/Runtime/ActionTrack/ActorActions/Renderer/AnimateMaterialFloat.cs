using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("Renderer")]
    public class AnimateMaterialFloat : ActorActionClip<Renderer>
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.2f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0.2f;

        [ShaderPropertyPopup(typeof(float))]
        public string propertyName;
        [AnimatableParameter]
        public float value;
        public EaseType interpolation = EaseType.QuadraticInOut;

        private float originalValue;

        public override string info {
            get { return string.Format("Animate '{0}'", propertyName); }
        }

        public override bool isValid {
            get { return actor != null && actor.sharedMaterial != null && actor.sharedMaterial.HasProperty(propertyName); }
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

        private Material targetMaterial {
            get { return Application.isPlaying ? actor.material : actor.sharedMaterial; }
        }

        protected override void OnEnter() { DoSet(); }
        protected override void OnReverseEnter() { DoSet(); }

        protected override void OnUpdate(float time) {
            var lerpValue = Easing.Ease(interpolation, originalValue, value, GetClipWeight(time));
            targetMaterial.SetFloat(propertyName, lerpValue);
        }

        protected override void OnReverse() { DoReset(); }
        protected override void OnExit() { DoReset(); }


        void DoSet() {
            originalValue = targetMaterial.GetFloat(propertyName);
        }

        void DoReset() {
            targetMaterial.SetFloat(propertyName, originalValue);
        }
    }
}