using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("Renderer")]
    public class AnimateMaterialColor : ActorActionClip<Renderer>
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

        [ShaderPropertyPopup(typeof(Color))]
        public string propertyName = "_Color";
        [AnimatableParameter]
        public Color color = Color.white;
        public EaseType interpolation = EaseType.QuadraticInOut;

        private Color originalColor;

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
            var lerpColor = Easing.Ease(interpolation, originalColor, color, GetClipWeight(time));
            targetMaterial.SetColor(propertyName, lerpColor);
        }

        protected override void OnReverse() { DoReset(); }
        protected override void OnExit() { DoReset(); }


        void DoSet() {
            originalColor = targetMaterial.GetColor(propertyName);
        }

        void DoReset() {
            targetMaterial.SetColor(propertyName, originalColor);
        }
    }
}