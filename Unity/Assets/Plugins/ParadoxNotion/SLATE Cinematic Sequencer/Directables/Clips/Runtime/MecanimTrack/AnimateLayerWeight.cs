using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Description("Animate an Animator layer weight to a value and back to previous value gradualy over a period of time.")]
    public class AnimateLayerWeight : MecanimBaseClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;
        [SerializeField]
        [HideInInspector]
        private float _blendIn = 0.2f;
        [SerializeField]
        [HideInInspector]
        private float _blendOut = 0.2f;

        public int layerIndex = 0;
        [AnimatableParameter(0, 1)]
        public float weight = 1;

        private float lastValue;

        public override string info {
            get { return string.Format("Layer '{0}' Weight", layerIndex); }
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

        protected override void OnEnter() {
            lastValue = actor.GetLayerWeight(layerIndex);
        }

        protected override void OnUpdate(float deltaTime) {
            actor.SetLayerWeight(layerIndex, Mathf.Lerp(lastValue, weight, GetClipWeight(deltaTime)));
        }

        protected override void OnReverse() {
            if ( Application.isPlaying ) {
                actor.SetLayerWeight(layerIndex, lastValue);
            }
        }
    }
}