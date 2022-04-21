using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Transform")]
    public class RotateTo : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1;

        public Vector3 targetRotation;
        public EaseType interpolation = EaseType.QuadraticInOut;

        private Vector3 originalRot;

        public override string info {
            get { return string.Format("Rotate To\n{0}", targetRotation); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return length; }
        }

        protected override void OnEnter() {
            originalRot = actor.transform.eulerAngles;
        }

        protected override void OnUpdate(float deltaTime) {
            if ( length == 0 ) {
                actor.transform.eulerAngles = targetRotation;
                return;
            }
            actor.transform.eulerAngles = Easing.Ease(interpolation, originalRot, targetRotation, deltaTime / length);
        }

        protected override void OnReverse() {
            actor.transform.eulerAngles = originalRot;
        }
    }
}