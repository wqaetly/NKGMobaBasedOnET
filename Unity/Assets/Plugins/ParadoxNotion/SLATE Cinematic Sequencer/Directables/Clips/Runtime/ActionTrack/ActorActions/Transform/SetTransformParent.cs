using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Transform")]
    [Description("Set the parent of the actor gameobject temporarily, or permanently if length is zero")]
    public class SetTransformParent : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length;

        public Transform newParent;
        public bool resetPosition = false;
        public bool resetRotation = false;
        public bool resetScale = false;

        private Transform originalParent;
        private Vector3 originalPos;
        private Quaternion originalRot;
        private Vector3 originalScale;
        private bool temporary;

        public override string info {
            get { return string.Format("Set Parent\n{0}", newParent != null ? newParent.name : "none"); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        protected override void OnEnter() { temporary = length > 0; Do(); }
        protected override void OnReverseEnter() { if ( temporary ) { Do(); } }
        protected override void OnExit() { if ( temporary ) { UnDo(); } }
        protected override void OnReverse() { UnDo(); }

        void Do() {
            originalParent = actor.transform.parent;
            originalPos = actor.transform.localPosition;
            originalRot = actor.transform.localRotation;
            originalScale = actor.transform.localScale;

            actor.transform.SetParent(newParent, true);
            if ( resetPosition ) { actor.transform.localPosition = Vector3.zero; }
            if ( resetRotation ) { actor.transform.localEulerAngles = Vector3.zero; }
            if ( resetScale ) { actor.transform.localScale = Vector3.one; }
        }

        void UnDo() {
            actor.transform.SetParent(originalParent, true);
            actor.transform.localPosition = originalPos;
            actor.transform.localRotation = originalRot;
            actor.transform.localScale = originalScale;
        }
    }
}