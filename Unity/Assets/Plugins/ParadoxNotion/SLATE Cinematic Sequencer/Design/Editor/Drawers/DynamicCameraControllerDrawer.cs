#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Slate
{

    [CustomPropertyDrawer(typeof(DynamicCameraController))]
    public class DynamicCameraControllerDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) { return -2; }
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label) {

            var transposerProp = prop.FindPropertyRelative("_transposer");
            var tModeProp = prop.FindPropertyRelative("_transposer.trackingMode");
            var composerProp = prop.FindPropertyRelative("_composer");
            var cModeProp = prop.FindPropertyRelative("_composer.trackingMode");

            var tModeName = tModeProp.enumNames[tModeProp.enumValueIndex];
            var cModeName = cModeProp.enumNames[cModeProp.enumValueIndex];

            EditorGUI.indentLevel++;

            GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
            GUILayout.BeginVertical(Styles.clipBoxStyle);
            GUI.color = Color.white;
            EditorGUILayout.PropertyField(prop, new GUIContent("<b>Dynamic Shot Controller (BETA Feature)</b>"), false);
            if ( prop.isExpanded ) {

                EditorGUI.indentLevel++;

                //Transposer
                GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
                GUILayout.BeginVertical(Styles.clipBoxStyle);
                GUI.color = Color.white;

                var tLabel = string.Format("Position Constraint ({0})", tModeName);
                EditorGUILayout.PropertyField(transposerProp, new GUIContent(tLabel), false);
                if ( transposerProp.isExpanded ) {
                    EditorGUILayout.HelpBox("Position Constraint can be used to automate positioning of the shot.\nIf so, keyframing Position Parameter of the shot will be disabled.", MessageType.None);
                    EditorGUILayout.PropertyField(tModeProp, GUIContent.none);
                    if ( tModeProp.intValue != 0 ) {
                        var tTargetProp = prop.FindPropertyRelative("_transposer.target");
                        var tOffsetProp = prop.FindPropertyRelative("_transposer.targetOffset");
                        var tOffsetModeProp = prop.FindPropertyRelative("_transposer.offsetMode");
                        var tDampProp = prop.FindPropertyRelative("_transposer.smoothDamping");

                        var nullTarget = tTargetProp.objectReferenceValue == null;
                        GUI.backgroundColor = nullTarget ? Color.red : Color.white;
                        EditorGUILayout.PropertyField(tTargetProp);
                        GUI.backgroundColor = Color.white;
                        GUI.enabled = !nullTarget;

                        EditorGUILayout.PropertyField(tOffsetProp);
                        EditorGUILayout.PropertyField(tOffsetModeProp);

                        if ( tModeProp.intValue == 1 ) {
                            //...
                        }

                        if ( tModeProp.intValue == 2 ) {
                            var tRailPos1Prop = prop.FindPropertyRelative("_transposer.railStart");
                            var tRailPos2Prop = prop.FindPropertyRelative("_transposer.railEnd");
                            var tRailOffset = prop.FindPropertyRelative("_transposer.railOffset");
                            EditorGUILayout.PropertyField(tRailPos1Prop);
                            EditorGUILayout.PropertyField(tRailPos2Prop);
                            EditorGUILayout.PropertyField(tRailOffset);
                        }

                        EditorGUILayout.PropertyField(tDampProp);
                        GUI.enabled = true;
                    }
                }

                GUILayout.EndVertical();
                GUILayout.Space(2);

                //Composer
                GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
                GUILayout.BeginVertical(Styles.clipBoxStyle);
                GUI.color = Color.white;

                var cLabel = string.Format("Rotation Constraint ({0})", cModeName);
                EditorGUILayout.PropertyField(composerProp, new GUIContent(cLabel), false);

                if ( composerProp.isExpanded ) {
                    EditorGUILayout.HelpBox("Rotation Constraint can be used to automate rotation of the shot.\nIf so, keyframing Rotation Parameter of the shot will be disabled.", MessageType.None);
                    EditorGUILayout.PropertyField(cModeProp, GUIContent.none);
                    if ( cModeProp.intValue != 0 ) {
                        var cTargetProp = prop.FindPropertyRelative("_composer.target");
                        var cPointProp = prop.FindPropertyRelative("_composer.targetOffset");
                        var cSizeProp = prop.FindPropertyRelative("_composer.targetSize");
                        var cFrameCenterProp = prop.FindPropertyRelative("_composer.frameCenter");
                        var cFrameExtendsProp = prop.FindPropertyRelative("_composer.frameExtends");
                        var cDutchProp = prop.FindPropertyRelative("_composer.dutchTilt");
                        var cZoomFrame = prop.FindPropertyRelative("_composer.zoomAtTargetFrame");
                        var cDampProp = prop.FindPropertyRelative("_composer.smoothDamping");

                        var nullTarget = cTargetProp.objectReferenceValue == null;
                        GUI.backgroundColor = nullTarget ? Color.red : Color.white;
                        EditorGUILayout.PropertyField(cTargetProp);
                        GUI.backgroundColor = Color.white;
                        GUI.enabled = !nullTarget;
                        EditorGUILayout.PropertyField(cPointProp);
                        EditorGUILayout.PropertyField(cSizeProp);

                        EditorGUILayout.PropertyField(cFrameCenterProp);
                        var temp = cFrameCenterProp.vector2Value;
                        temp.x = Mathf.Clamp(temp.x, -0.5f, 0.5f);
                        temp.y = Mathf.Clamp(temp.y, -0.5f, 0.5f);
                        cFrameCenterProp.vector2Value = temp;

                        EditorGUILayout.PropertyField(cFrameExtendsProp);
                        var temp2 = cFrameExtendsProp.vector2Value;
                        temp2.x = Mathf.Clamp01(temp2.x);
                        temp2.y = Mathf.Clamp01(temp2.y);
                        cFrameExtendsProp.vector2Value = temp2;

                        EditorGUILayout.PropertyField(cDutchProp);
                        EditorGUILayout.PropertyField(cZoomFrame);
                        EditorGUILayout.PropertyField(cDampProp);
                        GUI.enabled = true;
                        EditorGUILayout.HelpBox("You can also view the composition settings live in the Unity Game Window while a clip using this shot is selected and active (cutscene time within clip range).", MessageType.None);
                    }
                }
                GUILayout.EndVertical();
                GUILayout.Space(2);
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);

            EditorGUI.indentLevel = 0;
        }
    }
}

#endif