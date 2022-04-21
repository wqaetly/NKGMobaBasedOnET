#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Slate
{

    ///Draws a 3D curve editor in scene view (OnSceneGUI)
    public static class CurveEditor3D
    {

        ///Event raised when the CurveEditor3D changes the current curves, with argument being the IAnimatable the curves belong to.
        public static event System.Action<IAnimatableData> onCurvesUpdated;

        private static Dictionary<IAnimatableData, CurveEditor3DRenderer> cache = new Dictionary<IAnimatableData, CurveEditor3DRenderer>();
        public static void Draw3DCurve(IAnimatableData animatable, IKeyable keyable, Transform transformContext, float time, float timeSpan = 50f) {
            CurveEditor3DRenderer instance = null;
            if ( !cache.TryGetValue(animatable, out instance) ) {
                cache[animatable] = instance = new CurveEditor3DRenderer();
            }
            instance.Draw3DCurve(animatable, keyable, transformContext, time, timeSpan);
        }

        ///----------------------------------------------------------------------------------------------

        ///The actual class responsible
        public class CurveEditor3DRenderer
        {

            enum ContextAction
            {
                None,
                SetBrokenMode,
                SetTangentMode,
                Delete
            }

            const float DRAW_RESOLUTION = 0.1f;
            const float DRAW_THRESHOLD = 0.01f;
            const float HANDLE_DISTANCE_COMPENSATION = 2f;

            private IAnimatableData animatable;
            // private IKeyable keyable;

            private int lastCurveLength;
            private int kIndex = -1;
            private ContextAction contextAction;
            private bool contextBrokenMode;
            private TangentMode contextTangentMode;

            ///Display curves that belong to serializeContext and transformContext parent, at time and with timeSpan.
            public void Draw3DCurve(IAnimatableData animatable, IKeyable keyable, Transform transformContext, float time, float timeSpan = 50f) {

                this.animatable = animatable;
                // this.keyable = keyable;

                var curves = animatable.GetCurves();
                if ( curves == null || curves.Length != 3 ) {
                    return;
                }

                var curveX = curves[0];
                var curveY = curves[1];
                var curveZ = curves[2];

                if ( curveX.length < 2 || curveY.length < 2 || curveZ.length < 2 ) {
                    return;
                }

                if ( curveX.length != curveY.length || curveY.length != curveZ.length ) {
                    return;
                }


                var serializeContext = keyable as Object;
                var e = Event.current;

                var start = (float)Mathf.FloorToInt(time - ( timeSpan / 2 ));
                var end = (float)Mathf.CeilToInt(time + ( timeSpan / 2 ));

                start = Mathf.Max(start, Mathf.Min(curveX[0].time, curveY[0].time, curveZ[0].time));
                end = Mathf.Min(end, Mathf.Max(curveX[curveX.length - 1].time, curveY[curveY.length - 1].time, curveZ[curveZ.length - 1].time));

                if ( curveX.length != lastCurveLength ) {
                    lastCurveLength = curveX.length;
                    kIndex = -1;
                }

                //1. Keyframes.
                for ( var k = 0; k < curveX.length; k++ ) {

                    EditorGUI.BeginChangeCheck();
                    var forceChanged = false;

                    var keyX = curveX[k];
                    var keyY = curveY[k];
                    var keyZ = curveZ[k];

                    if ( keyX.time < start ) { continue; }
                    if ( keyX.time > end ) { break; }

                    var tangentModeX = CurveUtility.GetKeyTangentMode(keyX);
                    var tangentModeY = CurveUtility.GetKeyTangentMode(keyY);
                    var tangentModeZ = CurveUtility.GetKeyTangentMode(keyZ);
                    var haveSameTangents = tangentModeX == tangentModeY && tangentModeY == tangentModeZ;
                    var tangentMode = haveSameTangents ? tangentModeX : TangentMode.Editable;
                    var isBroken = CurveUtility.GetKeyBroken(keyX) && CurveUtility.GetKeyBroken(keyY) && CurveUtility.GetKeyBroken(keyZ);

                    var pos = new Vector3(keyX.value, keyY.value, keyZ.value);

                    if ( transformContext != null ) {
                        pos = transformContext.TransformPoint(pos);
                    }

                    Handles.Label(pos, keyX.time.ToString("0.00"));

                    ///MOUSE EVENTS
                    var screenPos = HandleUtility.WorldToGUIPoint(pos);
                    if ( ( (Vector2)screenPos - e.mousePosition ).magnitude < 10 ) {
                        if ( e.type == EventType.MouseDown ) {

                            if ( e.button == 0 && kIndex != k ) {
                                kIndex = k;
                                GUIUtility.hotControl = 0;
                                SceneView.RepaintAll();
                                e.Use();
                            }

                            if ( e.button == 1 && kIndex == k ) {
                                var menu = new GenericMenu();
                                menu.AddItem(new GUIContent("Jump Time Here"), false, () => { keyable.root.currentTime = curveX[kIndex].time + keyable.startTime; });
                                menu.AddItem(new GUIContent("Smooth"), tangentMode == TangentMode.Smooth, () => { contextAction = ContextAction.SetTangentMode; contextTangentMode = TangentMode.Smooth; });
                                menu.AddItem(new GUIContent("Linear"), tangentMode == TangentMode.Linear, () => { contextAction = ContextAction.SetTangentMode; contextTangentMode = TangentMode.Linear; });
                                menu.AddItem(new GUIContent("Constant"), tangentMode == TangentMode.Constant, () => { contextAction = ContextAction.SetTangentMode; contextTangentMode = TangentMode.Constant; });
                                menu.AddItem(new GUIContent("Editable"), tangentMode == TangentMode.Editable, () => { contextAction = ContextAction.SetTangentMode; contextTangentMode = TangentMode.Editable; });
                                if ( tangentMode == TangentMode.Editable ) {
                                    menu.AddItem(new GUIContent("Tangents/Connected"), !isBroken, () => { contextAction = ContextAction.SetBrokenMode; contextBrokenMode = false; });
                                    menu.AddItem(new GUIContent("Tangents/Broken"), isBroken, () => { contextAction = ContextAction.SetBrokenMode; contextBrokenMode = true; });
                                }
                                menu.AddSeparator("/");
                                menu.AddItem(new GUIContent("Delete"), false, () => { contextAction = ContextAction.Delete; });
                                menu.ShowAsContext();
                                e.Use();
                            }
                        }
                    }

                    ///APPLY CONTEXT ACTIONS
                    if ( contextAction != ContextAction.None && k == kIndex ) {
                        var _contextAction = contextAction;
                        contextAction = ContextAction.None;
                        forceChanged = true;
                        if ( _contextAction == ContextAction.SetBrokenMode ) {
                            Undo.RecordObject(serializeContext, "Animation Curve Change");
                            curveX.SetKeyBroken(kIndex, contextBrokenMode);
                            curveY.SetKeyBroken(kIndex, contextBrokenMode);
                            curveZ.SetKeyBroken(kIndex, contextBrokenMode);

                            NotifyChange();
                            return;
                        }

                        if ( _contextAction == ContextAction.SetTangentMode ) {
                            Undo.RecordObject(serializeContext, "Animation Curve Change");
                            curveX.SetKeyTangentMode(kIndex, contextTangentMode);
                            curveY.SetKeyTangentMode(kIndex, contextTangentMode);
                            curveZ.SetKeyTangentMode(kIndex, contextTangentMode);

                            NotifyChange();
                            return;
                        }

                        if ( _contextAction == ContextAction.Delete ) {
                            Undo.RecordObject(serializeContext, "Animation Curve Change");
                            curveX.RemoveKey(k);
                            curveY.RemoveKey(k);
                            curveZ.RemoveKey(k);
                            kIndex = -1;

                            NotifyChange();
                            return;
                        }
                    }


                    ///POSITION
                    var pointSize = HandleUtility.GetHandleSize(pos) * 0.05f;
                    var newValue = pos;
                    if ( kIndex == k ) {
                        if ( Tools.current == Tool.Move ) {
                            newValue = Handles.PositionHandle(pos, Quaternion.identity);
                        } else {
                            newValue = Handles.FreeMoveHandle(pos, Quaternion.identity, pointSize, Vector3.zero, Handles.RectangleHandleCap);
                        }
                    }
                    var cam = SceneView.lastActiveSceneView.camera;
                    Handles.RectangleHandleCap(0, pos, cam.transform.rotation, pointSize, EventType.Repaint);

                    if ( transformContext != null ) {
                        newValue = transformContext.InverseTransformPoint(newValue);
                    }

                    keyX.value = newValue.x;
                    keyY.value = newValue.y;
                    keyZ.value = newValue.z;


                    ///TANGENTS
                    if ( haveSameTangents && tangentMode == TangentMode.Editable ) {

                        if ( kIndex == k ) {

                            if ( k != 0 ) {
                                var inHandle = new Vector3(-keyX.inTangent, -keyY.inTangent, -keyZ.inTangent);
                                inHandle /= HANDLE_DISTANCE_COMPENSATION;
                                inHandle = newValue + inHandle;
                                if ( transformContext != null ) {
                                    inHandle = transformContext.TransformPoint(inHandle);
                                }
                                var handleSize = HandleUtility.GetHandleSize(inHandle) * 0.05f;
                                var newInHandle = Handles.FreeMoveHandle(inHandle, Quaternion.identity, handleSize, Vector3.zero, Handles.CircleHandleCap);
                                Handles.DrawLine(pos, newInHandle);
                                if ( transformContext != null ) {
                                    newInHandle = transformContext.InverseTransformPoint(newInHandle);
                                }

                                newInHandle -= newValue;
                                newInHandle *= HANDLE_DISTANCE_COMPENSATION;
                                keyX.inTangent = -newInHandle.x;
                                keyY.inTangent = -newInHandle.y;
                                keyZ.inTangent = -newInHandle.z;
                                if ( !isBroken ) {
                                    keyX.outTangent = keyX.inTangent;
                                    keyY.outTangent = keyY.inTangent;
                                    keyZ.outTangent = keyZ.inTangent;
                                }
                            }

                            if ( k < curveX.length - 1 ) {
                                var outHandle = new Vector3(keyX.outTangent, keyY.outTangent, keyZ.outTangent);
                                outHandle /= HANDLE_DISTANCE_COMPENSATION;
                                outHandle = newValue + outHandle;
                                if ( transformContext != null ) {
                                    outHandle = transformContext.TransformPoint(outHandle);
                                }
                                var handleSize = HandleUtility.GetHandleSize(outHandle) * 0.05f;
                                var newOutHandle = Handles.FreeMoveHandle(outHandle, Quaternion.identity, handleSize, Vector3.zero, Handles.CircleHandleCap);
                                Handles.DrawLine(pos, newOutHandle);
                                if ( transformContext != null ) {
                                    newOutHandle = transformContext.InverseTransformPoint(newOutHandle);
                                }
                                newOutHandle -= newValue;
                                newOutHandle *= HANDLE_DISTANCE_COMPENSATION;
                                keyX.outTangent = newOutHandle.x;
                                keyY.outTangent = newOutHandle.y;
                                keyZ.outTangent = newOutHandle.z;
                                if ( !isBroken ) {
                                    keyX.inTangent = keyX.outTangent;
                                    keyY.inTangent = keyY.outTangent;
                                    keyZ.inTangent = keyZ.outTangent;
                                }
                            }
                        }

                    }

                    ///APPLY
                    if ( EditorGUI.EndChangeCheck() || forceChanged ) {
                        Undo.RecordObject(serializeContext, "Animation Curve Change");
                        curveX.MoveKey(k, keyX);
                        curveY.MoveKey(k, keyY);
                        curveZ.MoveKey(k, keyZ);
                        EditorUtility.SetDirty(serializeContext);
                        NotifyChange();
                    }

                }


                ///2. Motion Path
                Handles.color = Prefs.motionPathsColor;
                var lastDrawnPos = Vector3.zero;
                for ( var t = start; t <= end; t += DRAW_RESOLUTION ) {
                    var pos = new Vector3(curveX.Evaluate(t), curveY.Evaluate(t), curveZ.Evaluate(t));
                    var nextPos = new Vector3(curveX.Evaluate(t + DRAW_RESOLUTION), curveY.Evaluate(t + DRAW_RESOLUTION), curveZ.Evaluate(t + DRAW_RESOLUTION));

                    if ( transformContext != null ) {
                        pos = transformContext.TransformPoint(pos);
                        nextPos = transformContext.TransformPoint(nextPos);
                    }

                    if ( ( pos - lastDrawnPos ).magnitude > DRAW_THRESHOLD ) {
                        lastDrawnPos = pos;
                        Handles.SphereHandleCap(0, pos, Quaternion.identity, 0.02f, EventType.Repaint);
                        Handles.DrawLine(pos, nextPos);
                    }
                }
                Handles.color = Color.white;


                ///3. GUI
                if ( kIndex >= 0 ) {
                    var guiRect = new Rect(Screen.width - 300, Screen.height - 190, 280, 130);
                    var kx = curveX[kIndex];
                    var ky = curveY[kIndex];
                    var kz = curveZ[kIndex];
                    EditorGUI.BeginChangeCheck();
                    {
                        Handles.BeginGUI();
                        GUILayout.BeginArea(guiRect);
                        EditorTools.BeginBody("Keyframe Parameters");
                        kx.value = EditorGUILayout.FloatField("X", kx.value);
                        ky.value = EditorGUILayout.FloatField("Y", ky.value);
                        kz.value = EditorGUILayout.FloatField("Z", kz.value);

                        GUI.enabled = CurveUtility.GetKeyTangentMode(kx) == TangentMode.Editable;
                        var inTangent = new Vector3(kx.inTangent, ky.inTangent, kz.inTangent);
                        inTangent = EditorGUILayout.Vector3Field("", inTangent);
                        kx.inTangent = inTangent.x;
                        ky.inTangent = inTangent.y;
                        kz.inTangent = inTangent.z;

                        GUI.enabled = CurveUtility.GetKeyBroken(kx);
                        var outTangent = new Vector3(kx.outTangent, ky.outTangent, kz.outTangent);
                        outTangent = EditorGUILayout.Vector3Field("", outTangent);
                        kx.outTangent = outTangent.x;
                        ky.outTangent = outTangent.y;
                        kz.outTangent = outTangent.z;

                        GUI.enabled = true;

                        EditorTools.EndBody();
                        GUILayout.EndArea();
                        Handles.EndGUI();
                    }
                    if ( EditorGUI.EndChangeCheck() ) {
                        Undo.RecordObject(serializeContext, "Animation Curve Change");
                        curveX.MoveKey(kIndex, kx);
                        curveY.MoveKey(kIndex, ky);
                        curveZ.MoveKey(kIndex, kz);
                        EditorUtility.SetDirty(serializeContext);
                        NotifyChange();
                    }
                }


                /*
                                for (var k = 0; k < curveX.length - 1; k++){
                                    var keyX = curveX[k];
                                    var keyY = curveY[k];
                                    var keyZ = curveZ[k];
                                    var nextKeyX = curveX[k+1];
                                    var nextKeyY = curveY[k+1];
                                    var nextKeyZ = curveZ[k+1];

                                    var t = new Vector3(keyX.time, keyY.time, keyZ.time);
                                    var nextT = new Vector3(nextKeyX.time, nextKeyY.time, nextKeyZ.time);

                                    var tangent = new Vector3( keyX.outTangent, keyY.outTangent, keyZ.outTangent );
                                    var nextTangent = new Vector3( nextKeyX.inTangent, nextKeyY.inTangent, nextKeyZ.inTangent );

                                    var pos = new Vector3( keyX.value, keyY.value, keyZ.value );
                                    var nextPos = new Vector3( nextKeyX.value, nextKeyY.value, nextKeyZ.value );

                                    if (transformContext != null){
                                        pos = transformContext.TransformPoint(pos);
                                        nextPos = transformContext.TransformPoint(nextPos);
                                    }

                                    var num = (nextT - t) * 0.333333f;
                                    var tangentPos = new Vector3( pos.x + num.x * tangent.x, pos.y + num.y * tangent.y, pos.z + num.z * tangent.z );
                                    var nextTangentPos = new Vector3( nextPos.x - num.x * nextTangent.x, nextPos.y - num.y * nextTangent.y, nextPos.z - num.z * nextTangent.z );

                                    Handles.DrawBezier(pos, nextPos, tangentPos, nextTangentPos, Prefs.motionPathsColor, null, 1.5f);
                                }
                */

            }

            void NotifyChange() {
                if ( onCurvesUpdated != null ) {
                    onCurvesUpdated(animatable);
                }
            }
        }
    }
}

#endif