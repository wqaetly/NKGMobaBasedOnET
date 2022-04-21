using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Transform")]
    [Description("Rotate actor transform to look at specified target position for a period of time or permanentely if blend out is zero")]
    public class LookAt : ActorActionClip
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

        public bool verticalOnly;
        public EaseType interpolation = EaseType.QuadraticInOut;
        public TransformRefPosition targetPosition;

        private Quaternion wasRotation;

        [AnimatableParameter(link = "targetPosition")]
        [ShowTrajectory]
        [PositionHandle]
        public Vector3 targetPositionVector {
            get { return targetPosition.value; }
            set { targetPosition.value = value; }
        }

        public override string info {
            get { return string.Format("Look At {0}", targetPosition.useAnimation ? "" : targetPosition.ToString()); }
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
            SetParameterEnabled("targetPositionVector", targetPosition.useAnimation);
        }

        protected override void OnEnter() {
            wasRotation = actor.transform.rotation;
        }

        protected override void OnUpdate(float deltaTime) {
            var pos = TransformPosition(targetPosition.value, targetPosition.space);
            if ( verticalOnly ) {
                pos.y = actor.transform.position.y;
            }
            var dir = pos - actor.transform.position;
            if ( dir.magnitude > 0.001f ) {
                var lookRot = Quaternion.LookRotation(dir);
                actor.transform.rotation = Easing.Ease(interpolation, wasRotation, lookRot, GetClipWeight(deltaTime));
            }
        }

        protected override void OnReverse() {
            actor.transform.rotation = wasRotation;
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnDrawGizmosSelected() {
            var pos = TransformPosition(targetPosition.value, targetPosition.space);
            Gizmos.color = new Color(1, 1, 1, GetClipWeight());
            Gizmos.DrawLine(actor.transform.position, pos);
            Gizmos.color = Color.white;
        }

#endif
    }
}