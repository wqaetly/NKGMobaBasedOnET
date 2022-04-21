using UnityEngine;

namespace Slate.ActionClips
{

    [Description("Make the actor look at target position. Please note that the Animator needs to have a Controller assigned and 'IK Pass' must be enabled in that Controller.")]
    [Category("Animator IK Control")]
    [Attachable(typeof(ActorActionTrack))]
    public class AnimateLookAtIK : ActorActionClip<Animator>
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.2f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0.2f;

        [AnimatableParameter(0, 1)]
        public float weight = 1;
        [AnimatableParameter(0, 1)]
        public float bodyWeight = 0.25f;
        [AnimatableParameter(0, 1)]
        public float headWeight = 0.95f;
        [AnimatableParameter(0, 1)]
        public float eyesWeight = 1;

        public TransformRefPosition targetPosition;
        [AnimatableParameter(link = "targetPosition")]
        [ShowTrajectory]
        [PositionHandle]
        public Vector3 targetPositionVector {
            get { return targetPosition.value; }
            set { targetPosition.value = value; }
        }

        private AnimatorDispatcher dispatcher;

        public override string info {
            get { return "Look At IK"; }
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

        protected override void OnCreate() {
            targetPosition.value = ActorPositionInSpace(targetPosition.space);
        }

        protected override void OnAfterValidate() {
            SetParameterEnabled((AnimateLookAtIK x) => x.targetPositionVector, targetPosition.useAnimation);
        }

        protected override void OnEnter() {
            dispatcher = actor.GetAddComponent<AnimatorDispatcher>();
            dispatcher.onAnimatorIK += OnAnimatorIK;
        }

        protected override void OnReverseEnter() {
            dispatcher = actor.GetAddComponent<AnimatorDispatcher>();
            dispatcher.onAnimatorIK += OnAnimatorIK;
        }

        protected override void OnReverse() {
            dispatcher.onAnimatorIK -= OnAnimatorIK;
        }

        protected override void OnExit() {
            dispatcher.onAnimatorIK -= OnAnimatorIK;
        }

        protected override void OnRootDisabled() {
            if ( dispatcher != null ) { DestroyImmediate(dispatcher); }
        }

        void OnAnimatorIK(int index) {
            var finalWeight = GetClipWeight() * weight;
            var pos = TransformPosition(targetPosition.value, targetPosition.space);
            actor.SetLookAtPosition(pos);
            actor.SetLookAtWeight(finalWeight, bodyWeight, headWeight, eyesWeight, 0.5f);
        }
    }
}