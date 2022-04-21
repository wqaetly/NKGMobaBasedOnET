using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Paths")]
    [Description("Animate the actor's position and look at target position on a Path. For example, a 'PositionOnPath' value of 0 means start of path, while a value of 1 means end of path.")]
    public class AnimateOnPath : ActorActionClip
    {

        [SerializeField, HideInInspector]
        private float _length = 5f;
        [SerializeField, HideInInspector]
        private float _blendIn = 0f;

        [Required]
        public Path path;
        [AnimatableParameter(0, 1)]
        public float positionOnPath;
        [AnimatableParameter, PositionHandle, ShowTrajectory]
        public Vector3 lookAtTargetPosition;
        public EaseType blendInterpolation = EaseType.QuadraticInOut;

        private Vector3 wasPosition;
        private Quaternion wasRotation;

        public override string info {
            get { return string.Format("Animate On Path '{0}'", path != null ? path.name : "NONE"); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return _blendIn; }
            set { _blendIn = value; }
        }

        public override bool isValid {
            get { return path != null; }
        }

        protected override void OnEnter() {
            path.Compute();
            wasPosition = actor.transform.position;
            wasRotation = actor.transform.rotation;
        }

        protected override void OnUpdate(float time) {

            if ( length == 0 ) {
                actor.transform.position = path.GetPointAt(positionOnPath);
                return;
            }

            var newPos = path.GetPointAt(positionOnPath);
            actor.transform.position = Easing.Ease(blendInterpolation, wasPosition, newPos, GetClipWeight(time));

            var lookPos = TransformPosition(lookAtTargetPosition, defaultTransformSpace);
            var dir = lookPos - actor.transform.position;
            if ( dir.magnitude > 0.001f ) {
                var lookRot = Quaternion.LookRotation(dir);
                actor.transform.rotation = Easing.Ease(blendInterpolation, wasRotation, lookRot, GetClipWeight(time));
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
            var pos = TransformPosition(lookAtTargetPosition, defaultTransformSpace);
            Gizmos.color = new Color(1, 1, 1, GetClipWeight());
            Gizmos.DrawLine(actor.transform.position, pos);
            Gizmos.color = Color.white;
        }

#endif
    }
}