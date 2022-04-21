#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Linq;

namespace Slate
{

    [CustomEditor(typeof(CutsceneGroup), true)]
    public class CutsceneGroupInspector : Editor
    {

        private CutsceneGroup group {
            get { return (CutsceneGroup)target; }
        }

        public override void OnInspectorGUI() {
            GUI.enabled = group.root.currentTime == 0;
            base.OnInspectorGUI();
        }
    }
}

#endif