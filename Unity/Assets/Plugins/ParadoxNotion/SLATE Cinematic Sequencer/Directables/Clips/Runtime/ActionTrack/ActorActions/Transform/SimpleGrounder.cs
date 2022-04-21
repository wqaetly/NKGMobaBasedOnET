using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Transform")]
    [Description("Grounds the actor gameobject to the nearest collider object beneath it up to the set Max Distance")]
    public class SimpleGrounder : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1;

        [Range(1, 100)]
        public float maxCheckDistance = 10;
        [Min(0)]
        public float offset = 0.01f;

        private RaycastHit hit;
        private Vector3 lastPos;

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        protected override void OnEnter() {
            lastPos = actor.transform.position;
        }

        protected override void OnUpdate(float time) {
            var pos = actor.transform.position + new Vector3(0, maxCheckDistance, 0);
            var hits = Physics.RaycastAll(new Ray(pos, Vector3.down), maxCheckDistance * 2);
            for ( var i = 0; i < hits.Length; i++ ) {
                var hit = hits[i];
                if ( hit.distance < maxCheckDistance && hit.transform != actor.transform ) {
                    pos.y = hit.point.y + offset;
                    actor.transform.position = pos;
                    break;
                }
            }
        }

        protected override void OnReverse() {
            actor.transform.position = lastPos;
        }
    }
}