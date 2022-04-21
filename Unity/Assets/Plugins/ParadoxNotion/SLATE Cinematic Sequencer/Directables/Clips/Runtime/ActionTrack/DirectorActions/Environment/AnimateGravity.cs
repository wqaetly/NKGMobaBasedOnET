using UnityEngine;

namespace Slate.ActionClips
{

    [Category("Environment")]
    [Description("Animate the gravity physics are using for Rigidbodies. This applies to both 2D and 3D Physics.")]
    public class AnimateGravity : DirectorActionClip
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
        public Vector3 gravity;

        private Vector3 wasGravity;

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
            gravity = Physics.gravity;
        }

        protected override void OnEnter() {
            wasGravity = Physics.gravity;
        }

        protected override void OnUpdate(float time) {
            var weight = GetClipWeight(time);
            Physics.gravity = Vector3.Lerp(wasGravity, gravity, weight);
            Physics2D.gravity = Physics.gravity;
        }

        protected override void OnReverse() {
            Physics.gravity = wasGravity;
            Physics2D.gravity = wasGravity;
        }
    }
}