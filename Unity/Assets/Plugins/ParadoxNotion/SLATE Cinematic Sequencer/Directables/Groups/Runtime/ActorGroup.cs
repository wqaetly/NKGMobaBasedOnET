using UnityEngine;
using System.Collections.Generic;

namespace Slate
{

    [Description("All tracks of an Actor Group affect a specific actor GameObject or one of it's Components. Specifying a name manually comes in handy if you want to set the target actor of this group via scripting. The ReferenceMode along with InitialCoordinates are essential when you are working with prefab actors.")]
    public class ActorGroup : CutsceneGroup
    {

        [SerializeField]
        private string _name;
        [SerializeField]
        private GameObject _actor;

        [SerializeField]
        private ActorReferenceMode _referenceMode;

        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("_initialTransformation")]
        private ActorInitialTransformation _initialCoordinates;
        [SerializeField]
        [ShowIf("_initialCoordinates", 1)]
        private Vector3 _initialLocalPosition;
        [SerializeField]
        [ShowIf("_initialCoordinates", 1)]
        private Vector3 _initialLocalRotation;
        [SerializeField]
        [ShowIf("_initialCoordinates", 1)]
        private bool _displayVirtualMeshGizmo = true;

        public override string name {
            get { return string.IsNullOrEmpty(_name) ? ( actor != null ? actor.name : null ) : _name; }
            set { _name = value; }
        }

        public override GameObject actor {
            get { return base.ResolveActor(_actor); }
            set
            {
                if ( _actor != value ) {
                    _actor = value;

#if UNITY_EDITOR
                    if ( value != null && !Application.isPlaying ) {
                        if ( UnityEditor.EditorUtility.IsPersistent(value) ) {
                            referenceMode = ActorReferenceMode.UseInstanceHideOriginal;
                            initialTransformation = ActorInitialTransformation.UseLocal;

                        }
                    }
#endif

                }
            }
        }

        public override ActorReferenceMode referenceMode {
            get { return _referenceMode; }
            set { _referenceMode = value; }
        }

        public override ActorInitialTransformation initialTransformation {
            get { return _initialCoordinates; }
            set { _initialCoordinates = value; }
        }

        public override Vector3 initialLocalPosition {
            get { return _initialLocalPosition; }
            set { _initialLocalPosition = value; }
        }

        public override Vector3 initialLocalRotation {
            get { return _initialLocalRotation; }
            set { _initialLocalRotation = value; }
        }

        public override bool displayVirtualMeshGizmo {
            get { return _displayVirtualMeshGizmo; }
            set { _displayVirtualMeshGizmo = value; }
        }
    }
}