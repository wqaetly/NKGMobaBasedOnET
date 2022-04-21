using UnityEngine;
using System.Collections;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("Events")]
    [Description("Send a Unity Message to all actors of this Cutscene, including the Director Camera, as well as the Cutscene itself.")]
    public class SendGlobalMessage : DirectorActionClip, IEvent
    {

        [Required]
        public string message;

        public override string info {
            get { return string.Format("Global Message\n'{0}'", message); }
        }

        public override bool isValid {
            get { return !string.IsNullOrEmpty(message); }
        }

        string IEvent.name {
            get { return message; }
        }

        void IEvent.Invoke() {
            OnEnter();
        }

        protected override void OnEnter() {

            if ( !Application.isPlaying ) {
                return;
            }

            this.root.SendGlobalMessage(message, null);
        }
    }
}