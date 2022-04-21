using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("GameObject")]
    [System.Obsolete("Use 'Set Actor Visibility'")]
    public class SetActorVisibilityTemporary : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1f;

        public ActiveState activeState = ActiveState.Enable;

        private bool lastState;
        private bool currentState;
        private bool temporary;

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override string info {
            get { return string.Format("{0} Actor", activeState); }
        }

        protected override void OnEnter() {
            lastState = actor.activeSelf;
            if ( activeState == ActiveState.Toggle ) {
                actor.SetActive(!actor.activeSelf);
            } else {
                actor.SetActive(activeState == ActiveState.Enable);
            }
            currentState = actor.activeSelf;
            temporary = length > 0;
        }

        protected override void OnExit() {
            if ( temporary ) actor.SetActive(!currentState);
        }

        protected override void OnReverseEnter() {
            if ( temporary ) actor.SetActive(currentState);
        }

        protected override void OnReverse() {
            actor.SetActive(lastState);
        }
    }
}