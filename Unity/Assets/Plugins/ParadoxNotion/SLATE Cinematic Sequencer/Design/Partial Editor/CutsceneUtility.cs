#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate
{

    ///Utilities specific to Cutscenes
    public static class CutsceneUtility
    {

        [System.NonSerialized]
        private static string copyJson;
        [System.NonSerialized]
        private static System.Type copyType;
        [System.NonSerialized]
        private static IDirectable _selectedObject;
        [System.NonSerialized]
        public static Dictionary<AnimatedParameter, ChangedParameterCallbacks> changedParameterCallbacks = new Dictionary<AnimatedParameter, ChangedParameterCallbacks>();

        ///Raised when directable selection change
        public static event System.Action<IDirectable> onSelectionChange;
        ///Raised when animation editors refresh
        public static event System.Action<IAnimatableData> onRefreshAllAnimationEditors;

        public struct ChangedParameterCallbacks
        {
            public System.Action Restore;
            public System.Action Commit;
            public ChangedParameterCallbacks(System.Action restore, System.Action commit) {
                Restore = restore;
                Commit = commit;
            }
        }

        ///Returns the current cutscene that is being edited in the editor
        public static Cutscene cutsceneInEditor {
            get
            {
                var editor = ReflectionTools.GetType("CutsceneEditor").RTGetFieldOrProp("current").RTGetFieldOrPropValue(null);
                if ( editor != null ) {
                    return editor.GetType().RTGetFieldOrProp("cutscene").RTGetFieldOrPropValue(editor) as Cutscene;
                }
                return null;
            }
        }

        ///Currently selected directable element
        public static IDirectable selectedObject {
            get { return _selectedObject; }
            set
            {
                //select the root cutscene which in turns display the inspector of the object within it.
                if ( value != null ) { UnityEditor.Selection.activeObject = value.root.context; }
                _selectedObject = value;
                if ( onSelectionChange != null ) {
                    onSelectionChange(value);
                }
            }
        }

        ///Refresh animation editors (dopesheet, curves) of targer animatable
        public static void RefreshAllAnimationEditorsOf(IAnimatableData animatable) {
            if ( onRefreshAllAnimationEditors != null && animatable != null ) {
                onRefreshAllAnimationEditors(animatable);
            }
        }

        ///Returns the currently copied clip type
        public static System.Type GetCopyType() {
            return copyType;
        }

        ///Flush the copy data
        public static void FlushCopy() {
            copyType = null;
            copyJson = null;
        }

        ///Copy a clip
        public static void CopyClip(ActionClip clip) {
            copyJson = JsonUtility.ToJson(clip, false);
            copyType = clip.GetType();
        }

        ///Cut a clip
        public static void CutClip(ActionClip clip) {
            copyJson = JsonUtility.ToJson(clip, false);
            copyType = clip.GetType();
            ( clip.parent as CutsceneTrack ).DeleteAction(clip);
        }

        ///Paste a previously copied clip. Creates a new clip with copied values within the target track.
        public static ActionClip PasteClip(CutsceneTrack track, float time) {
            if ( copyType != null && !string.IsNullOrEmpty(copyJson) ) {
                var newAction = track.AddAction(copyType, time);
                JsonUtility.FromJsonOverwrite(copyJson, newAction);
                newAction.startTime = time;

                var nextAction = track.clips.FirstOrDefault(a => a.startTime > newAction.startTime);
                if ( nextAction != null && newAction.endTime > nextAction.startTime ) {
                    newAction.endTime = nextAction.startTime;
                }

                return newAction;
            }
            return null;
        }


        ///Copies the object's values to editor prefs json
		public static void CopyClipValues(ActionClip clip) {
            var json = JsonUtility.ToJson(clip);
            EditorPrefs.SetString("Slate_CopyDirectableValuesJSON", json);
        }

        ///Pastes the object's values from editor prefs json
        public static void PasteClipValues(ActionClip clip) {
            var json = EditorPrefs.GetString("Slate_CopyDirectableValuesJSON");
            var wasStartTime = clip.startTime;
            var wasEndTime = clip.endTime;
            var wasBlendIn = clip.blendIn;
            var wasBlendOut = clip.blendOut;
            JsonUtility.FromJsonOverwrite(json, clip);
            clip.startTime = wasStartTime;
            clip.endTime = wasEndTime;
            clip.blendIn = wasBlendIn;
            clip.blendOut = wasBlendOut;
        }

        ///Is any object copied?
        public static bool HasCopyDirectableValues() {
            var json = EditorPrefs.GetString("Slate_CopyDirectableValuesJSON");
            return !string.IsNullOrEmpty(json);
        }

    }
}

#endif