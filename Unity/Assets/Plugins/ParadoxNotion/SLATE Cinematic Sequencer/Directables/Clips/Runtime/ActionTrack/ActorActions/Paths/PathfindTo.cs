using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif

namespace Slate.ActionClips
{

    [Category("Paths")]
    [Description("For this clip to work you only need to have a baked NavMesh. The actor does NOT need, or use a NavMeshAgent Component. The length of the clip is determined by the path's length and the speed parameter set, while the Blend In parameter is used only for the initial look ahead blending")]
    public class PathfindTo : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.5f;

        [Min(0.01f)]
        public float speed = 3f;
        public TransformRefPosition targetPosition;

        private NavMeshPath navPath;
        private Vector3[] pathPoints;

        private Vector3 wasPosition;
        private Quaternion wasRotation;

        private Vector3 lastFrom;
        private Vector3 lastTo;
        private bool lockCalc;

        public override string info {
            get { return string.Format("Pathfind To\n{0}", targetPosition.ToString()); }
        }

        public override float length {
            get
            {
                if ( isValid ) {
                    TryCalculatePath();
                    return Path.GetLength(pathPoints) / speed;
                }
                return 0;
            }
        }

        public override float blendIn {
            get { return length > 0 ? _blendIn : 0; }
            set { _blendIn = value; }
        }

        protected override void OnEnter() {
            lockCalc = false;
            TryCalculatePath();
            lockCalc = true;
            wasPosition = actor.transform.position;
            wasRotation = actor.transform.rotation;
            if ( pathPoints == null || pathPoints.Length == 0 ) {
                Debug.LogWarning(string.Format("Actor '{0}' can't pathfind to '{1}'", actor.name, targetPosition.value), actor);
            }
        }

        protected override void OnUpdate(float time) {
            if ( pathPoints != null && pathPoints.Length > 1 ) {

                if ( length == 0 ) {
                    actor.transform.position = Path.GetPoint(0, pathPoints);
                    return;
                }

                //POSITION
                var pos = actor.transform.position;
                pos = Path.GetPoint(time / length, pathPoints);
                NavMeshHit hit;
                if ( NavMesh.SamplePosition(pos, out hit, 10f, -1) ) {
                    pos.y = hit.position.y;
                }
                actor.transform.position = pos;


                //ROTATION
                var lookPos = Path.GetPoint(( time / length ) + 0.01f, pathPoints); //fix this!
                if ( blendIn > 0 && time <= blendIn ) {
                    var lookRot = Quaternion.LookRotation(lookPos - actor.transform.position);
                    actor.transform.rotation = Easing.Ease(EaseType.QuadraticInOut, wasRotation, lookRot, time / blendIn);
                } else {
                    actor.transform.LookAt(lookPos);
                }
                var angles = actor.transform.GetLocalEulerAngles();
                angles.x = 0;
                angles.z = 0;
                actor.transform.SetLocalEulerAngles(angles);
            }
        }

        protected override void OnReverse() {
            actor.transform.position = wasPosition;
            actor.transform.rotation = wasRotation;
            lockCalc = false;
        }

        void TryCalculatePath() {
            var pos = TransformPosition(targetPosition.value, targetPosition.space);
            if ( !lockCalc && ( navPath == null || lastFrom != actor.transform.position || lastTo != pos ) ) {
                navPath = new NavMeshPath();
                NavMesh.CalculatePath(actor.transform.position, pos, -1, navPath);
                pathPoints = navPath.corners.ToArray();
            }
            lastFrom = actor.transform.position;
            lastTo = pos;
        }


        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnDrawGizmosSelected() {

            var pos = TransformPosition(targetPosition.value, targetPosition.space);
            Gizmos.DrawSphere(pos, 0.2f);

            if ( pathPoints != null && pathPoints.Length > 1 ) {
                for ( int i = 0; i < pathPoints.Length; i++ ) {
                    var a = pathPoints[i];
                    var b = ( i == pathPoints.Length - 1 ) ? pathPoints[i] : pathPoints[i + 1];
                    Gizmos.DrawLine(a, b);
                }
            }
        }

        protected override void OnSceneGUI() {
            var value = targetPosition.value;
            DoVectorPositionHandle(targetPosition.space, ref value);
            targetPosition.value = value;
        }

#endif

    }
}