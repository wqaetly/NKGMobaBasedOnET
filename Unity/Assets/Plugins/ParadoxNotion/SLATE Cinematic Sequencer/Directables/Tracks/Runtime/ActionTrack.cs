using UnityEngine;
using System.Collections;
using System.Linq;

namespace Slate
{

    ///Action Tracks contain general purpose ActionClips
    [Name("Action Track")]
    [Description("Action Tracks are generic purpose tracks. Once an Action Clip has been placed, the Action Track will lock to accept only clips of the same category.")]
    [Icon(typeof(CircleCollider2D))]
    abstract public class ActionTrack : CutsceneTrack
    {

#if UNITY_EDITOR

        [SerializeField]
        protected Texture _icon;

        public override Texture icon {
            get { return _icon != null ? _icon : base.icon; }
        }

#endif
    }
}