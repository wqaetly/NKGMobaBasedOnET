using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Transform")]
    [System.Obsolete("Use Set Parent")]
    public class SetParentTemporary : ActorActionClip
    {

        public float _length = 1f;
        public Transform newParent;
        public bool matchPosition = false;
        public bool matchRotation = false;
        public bool matchScale = false;

        private Transform originalParent;
        private Vector3 originalPos;
        private Quaternion originalRot;
        private Vector3 originalScale;

        public override string info {
            get { return string.Format("Set Parent Temporary\n{0}", newParent != null ? newParent.name : "none"); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        protected override void OnEnter() {
            originalParent = actor.transform.parent;
            originalPos = actor.transform.localPosition;
            originalRot = actor.transform.localRotation;
            originalScale = actor.transform.localScale;
        }

        protected override void OnUpdate(float deltaTime) {

            if ( deltaTime < length ) {
                actor.transform.SetParent(newParent, true);
                if ( matchPosition ) {
                    actor.transform.localPosition = Vector3.zero;
                }
                if ( matchRotation ) {
                    actor.transform.localEulerAngles = Vector3.zero;
                }
                if ( matchScale ) {
                    actor.transform.localScale = Vector3.one;
                }
            } else {
                actor.transform.SetParent(originalParent, true);
            }
        }

        protected override void OnReverse() {
            actor.transform.SetParent(originalParent, true);
            actor.transform.localPosition = originalPos;
            actor.transform.localRotation = originalRot;
            actor.transform.localScale = originalScale;
        }
    }
}