using UnityEngine;
using System.Collections.Generic;

namespace Slate.ActionClips
{

    [Category("Transform")]
    [Description("Attach an object to a child transform of the actor (or the actor itself) either permantentely or temporary if length is greater than zero.")]
    public class AttachObject : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;

        [Required]
        public Transform targetObject;
        public string childTransformName;
        public Vector3 localPosition;
        public Vector3 localRotation;
        public Vector3 localScale = Vector3.one;

        private TransformSnapshot snapshot;
        private bool temporary;

        public override bool isValid {
            get { return targetObject != null; }
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
            snapshot = new TransformSnapshot(targetObject.gameObject, TransformSnapshot.StoreMode.RootOnly);
            var finalTransform = actor.transform.FindInChildren(childTransformName, true);
            if ( finalTransform == null ) {
                Debug.LogError(string.Format("Child Transform with name '{0}', can't be found on actor '{1}' hierarchy", childTransformName, actor.name), actor.gameObject);
                return;
            }

            targetObject.SetParent(finalTransform);
            targetObject.localPosition = localPosition;
            targetObject.localEulerAngles = localRotation;
            targetObject.localScale = localScale;
        }

        void UnDo() {
            snapshot.Restore();
        }
    }
}