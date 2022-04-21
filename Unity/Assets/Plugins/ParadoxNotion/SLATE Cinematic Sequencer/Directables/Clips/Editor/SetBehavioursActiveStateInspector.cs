#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate
{

    [CustomEditor(typeof(ActionClips.SetBehavioursActiveState))]
    public class SetBehavioursActiveStateInspector : ActionClipInspector<ActionClips.SetBehavioursActiveState>
    {

        public override void OnInspectorGUI() {

            base.OnInspectorGUI();

            if ( action.actor != null ) {

                var behaviours = action.actor.GetComponents<Behaviour>();
                if ( behaviours.Length == 0 ) {
                    EditorGUILayout.HelpBox("There are no Behaviours attached on the actor", MessageType.Warning);
                    return;
                }

                GUILayout.BeginVertical("box");
                GUILayout.Label("Choose Behaviours to affect:");

                foreach ( var b in behaviours ) {
                    var typeName = b.GetType().Name;
                    var toggle = action.behaviourNames.Contains(typeName);
                    toggle = EditorGUILayout.Toggle(typeName, toggle);
                    if ( toggle ) {
                        if ( !action.behaviourNames.Contains(typeName) ) {
                            action.behaviourNames.Add(typeName);
                        }
                    } else {
                        if ( action.behaviourNames.Contains(typeName) ) {
                            action.behaviourNames.Remove(typeName);
                        }
                    }
                }

                GUILayout.EndVertical();
            }
        }
    }
}

#endif