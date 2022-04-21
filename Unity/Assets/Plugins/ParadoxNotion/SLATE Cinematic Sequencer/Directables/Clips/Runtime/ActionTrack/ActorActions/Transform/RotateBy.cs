using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Transform")]
    [Description("Rotate the actor by specified degrees and optionaly per second")]
    public class RotateBy : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1;

        public Vector3 rotation = new Vector3(0, 90, 0);
        public bool perSecond;
        public EaseType interpolation = EaseType.QuadraticInOut;

        private Vector3 originalRot;

        public override string info {
            get { return string.Format("Rotate{0} By\n{1}", perSecond ? " Per Second" : "", rotation); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return length; }
        }

        protected override void OnEnter() {
            originalRot = actor.transform.GetLocalEulerAngles();
        }

        protected override void OnUpdate(float time) {
            var target = originalRot + ( rotation * ( perSecond ? length : 1 ) );
            actor.transform.SetLocalEulerAngles(Easing.Ease(interpolation, originalRot, target, GetClipWeight(time)));
        }

        protected override void OnReverse() {
            actor.transform.SetLocalEulerAngles(originalRot);
        }
    }
}