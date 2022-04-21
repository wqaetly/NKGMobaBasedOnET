using UnityEngine;
using System.Collections.Generic;

namespace Slate.ActionClips
{

    [Category("Transform")]
    [Description("Instantiate and attach a prefab object to a child transform of the actor (or the actor itself) either permantentely or temporary if length is greater than zero.")]
    public class AttachObjectPrefab : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;

        [Required]
        public Transform targetPrefab;
        public string childTransformName;
        public Vector3 localPosition;
        public Vector3 localRotation;
        public Vector3 localScale = Vector3.one;

        private Transform instance;
        private TransformSnapshot snapshot;
        private bool temporary;

        public override bool isValid {
            get { return targetPrefab != null; }
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
            instance = (Transform)Instantiate(targetPrefab);
            var finalTransform = actor.transform.FindInChildren(childTransformName, true);
            if ( finalTransform == null ) {
                Debug.LogError(string.Format("Child Transform with name '{0}', can't be found on actor '{1}' hierarchy", childTransformName, actor.name), actor.gameObject);
                return;
            }

            instance.SetParent(finalTransform);
            instance.localPosition = localPosition;
            instance.localEulerAngles = localRotation;
            instance.localScale = localScale;
        }

        void UnDo() {
            if ( instance != null ) {
                DestroyImmediate(instance.gameObject);
                instance = null;
            }
        }
    }
}