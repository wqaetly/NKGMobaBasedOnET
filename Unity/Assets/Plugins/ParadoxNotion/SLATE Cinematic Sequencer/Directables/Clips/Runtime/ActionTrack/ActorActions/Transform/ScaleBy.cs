using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Transform")]
    [Description("Scale the actor by specified value and optionlay per second")]
    public class ScaleBy : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1;

        public Vector3 scale = Vector3.one;
        public bool perSecond;
        public EaseType interpolation = EaseType.QuadraticInOut;

        private Vector3 originalScale;

        public override string info {
            get { return string.Format("Scale{0} By\n{1}", perSecond ? " Per Second" : "", scale); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return length; }
        }

        protected override void OnEnter() {
            originalScale = actor.transform.localScale;
        }

        protected override void OnUpdate(float deltaTime) {
            var target = originalScale + ( scale * ( perSecond ? length : 1 ) );
            actor.transform.localScale = Easing.Ease(interpolation, originalScale, target, deltaTime / length);
        }

        protected override void OnReverse() {
            actor.transform.localScale = originalScale;
        }
    }
}