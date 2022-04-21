#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Slate
{

    ///A popup window to select a camera shot with a preview
    public class PreferencesWindow : PopupWindowContent
    {

        private static Rect myRect;
        private bool firstPass = true;

        ///Shows the popup menu at position and with title
        public static void Show(Rect rect) {
            myRect = rect;
            PopupWindow.Show(new Rect(rect.x, rect.y, 0, 0), new PreferencesWindow());
        }

        public override Vector2 GetWindowSize() { return new Vector2(myRect.width, myRect.height); }
        public override void OnGUI(Rect rect) {

            GUILayout.BeginVertical("box");

            GUI.color = new Color(0, 0, 0, 0.3f);
            GUILayout.BeginHorizontal(Slate.Styles.headerBoxStyle);
            GUI.color = Color.white;
            GUILayout.Label("<size=22><b>Global Editor Preferences</b></size>");
            GUILayout.EndHorizontal();
            GUILayout.Space(2);

            GUILayout.BeginVertical("box");
            Prefs.timeStepMode = (Prefs.TimeStepMode)EditorGUILayout.EnumPopup("Time Step Mode", Prefs.timeStepMode);
            if ( Prefs.timeStepMode == Prefs.TimeStepMode.Seconds ) {
                Prefs.snapInterval = EditorTools.CleanPopup<float>("Working Snap Interval", Prefs.snapInterval, Prefs.snapIntervals.ToList());
            } else {
                Prefs.frameRate = EditorTools.CleanPopup<int>("Working Frame Rate", Prefs.frameRate, Prefs.frameRates.ToList());
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            Prefs.magnetSnapping = EditorGUILayout.Toggle("Clips Magnet Snapping", Prefs.magnetSnapping);
            Prefs.lockHorizontalCurveEditing = EditorGUILayout.Toggle(new GUIContent("Lock xAxis Curve Editing", "Disallows moving keys in x in CurveEditor. They can still be moved in DopeSheetEditor"), Prefs.lockHorizontalCurveEditing);
            Prefs.autoFirstKey = EditorGUILayout.Toggle(new GUIContent("Auto First Key", "If enabled, will automatically add a keyframe at time 0 of the animated property"), Prefs.autoFirstKey);
            Prefs.autoCleanKeysOffRange = EditorGUILayout.Toggle(new GUIContent("Auto Clean Keys", "If enabled, will automatically clean keys off clip range"), Prefs.autoCleanKeysOffRange);
            Prefs.showDopesheetKeyValues = EditorGUILayout.Toggle("Show Keyframe Values", Prefs.showDopesheetKeyValues);
            Prefs.defaultTangentMode = (TangentMode)EditorGUILayout.EnumPopup("Initial Keyframe Tangent", Prefs.defaultTangentMode);
            Prefs.keyframesStyle = (Prefs.KeyframesStyle)EditorGUILayout.EnumPopup("Keyframes Style", Prefs.keyframesStyle);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            Prefs.showShotThumbnails = EditorGUILayout.Toggle("Show Shot Thumbnails", Prefs.showShotThumbnails);
            if ( Prefs.showShotThumbnails ) {
                Prefs.thumbnailsRefreshInterval = EditorGUILayout.IntSlider(new GUIContent("Thumbnails Refresh", "The interval between which thumbnails refresh in editor frames"), Prefs.thumbnailsRefreshInterval, 2, 100);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            Prefs.scrollWheelZooms = EditorGUILayout.Toggle("Scroll Wheel Zooms", Prefs.scrollWheelZooms);
            Prefs.showDescriptions = EditorGUILayout.Toggle("Show Help Descriptions", Prefs.showDescriptions);
            Prefs.gizmosLightness = EditorGUILayout.Slider("Gizmos Lightness", Prefs.gizmosLightness, 0, 1);
            Prefs.motionPathsColor = EditorGUILayout.ColorField("Motion Paths Color", Prefs.motionPathsColor);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            GUI.enabled = !Application.isPlaying;
            var usePostStack = DefinesManager.HasDefine(Prefs.USE_POSTSTACK_DEFINE);
            var newUsePostStack = EditorGUILayout.ToggleLeft(new GUIContent("Use Post Processing Stack V2 Define", "Enable this is you use Unity's PostProcessing Stack V2"), usePostStack);
            if ( newUsePostStack != usePostStack ) {
                if ( newUsePostStack == true ) {
                    EditorUtility.DisplayDialog("Post Processing Stack V2 Enabled", "Quick setup using Post Processing Stack 2 with Slate:\n\n1) Create a 'Global Post Process Volume' and set it to a new layer.\n\n2) Add a 'Post Processing Layer' Component to the 'Render Camera' and set the 'Layer' property to the one used in step 1.\n\n3) In the Director Camera inspector, set the 'Post Processing Volume Layer' to that same layer used in step 1 and 2.\n\nStep 3 is only Slate related step. For more information on step 1 and 2, please check the Post Processing Stack v2 documentation.", "OK");
                }
                DefinesManager.SetDefineActive(Prefs.USE_POSTSTACK_DEFINE, newUsePostStack);
            }

            //Needs more work
            // var useExpressions = DefinesManager.HasDefine(Prefs.USE_EXPRESSIONS_DEFINE);
            // var newUseExpressions = EditorGUILayout.ToggleLeft("Use Expressions Define (Very Experimental Feature!)", useExpressions);
            // if ( newUseExpressions != useExpressions ) {
            //     DefinesManager.SetDefineActive(Prefs.USE_EXPRESSIONS_DEFINE, newUseExpressions);
            // }

            GUI.enabled = true;
            GUILayout.EndVertical();


            GUI.backgroundColor = Prefs.autoKey ? new Color(1, 0.5f, 0.5f) : Color.white;
            if ( GUILayout.Button(new GUIContent(Prefs.autoKey ? "AUTOKEY IS ENABLED" : "AUTOKEY IS DISABLED", Styles.keyIcon)) ) {
                Prefs.autoKey = !Prefs.autoKey;
            }
            GUI.backgroundColor = Color.white;


            GUILayout.EndVertical();

            if ( firstPass || Event.current.type == EventType.Repaint ) {
                firstPass = false;
                myRect.height = GUILayoutUtility.GetLastRect().yMax + 5;
            }
        }
    }
}

#endif