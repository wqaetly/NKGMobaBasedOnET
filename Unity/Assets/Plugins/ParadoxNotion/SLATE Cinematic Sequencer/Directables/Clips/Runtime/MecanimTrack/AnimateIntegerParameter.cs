using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Description("Animate an integer Animator parameter to a value and back to previous value gradualy over a period of time.")]
    public class AnimateIntegerParameter : MecanimBaseClip
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

        public string parameterName;
        [AnimatableParameter]
        public int value;

        private int lastValue;

        public override bool isValid {
            get { return base.isValid && HasParameter(parameterName); }
        }

        public override string info {
            get { return string.Format("'{0}' Parameter", parameterName); }
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
            lastValue = actor.GetInteger(parameterName);
        }

        protected override void OnUpdate(float deltaTime) {
            actor.SetInteger(parameterName, (int)Mathf.Lerp(lastValue, value, GetClipWeight(deltaTime)));
        }

        protected override void OnReverse() {
            if ( Application.isPlaying ) {
                actor.SetInteger(parameterName, lastValue);
            }
        }
    }
}