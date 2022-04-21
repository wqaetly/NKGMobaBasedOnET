using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Description("CrossFades to an Animator state within a period of time.")]
    public class CrossFadeState : MecanimBaseClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;

        [Required]
        public string stateName;

        public override string info {
            get { return string.Format("CrossFade State\n'{0}'", stateName); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return length; }
        }

        protected override void OnEnter() {
            actor.CrossFade(stateName, length, -1, float.NegativeInfinity);
        }
    }
}