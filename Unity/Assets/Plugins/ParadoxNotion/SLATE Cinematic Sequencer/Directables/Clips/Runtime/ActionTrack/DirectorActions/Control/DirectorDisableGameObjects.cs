using UnityEngine;
using System.Collections.Generic;

namespace Slate.ActionClips
{

    [Category("Control")]
    [Name("Disable Game Objects")]
    [Description("All gameobjects in the list will be disabled if not already")]
    public class DirectorDisableGameObjects : DirectorActionClip
    {

        public List<GameObject> targetObjects = new List<GameObject>();
        private Dictionary<GameObject, bool> states;

        public override string info {
            get { return string.Format("Disable\n({0}) GameObjects", targetObjects.Count); }
        }

        protected override void OnEnter() {
            states = new Dictionary<GameObject, bool>();
            foreach ( var o in targetObjects ) {
                states[o] = o.activeSelf;
                o.SetActive(false);
            }
        }

        protected override void OnReverse() {
            foreach ( var pair in states ) {
                if ( pair.Key != null ) {
                    pair.Key.SetActive(pair.Value);
                }
            }
        }
    }
}