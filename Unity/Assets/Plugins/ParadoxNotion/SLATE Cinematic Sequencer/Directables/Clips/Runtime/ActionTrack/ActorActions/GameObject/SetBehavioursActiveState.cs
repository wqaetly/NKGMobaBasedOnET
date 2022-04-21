using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("GameObject")]
    [Description("The selected Behaviours will be Enabled or Disable (based on state option) on the actor if they are not already")]
    public class SetBehavioursActiveState : ActorActionClip
    {

        [SerializeField]
        [HideInInspector]
        private float _length;

        [HideInInspector]
        public List<string> behaviourNames = new List<string>();
        public ActiveState activeState = ActiveState.Enable;

        private Dictionary<Behaviour, bool> originalStates;
        private Dictionary<Behaviour, bool> currentStates;
        private bool temporary;

        public override string info {
            get { return string.Format("{0}\n({1}) Behaviours", activeState.ToString(), behaviourNames.Count); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        protected override void OnEnter() {
            originalStates = new Dictionary<Behaviour, bool>();
            currentStates = new Dictionary<Behaviour, bool>();
            foreach ( var behaviour in actor.GetComponents<Behaviour>().Where(c => behaviourNames.Contains(c.GetType().Name)) ) {
                originalStates[behaviour] = behaviour.enabled;
                if ( activeState == ActiveState.Toggle ) {
                    behaviour.enabled = !behaviour.enabled;
                } else {
                    behaviour.enabled = activeState == ActiveState.Enable;
                }
                currentStates[behaviour] = behaviour.enabled;
                temporary = length > 0;
            }
        }

        protected override void OnExit() {
            if ( temporary ) {
                foreach ( var pair in originalStates ) {
                    if ( pair.Key != null ) {
                        pair.Key.enabled = !currentStates[pair.Key];
                    }
                }
            }
        }

        protected override void OnReverseEnter() {
            if ( temporary ) {
                foreach ( var pair in originalStates ) {
                    if ( pair.Key != null ) {
                        pair.Key.enabled = currentStates[pair.Key];
                    }
                }
            }
        }

        protected override void OnReverse() {
            foreach ( var pair in originalStates ) {
                if ( pair.Key != null ) {
                    pair.Key.enabled = pair.Value;
                }
            }
        }
    }
}