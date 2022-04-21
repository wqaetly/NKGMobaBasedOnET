#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Linq;

namespace Slate
{

    [CustomEditor(typeof(CutsceneTrack), true)]
    public class CutsceneTrackInspector : Editor
    {

        private CutsceneTrack track {
            get { return (CutsceneTrack)target; }
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            /*		
                        if (track is IKeyable){
                            var keyable = track as IKeyable;
                            if (keyable.animationData != null && keyable.animationData.isValid){
                                for (var i = 0; i < keyable.animationData.animatedParameters.Count; i++){
                                    GUILayout.Space(2);
                                    AnimatableParameterEditor.ShowParameter(keyable.animationData.animatedParameters[i], keyable);
                                }
                            }
                        }
            */
        }
    }
}

#endif