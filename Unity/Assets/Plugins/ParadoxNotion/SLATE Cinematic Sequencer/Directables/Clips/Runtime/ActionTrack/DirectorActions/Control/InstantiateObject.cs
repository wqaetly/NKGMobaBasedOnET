using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Control")]
    [Description("Instantiates an object with optional popup animation if BlendIn is higher than zero. You can optionaly 'popdown' and destroy the object after a period of time, if you also set a BlendOut value higher than zero.")]
    public class InstantiateObject : DirectorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 2f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0f;

        [Required]
        [PlaybackProtected]
        public GameObject targetObject;
        public Transform optionalParent;
        public Vector3 targetPosition;
        public Vector3 targetRotation;
        public MiniTransformSpace space;
        public EaseType popupInterpolation = EaseType.ElasticInOut;

        private GameObject instance;
        private Vector3 wasScale;

        public override bool isValid {
            get { return targetObject != null; }
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

        public override string info {
            get { return string.Format("Instantiate\n{0}", targetObject != null ? targetObject.name : "NULL"); }
        }

        new GameObject actor {
            get { return instance; }
        }

        protected override void OnEnter() {
            wasScale = targetObject.transform.localScale;
            instance = (GameObject)Instantiate(targetObject);
            SceneManager.MoveGameObjectToScene(instance, optionalParent != null ? optionalParent.gameObject.scene : root.context.scene);
            instance.transform.parent = optionalParent;

            // instance.transform.position = TransformPosition(targetPosition, (TransformSpace)space );
            // instance.transform.rotation = TransformRotation(targetRotation, (TransformSpace)space );

            ///REMARK: This is a special case since the created instance is not the actor of this clip (since it's a Director Clip),
            ///but we need do transformation based on the instance.
            var spaceTransform = this.GetSpaceTransform((TransformSpace)space, instance);
            instance.transform.position = spaceTransform != null ? spaceTransform.TransformPoint(targetPosition) : targetPosition;
            instance.transform.rotation = spaceTransform != null ? spaceTransform.rotation * Quaternion.Euler(targetRotation) : Quaternion.Euler(targetRotation);
        }

        protected override void OnUpdate(float time) {
            if ( instance != null ) {
                instance.transform.localScale = Easing.Ease(popupInterpolation, Vector3.zero, wasScale, GetClipWeight(time));
            }
        }

        protected override void OnExit() {
            if ( blendOut > 0 ) {
                DestroyImmediate(instance, false);
            }
        }

        protected override void OnReverseEnter() {
            if ( blendOut > 0 ) {
                OnEnter();
            }
        }

        protected override void OnReverse() {
            DestroyImmediate(instance, false);
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnSceneGUI() {
            if ( optionalParent == null ) {
                DoVectorPositionHandle((TransformSpace)space, ref targetPosition);
                DoVectorRotationHandle((TransformSpace)space, targetPosition, ref targetRotation);
            }
        }

#endif

    }
}