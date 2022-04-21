using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    public class AnimateTrigger : MecanimBaseClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;

        public string triggerName;
        [AnimatableParameter]
        public bool value;

        public override bool isValid {
            get { return base.isValid && HasParameter(triggerName); }
        }

        public override string info {
            get { return string.Format("'{0}' Trigger", triggerName); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        protected override void OnUpdate(float time) {
            if ( value ) {
                actor.SetTrigger(triggerName);
            } else {
                actor.ResetTrigger(triggerName);
            }
        }
    }
}