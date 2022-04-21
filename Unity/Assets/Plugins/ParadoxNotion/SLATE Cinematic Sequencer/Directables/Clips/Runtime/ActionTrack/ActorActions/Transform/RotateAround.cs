using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Transform")]
    [Description("Rotate the actor around target position or object by specified degrees and optionaly per second.")]
    public class RotateAround : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1;

        public Vector3 rotation = new Vector3(0, 360, 0);
        public bool perSecond;
        public bool lookTarget = false;
        public EaseType interpolation = EaseType.QuadraticInOut;
        public TransformRefPosition targetPosition;

        private Vector3 wasPosition;
        private Quaternion wasRotation;
        private Vector3 targetWasPosition;


        [AnimatableParameter(link = "targetPosition")]
        [ShowTrajectory]
        [PositionHandle]
        public Vector3 targetPositionVector {
            get { return targetPosition.value; }
            set { targetPosition.value = value; }
        }


        public override string info {
            get { return string.Format("Rotate {0}{1} Around\n{2}", rotation, perSecond ? " Per Second" : "", targetPosition.useAnimation ? "" : targetPosition.ToString()); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return length; }
        }

        protected override void OnAfterValidate() {
            SetParameterEnabled("targetPositionVector", targetPosition.useAnimation);
        }

        protected override void OnEnter() {
            wasPosition = actor.transform.position;
            wasRotation = actor.transform.rotation;
            targetWasPosition = TransformPosition(targetPosition.value, targetPosition.space);
        }

        protected override void OnUpdate(float time) {
            var pos = TransformPosition(targetPosition.value, targetPosition.space);
            var targetPos = wasPosition + ( rotation * ( perSecond ? length : 1 ) );
            var rot = Easing.Ease(interpolation, Vector3.zero, targetPos, GetClipWeight(time));

            var angle = Quaternion.Euler(rot);
            var rotatedPos = angle * ( wasPosition - targetWasPosition ) + targetWasPosition;
            actor.transform.position = rotatedPos + ( pos - targetWasPosition );

            if ( lookTarget ) {
                actor.transform.rotation = Quaternion.LookRotation(pos - actor.transform.position); ;
            }
        }


        protected override void OnReverse() {
            actor.transform.position = wasPosition;
            actor.transform.rotation = wasRotation;
        }



        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnDrawGizmosSelected() {
            var pos = TransformPosition(targetPosition.value, targetPosition.space);
            Gizmos.DrawLine(actor.transform.position, pos);
        }

#endif
    }
}