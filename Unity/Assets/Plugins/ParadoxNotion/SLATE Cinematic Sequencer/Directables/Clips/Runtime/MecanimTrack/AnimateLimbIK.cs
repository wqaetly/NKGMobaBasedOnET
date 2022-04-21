using UnityEngine;

namespace Slate.ActionClips
{

    [Description("Animate an actor IK Goal. Please note that the Animator needs to have a Controller assigned and 'IK Pass' must be enabled in that Controller.")]
    [Category("Animator IK Control")]
    [Attachable(typeof(ActorActionTrack))]
    public class AnimateLimbIK : ActorActionClip<Animator>
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

        public AvatarIKGoal IKGoal = AvatarIKGoal.RightHand;
        [AnimatableParameter(0, 1)]
        public float weight = 1;

        public TransformRefPositionRotation IKTarget;

        [AnimatableParameter(link = "IKTarget")]
        [ShowTrajectory]
        [PositionHandle]
        public Vector3 targetPosition {
            get { return IKTarget.position; }
            set { IKTarget.position = value; }
        }

        [AnimatableParameter(link = "IKTarget")]
        [RotationHandle("targetPosition")]
        public Vector3 targetRotation {
            get { return IKTarget.rotation; }
            set { IKTarget.rotation = value; }
        }

        private AnimatorDispatcher dispatcher;

        public override string info {
            get { return string.Format("'{0}' IK", IKGoal.ToString()); }
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
            IKTarget.position = ActorPositionInSpace(IKTarget.space);
        }

        protected override void OnAfterValidate() {
            SetParameterEnabled((AnimateLimbIK x) => x.targetPosition, IKTarget.useAnimation);
            SetParameterEnabled((AnimateLimbIK x) => x.targetRotation, IKTarget.useAnimation);
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
            var pos = TransformPosition(IKTarget.position, IKTarget.space);
            var rot = TransformRotation(IKTarget.rotation, IKTarget.space);
            actor.SetIKPosition(IKGoal, pos);
            actor.SetIKRotation(IKGoal, rot);
            actor.SetIKPositionWeight(IKGoal, finalWeight);
            actor.SetIKRotationWeight(IKGoal, finalWeight);
        }
    }
}