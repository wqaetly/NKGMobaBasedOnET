using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("Renderer")]
    [Description("Animate a material's texture offset and scale over time")]
    public class AnimateMaterialTexture : ActorActionClip<Renderer>
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

        [ShaderPropertyPopup(typeof(Texture))]
        public string propertyName = "_MainTex";
        [AnimatableParameter]
        public Vector2 offset;
        [AnimatableParameter]
        public Vector2 scale = Vector2.one;
        public EaseType interpolation = EaseType.QuadraticInOut;

        private Vector2 originalOffset;
        private Vector2 originalScale;

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
            var weight = GetClipWeight(time);
            var lerpOffset = Easing.Ease(interpolation, originalOffset, offset, weight);
            var lerpScale = Easing.Ease(interpolation, originalScale, scale, weight);
            targetMaterial.SetTextureOffset(propertyName, lerpOffset);
            targetMaterial.SetTextureScale(propertyName, lerpScale);
        }

        protected override void OnReverse() { DoReset(); }
        protected override void OnExit() { DoReset(); }


        void DoSet() {
            originalOffset = targetMaterial.GetTextureOffset(propertyName);
            originalScale = targetMaterial.GetTextureScale(propertyName);
        }

        void DoReset() {
            targetMaterial.SetTextureOffset(propertyName, originalOffset);
            targetMaterial.SetTextureScale(propertyName, originalScale);
        }
    }
}