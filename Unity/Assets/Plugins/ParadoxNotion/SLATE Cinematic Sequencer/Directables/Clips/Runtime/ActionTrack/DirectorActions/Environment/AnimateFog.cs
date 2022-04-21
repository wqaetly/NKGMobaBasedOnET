using UnityEngine;

namespace Slate.ActionClips
{

    [Category("Environment")]
    public class AnimateFog : DirectorActionClip
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

        [AnimatableParameter]
        public Color fogColor;
        [AnimatableParameter]
        public float fogDensity;
        [AnimatableParameter]
        public float linearFogStartDistance;
        [AnimatableParameter]
        public float linearFogEndDistance;

        private Color wasColor;
        private float wasDensity;
        private float wasStartDistance;
        private float wasEndDistance;

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
            fogColor = RenderSettings.fogColor;
            fogDensity = RenderSettings.fogDensity;
            linearFogStartDistance = RenderSettings.fogStartDistance;
            linearFogEndDistance = RenderSettings.fogEndDistance;
        }

        protected override void OnEnter() {
            wasColor = RenderSettings.fogColor;
            wasDensity = RenderSettings.fogDensity;
            wasStartDistance = RenderSettings.fogStartDistance;
            wasEndDistance = RenderSettings.fogEndDistance;
        }

        protected override void OnUpdate(float time) {
            var weight = GetClipWeight(time);
            RenderSettings.fogColor = Color.Lerp(wasColor, fogColor, weight);
            RenderSettings.fogDensity = Mathf.Lerp(wasDensity, fogDensity, weight);
            RenderSettings.fogStartDistance = Mathf.Lerp(wasStartDistance, linearFogStartDistance, weight);
            RenderSettings.fogEndDistance = Mathf.Lerp(wasEndDistance, linearFogEndDistance, weight);
        }

        protected override void OnReverse() {
            RenderSettings.fogColor = wasColor;
            RenderSettings.fogDensity = wasDensity;
            RenderSettings.fogStartDistance = wasStartDistance;
            RenderSettings.fogEndDistance = wasEndDistance;
        }
    }
}