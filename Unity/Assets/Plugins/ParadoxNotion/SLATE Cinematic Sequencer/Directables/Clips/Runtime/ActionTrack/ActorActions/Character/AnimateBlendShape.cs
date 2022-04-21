using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("Character")]
    [Description("Animates a single blend shape chosen from within the whole actor gameobject hierarchy.")]
    public class AnimateBlendShape : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.25f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0.25f;

        [SerializeField]
        [HideInInspector]
        private string _skinName;
        [SerializeField]
        [HideInInspector]
        private string _shapeName;

        [AnimatableParameter(-1, 2)]
        public float weight = 1;

        private float originalWeight;
        private int index;

        public override string info {
            get { return string.Format("Animate '{0}'", shapeName); }
        }

        public override bool isValid {
            get { return skinnedMesh != null && skinnedMesh.GetBlendShapeIndex(shapeName) != -1; }
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

        public string skinName {
            get { return _skinName; }
            set { _skinName = value; _skinnedMesh = null; }
        }

        public string shapeName {
            get { return _shapeName; }
            set { _shapeName = value; }
        }

        private SkinnedMeshRenderer _skinnedMesh;
        private SkinnedMeshRenderer skinnedMesh {
            get
            {
                if ( actor != null && _skinnedMesh == null ) {
                    _skinnedMesh = actor.GetComponentsInChildren<SkinnedMeshRenderer>().ToList().Find(s => s.name == skinName);
                }
                return _skinnedMesh;
            }
        }

        protected override void OnEnter() {
            _skinnedMesh = null; //flush
            index = skinnedMesh != null ? skinnedMesh.GetBlendShapeIndex(shapeName) : -1;
            if ( index != -1 ) {
                originalWeight = skinnedMesh.GetBlendShapeWeight(index);
            }
        }

        protected override void OnUpdate(float deltaTime) {
            if ( index != -1 ) {
                var value = Easing.Ease(EaseType.QuadraticInOut, originalWeight, weight, GetClipWeight(deltaTime));
                skinnedMesh.SetBlendShapeWeight(index, value * 100);
            }
        }

        protected override void OnReverse() {
            if ( index != -1 ) {
                skinnedMesh.SetBlendShapeWeight(index, originalWeight);
            }
        }
    }
}