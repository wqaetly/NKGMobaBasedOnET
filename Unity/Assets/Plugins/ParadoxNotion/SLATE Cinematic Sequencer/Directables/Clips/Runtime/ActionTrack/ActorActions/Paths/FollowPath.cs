using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Paths")]
    [Description("Put the actor on a path to follow for the duration of the clip from path start to path end, or by using speed if 'Use Speed' is true. If you want the actor to look ahead of the path, set 'Look Ahead' to any value other than 0.")]
    public class FollowPath : ActorActionClip
    {

        [SerializeField, HideInInspector]
        private float _length = 5f;
        [SerializeField, HideInInspector]
        private float _blendIn = 0f;
        [SerializeField, HideInInspector]
        private float _blendOut = 0f;

        [Required]
        public Path path;
        public bool useSpeed = false;
        [ShowIf("useSpeed", 1)]
        public float speed = 3f;
        [Range(0, 1)]
        public float lookAhead = 0f;
        public Vector3 upVector = Vector3.up;
        public EaseType blendInterpolation = EaseType.QuadraticInOut;

        private Vector3 lastPos;
        private Quaternion lastRot;

        public override string info {
            get { return string.Format("Follow Path\n{0}", path != null ? path.name : "NONE"); }
        }

        public override float length {
            get { return useSpeed && path != null ? path.length / Mathf.Max(speed, 0.01f) : _length; }
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

        public override bool isValid {
            get { return path != null; }
        }

        protected override void OnEnter() {
            path.Compute();
            lastPos = actor.transform.position;
            lastRot = actor.transform.rotation;
        }

        protected override void OnUpdate(float time) {
            if ( length == 0 ) {
                actor.transform.position = path.GetPointAt(0);
                return;
            }

            var newPos = path.GetPointAt(time / length);
            actor.transform.position = Easing.Ease(blendInterpolation, lastPos, newPos, GetClipWeight(time));

            if ( lookAhead > 0 ) {
                var lookPos = path.GetPointAt(( time / length ) + lookAhead);
                var dir = lookPos - actor.transform.position;
                if ( dir.magnitude > 0.001f ) {
                    var lookRot = Quaternion.LookRotation(dir, upVector);
                    actor.transform.rotation = Easing.Ease(blendInterpolation, lastRot, lookRot, GetClipWeight(time));
                }
            }
        }

        protected override void OnReverse() {
            actor.transform.position = lastPos;
            actor.transform.rotation = lastRot;
        }
    }
}