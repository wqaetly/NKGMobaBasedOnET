using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

namespace Slate.ActionClips
{

    [Category("Events")]
    [Description("Send a Unity Message to the actor")]
    public class SendMessage : ActorActionClip
    {

        [Required, Tooltip("The method name to call")]
        public string message;
        [Tooltip("If multiple components of the same type exist on gameobject you can specify the index of the component to use here. Leave at -1 to call all found instances")]
        public int componentIndex = -1;

        public override string info => "Message\n" + message;
        public override bool isValid => !string.IsNullOrEmpty(message);
        virtual public System.Type parameterType => null;


        protected override void OnEnter() {

            Debug.Log(string.Format("<b>({0}) Actor Message Send:</b> '{1}'", actor.name, message));

            var received = false;

            var split = message.Split('.');
            var cType = split.Length == 2 ? ReflectionTools.GetType(split[0]) : typeof(Component);
            if ( cType == null ) {
                Debug.LogError("Component Type Not Found");
                return;
            }

            var components = actor.GetComponents(cType);
            for ( var i = 0; i < components.Length; i++ ) {
                if ( componentIndex <= -1 || componentIndex == i ) {
                    var c = components[i];
                    var methodName = split.Length == 2 ? split[1] : split[0];
                    var m = c.GetType().RTGetMethod(methodName);
                    if ( m != null && m.GetParameters().Length == 0 ) {
                        received = true;
                        m.Invoke(c, null);
                    }
                }
            }

            if ( !received ) { Debug.LogError("Message had no receiver"); }
        }
    }
}