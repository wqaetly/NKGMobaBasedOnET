using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Transform")]
    public class TranslateTo : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1;

        public Vector3 targetPosition;
        public MiniTransformSpace space;
        public EaseType interpolation = EaseType.QuadraticInOut;

        private Vector3 wasPosition;

        public override string info {
            get { return string.Format("Translate To\n{0}", targetPosition); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return length; }
        }

        protected override void OnEnter() {
            wasPosition = actor.transform.position;
        }

        protected override void OnUpdate(float time) {
            var pos = TransformPosition(targetPosition, (TransformSpace)space);
            if ( length == 0 ) {
                actor.transform.position = pos;
                return;
            }
            actor.transform.position = Easing.Ease(interpolation, wasPosition, pos, time / length);
        }

        protected override void OnReverse() {
            actor.transform.position = wasPosition;
        }


#if UNITY_EDITOR
        protected override void OnSceneGUI() {
            DoVectorPositionHandle((TransformSpace)space, ref targetPosition);
        }
#endif
    }
}