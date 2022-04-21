using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("Renderer")]
    public class SetMaterial : ActorActionClip<Renderer>
    {

        [SerializeField]
        [HideInInspector]
        private float _length;

        public Material material;

        private Material sharedMat;
        private bool temporary;

        public override string info {
            get { return string.Format("Set Material\n{0}", material ? material.name : "null"); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        protected override void OnEnter() { temporary = length > 0; DoSet(); }
        protected override void OnReverseEnter() { if ( temporary ) DoSet(); }

        protected override void OnReverse() { DoReset(); }
        protected override void OnExit() { if ( temporary ) DoReset(); }


        void DoSet() {
            sharedMat = actor.sharedMaterial;
            actor.material = material;
        }

        void DoReset() {
            actor.sharedMaterial = sharedMat;
        }
    }
}