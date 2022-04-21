using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("Events")]
    [Description("Raise a Unity Event when the time is moving forwards and another when the time is moving backwards. This is helpfull if you want to use these events with UI to reverse their state, both in runtime and in editor.")]
    public class RaiseUnityEvent : DirectorActionClip
    {

        public string customLabel;
        public UnityEvent forwardEvent = new UnityEvent();
        public UnityEvent reverseEvent = new UnityEvent();

        public override string info {
            get
            {
                if ( !string.IsNullOrEmpty(customLabel) ) {
                    return customLabel;
                }
                var count1 = forwardEvent.GetPersistentEventCount();
                var count2 = reverseEvent.GetPersistentEventCount();
                var label1 = count1 > 0 ? "" : "No Event";
                var label2 = count2 > 0 ? "" : "No Event";
                if ( count1 > 0 ) {
                    var eTarget = forwardEvent.GetPersistentTarget(0);
                    var eName = forwardEvent.GetPersistentMethodName(0);
                    label1 = string.Format("{0}: {1}", eTarget != null ? eTarget.name : "null", eName);
                }
                if ( count2 > 0 ) {
                    var eTarget = reverseEvent.GetPersistentTarget(0);
                    var eName = reverseEvent.GetPersistentMethodName(0);
                    label2 = string.Format("{0}: {1}", eTarget != null ? eTarget.name : "null", eName);
                }

                return string.Format("> {0}\n< {1}", label1, label2);
            }
        }

        protected override void OnEnter() {
            forwardEvent.Invoke();
        }

        protected override void OnReverse() {
            reverseEvent.Invoke();
        }
    }
}