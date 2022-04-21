using UnityEngine;
using System.Collections;
using System;
using System.Linq;

namespace Slate.ActionClips
{

    abstract public class SendMessage<T> : SendMessage
    {

        [Tooltip("The parameter value to use when calling the method")]
        public T value;

        public override string info => string.Format("Message\n{0}({1})", message, value != null ? value.ToString() : "null");
        public override bool isValid => !string.IsNullOrEmpty(message);
        public override Type parameterType => typeof(T);

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
                    if ( m != null && m.GetParameters().Length == 1 && m.GetParameters().First().ParameterType == this.parameterType ) {
                        received = true;
                        m.Invoke(c, new object[] { value });
                    }
                }
            }

            if ( !received ) { Debug.LogError("Message had no receiver"); }
        }
    }
}