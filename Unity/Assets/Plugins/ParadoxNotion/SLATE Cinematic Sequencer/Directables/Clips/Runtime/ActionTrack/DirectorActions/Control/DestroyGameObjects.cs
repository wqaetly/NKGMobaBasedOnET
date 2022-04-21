using UnityEngine;
using System.Collections.Generic;

namespace Slate.ActionClips
{

    [Category("Control")]
    [Description("Destroy all gameobjects in the list (PlayMode only).\nYou should NOT use this clip to destroy actors.")]
    public class DestroyGameObjects : DirectorActionClip
    {

        public List<GameObject> targetObjects = new List<GameObject>();

        public override string info {
            get { return string.Format("Destroy\n({0}) GameObjects", targetObjects.Count); }
        }

        protected override void OnEnter() {
            if ( Application.isPlaying ) {
                foreach ( var o in targetObjects ) {
                    if ( o != null ) {
                        Destroy(o);
                    }
                }
            }
        }
    }
}