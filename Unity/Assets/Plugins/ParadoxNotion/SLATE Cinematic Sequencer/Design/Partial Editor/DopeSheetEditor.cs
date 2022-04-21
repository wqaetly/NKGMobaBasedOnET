#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Slate
{

    ///A DopeSheet editor for animation curves
    public static class DopeSheetEditor
    {

        ///Event raised when the DopeSheet changes the current curves, with argument being the IAnimatable the curves belong to.
        public static event System.Action<IAnimatableData> onCurvesUpdated;

        private static Dictionary<IAnimatableData, DopeSheetRenderer> cache = new Dictionary<IAnimatableData, DopeSheetRenderer>();
        public static void DrawDopeSheet(IAnimatableData animatable, IKeyable keyable, Rect rect, float startTime, float length, bool highlightRange = true) {
            DopeSheetRenderer dopeSheet = null;
            if ( !cache.TryGetValue(animatable, out dopeSheet) ) {
                cache[animatable] = dopeSheet = new DopeSheetRenderer(animatable, keyable);
            }
            dopeSheet.DrawDopeSheet(animatable, keyable, rect, startTime, length, highlightRange);
        }

        static DopeSheetEditor() {
            AnimatedParameter.onParameterChanged += RefreshDopeKeysOf;
            CurveEditor.onCurvesUpdated += RefreshDopeKeysOf;
            CurveEditor3D.onCurvesUpdated += RefreshDopeKeysOf;
            CutsceneUtility.onRefreshAllAnimationEditors += RefreshDopeKeysOf;
        }

        //Refresh dopesheet keys of target animatable.
        static void RefreshDopeKeysOf(IAnimatableData animatable) {

            DopeSheetRenderer dopeSheet = null;
            if ( cache.TryGetValue(animatable, out dopeSheet) ) {
                dopeSheet.RefreshDopeKeys(animatable);
            }

            if ( animatable is AnimationDataCollection ) {
                var data = (AnimationDataCollection)animatable;
                if ( data.animatedParameters != null ) {
                    foreach ( var animParam in data.animatedParameters ) {
                        if ( cache.TryGetValue(animParam, out dopeSheet) ) {
                            dopeSheet.RefreshDopeKeys(animParam);
                        }
                    }
                }
            }

            if ( animatable is AnimatedParameter ) {
                foreach ( var pair in cache ) {
                    if ( pair.Key is AnimationDataCollection ) {
                        var data = (AnimationDataCollection)pair.Key;
                        if ( data.animatedParameters != null && data.animatedParameters.Contains((AnimatedParameter)animatable) ) {
                            pair.Value.RefreshDopeKeys(pair.Key);
                            break;
                        }
                    }
                }
            }
        }

        ///----------------------------------------------------------------------------------------------

        ///The actual class responsible
        public class DopeSheetRenderer
        {

            const float KEY_PROXIMITY_TOLERANCE = 0.001f;

            //flags that dopekeys need refresh.
            private bool refreshDopeKeys;

            private IAnimatableData animatable;
            private IKeyable keyable;

            private AnimationCurve[] allCurves;
            private List<Keyframe> allKeys;
            private float length;
            private Rect rect;
            private float width;
            private float startTime;

            private int pickIndex = -1;
            private List<float> currentTimes;
            private List<float> prePickTimes;
            private List<TangentMode> tangentModes; //tangent modes of dopekeys in order of appearence in currentTimes
            private List<string> keyLabels; //key labels of dopekeys in order of appearence in currentTimes
            private WrapMode preWrapMode;
            private WrapMode postWrapMode;

            private static Keyframe[] copyKeyframes;
            private static List<Keyframe[]> multiCopyKeyframes;

            private float? selectionStartPos; //used only for creating the rect
            private float? startDragTime; //the time which we started dragging the selection rect
            private Rect? timeSelectionRect; //the actual selection rect
            private Rect preScaleSelectionRect; //the rect before start retiming selection rect
            private bool isRetiming; //is retiming/scaling keys?
            private List<int> rectSelectedIndeces; //the indeces of dopekeys that were originaly in the selection rect
            private Rect pixelSelectionRect { //the converted selection rect from time to pos/pixels
                get
                {
                    if ( timeSelectionRect == null ) return new Rect();
                    var temp = timeSelectionRect.Value;
                    return Rect.MinMaxRect(TimeToPos(temp.xMin), rect.yMin, TimeToPos(temp.xMax), rect.yMax);
                }
            }

            private float timeSelectionRange {
                get { return timeSelectionRect != null ? timeSelectionRect.Value.xMax - timeSelectionRect.Value.xMin : 0f; }
            }

            float TimeToPos(float time) {
                return ( ( time - startTime ) / length ) * width + rect.x;
            }

            float PosToTime(float pos) {
                return ( ( pos - rect.x ) / width ) * length + startTime;
            }

            public DopeSheetRenderer(IAnimatableData animatable, IKeyable keyable) {
                this.animatable = animatable;
                this.allCurves = animatable.GetCurves();
                this.keyable = keyable;
                RefreshDopeKeys(animatable);
                Undo.undoRedoPerformed += () => { ResetInteraction(); refreshDopeKeys = true; };

#if UNITY_2018_3_OR_NEWER
                UnityEditor.Experimental.SceneManagement.PrefabStage.prefabStageClosing += (stage) => { refreshDopeKeys = true; };
#endif
            }

            public void DrawDopeSheet(IAnimatableData animatable, IKeyable keyable, Rect rect, float startTime, float length, bool highlightRange) {

                this.length = length;
                this.rect = rect;
                this.width = rect.width;
                this.startTime = startTime;
                this.animatable = animatable;
                this.allCurves = animatable.GetCurves();
                this.keyable = keyable;
                var e = Event.current;

                //no curves?
                if ( allCurves == null || allCurves.Length == 0 ) {
                    GUI.Label(new Rect(rect.x, rect.y, rect.width, rect.height), "---");
                    return;
                }

                //if flag is true refresh all dopesheets of the same IKeyable
                if ( refreshDopeKeys ) {
                    refreshDopeKeys = false;
                    DopeSheetEditor.RefreshDopeKeysOf(animatable);
                }

                //range graphics
                if ( highlightRange && currentTimes.Count > 0 ) {
                    var firstKeyPos = TimeToPos(currentTimes.FirstOrDefault());
                    var lastKeyPos = TimeToPos(currentTimes.LastOrDefault());

                    if ( Mathf.Abs(firstKeyPos - lastKeyPos) > 0 ) {
                        var rangeRect = Rect.MinMaxRect(firstKeyPos - 8, rect.yMin, lastKeyPos + 8, rect.yMax);
                        rangeRect.xMin = Mathf.Max(rangeRect.xMin, rect.xMin - 2);
                        rangeRect.xMax = Mathf.Min(rangeRect.xMax, rect.xMax + 2);
                        if ( rangeRect.width > 5 ) {
                            GUI.color = EditorGUIUtility.isProSkin ? new Color(0f, 0.5f, 0.5f, 0.4f) : new Color(1, 1, 1, 0.5f);
                            GUI.Box(rangeRect, string.Empty, Slate.Styles.clipBoxStyle);
                            GUI.color = Color.white;
                        }

                        if ( preWrapMode != WrapMode.ClampForever ) {
                            var r = Rect.MinMaxRect(rect.xMin, rect.yMin, firstKeyPos, rect.yMax);
                            if ( r.width > 16 ) {
                                GUI.color = new Color(1, 1, 1, 0.5f);
                                var r2 = new Rect(0, 0, 16, 16);
                                r2.center = r.center;
                                Texture2D icon = null;
                                if ( preWrapMode == WrapMode.Loop ) { icon = Styles.loopIcon; }
                                if ( preWrapMode == WrapMode.PingPong ) { icon = Styles.pingPongIcon; }
                                if ( icon != null ) { GUI.Box(r2, icon, GUIStyle.none); }
                                GUI.color = Color.white;
                            }
                        }

                        if ( postWrapMode != WrapMode.ClampForever ) {
                            var r = Rect.MinMaxRect(lastKeyPos, rect.yMin, rect.xMax, rect.yMax);
                            if ( r.width > 16 ) {
                                GUI.color = new Color(1, 1, 1, 0.5f);
                                var r2 = new Rect(0, 0, 16, 16);
                                r2.center = r.center;
                                Texture2D icon = null;
                                if ( postWrapMode == WrapMode.Loop ) { icon = Styles.loopIcon; }
                                if ( postWrapMode == WrapMode.PingPong ) { icon = Styles.pingPongIcon; }
                                if ( icon != null ) { GUI.Box(r2, icon, GUIStyle.none); }
                                GUI.color = Color.white;
                            }
                        }
                    }
                }

                //bg graphics (just a horizontal line)
                Handles.color = Color.black.WithAlpha(0.2f);
                var center = rect.y + ( rect.height / 2 );
                Handles.DrawLine(new Vector2(rect.xMin, center), new Vector2(rect.xMax, center));
                Handles.color = Color.white;

                //selection rect graphics
                if ( timeSelectionRect != null ) {
                    GUI.color = new Color(0.5f, 0.5f, 1, 0.5f);
                    GUI.DrawTexture(pixelSelectionRect, Slate.Styles.whiteTexture);
                    GUI.color = Color.white;
                }


                //draw the dopekeys
                var tangentMode = TangentMode.Editable;
                for ( var t = 0; t < currentTimes.Count; t++ ) {

                    var time = currentTimes[t];
                    //ignore if out of view range (+- some extra offset)
                    if ( time < startTime - 0.1f || time > startTime + length + 0.1f ) {
                        continue;
                    }

                    //DopeKey graphics/icon
                    var icon = Slate.Styles.dopeKey;
                    if ( Prefs.keyframesStyle == Prefs.KeyframesStyle.PerTangentMode ) {
                        tangentMode = tangentModes[t];
                        if ( tangentMode != TangentMode.Editable ) {
                            if ( tangentMode == TangentMode.Smooth ) {
                                icon = Slate.Styles.dopeKeySmooth;
                            }
                            if ( tangentMode == TangentMode.Constant ) {
                                icon = Slate.Styles.dopeKeyConstant;
                            }
                            if ( tangentMode == TangentMode.Linear ) {
                                icon = Slate.Styles.dopeKeyLinear;
                            }
                        }
                    }

                    var dopeKeyRect = new Rect(0, 0, icon.width, icon.height);
                    dopeKeyRect.center = new Vector2(TimeToPos(time), rect.center.y);
                    var isSelected = t == pickIndex || ( rectSelectedIndeces != null && rectSelectedIndeces.Contains(t) );
                    GUI.color = isSelected ? new Color(0.6f, 0.6f, 1) : Color.white;
                    GUI.DrawTexture(dopeKeyRect, icon);
                    GUI.color = Color.white;


                    //key value label
                    if ( Prefs.showDopesheetKeyValues ) {
                        var nextPos = t < currentTimes.Count - 1 ? TimeToPos(currentTimes[t + 1]) : TimeToPos(length);
                        var valueLabelRect = Rect.MinMaxRect(dopeKeyRect.xMax, rect.yMin - 3, nextPos - dopeKeyRect.width / 2, rect.yMax);
                        if ( valueLabelRect.width > 20 ) {
                            GUI.Label(valueLabelRect, keyLabels[t], Slate.Styles.leftLabel);
                        }
                    }


                    //do the following only if we dont have a rect selection
                    if ( timeSelectionRect == null ) {

                        //pick the key
                        if ( e.type == EventType.MouseDown && dopeKeyRect.Contains(e.mousePosition) ) {
                            prePickTimes = new List<float>(currentTimes);
                            pickIndex = t;
                            if ( e.clickCount == 2 ) {
                                keyable.root.currentTime = time + keyable.startTime;
                                CutsceneUtility.selectedObject = keyable;
                            }
                            e.Use();
                        }

                        //single key context menu
                        if ( e.type == EventType.MouseUp && e.button == 1 && dopeKeyRect.Contains(e.mousePosition) ) {
                            DoSingleKeyContextMenu(e, time, tangentMode);
                            e.Use();
                        }
                    }
                }


                //drag the picked key if any. Shift drags all next to it as well
                if ( pickIndex != -1 ) {
                    var controlID = GUIUtility.GetControlID(FocusType.Passive);
                    var eventType = e.GetTypeForControl(controlID);
                    if ( eventType == EventType.MouseDrag && e.button == 0 ) {
                        GUIUtility.hotControl = controlID;
                        var lastTime = currentTimes[pickIndex];
                        var newTime = PosToTime(e.mousePosition.x);
                        newTime = Mathf.Round(newTime / Prefs.snapInterval) * Prefs.snapInterval;
                        newTime = Mathf.Clamp(newTime, startTime, startTime + length);
                        if ( e.shift ) {
                            var max = pickIndex > 0 ? currentTimes[pickIndex - 1] + Prefs.snapInterval : startTime;
                            newTime = Mathf.Max(newTime, max);
                            foreach ( var time in currentTimes.Where(k => k > lastTime) ) {
                                var index = currentTimes.IndexOf(time);
                                currentTimes[index] += newTime - lastTime;
                            }
                        }
                        currentTimes[pickIndex] = newTime;
                    }

                    //apply the changes when mouse up and deselect key
                    if ( eventType == EventType.MouseUp ) {
                        GUIUtility.hotControl = 0;
                        pickIndex = -1;
                        Apply();
                        e.Use();
                    }
                }



                //Multikey selection, dragging and retiming
                if ( pickIndex == -1 ) {

                    var retimeInRect = Rect.MinMaxRect(pixelSelectionRect.xMin, pixelSelectionRect.yMin, pixelSelectionRect.xMin + 4, pixelSelectionRect.yMax);
                    var retimeOutRect = Rect.MinMaxRect(pixelSelectionRect.xMax - 4, pixelSelectionRect.yMin, pixelSelectionRect.xMax, pixelSelectionRect.yMax);

                    var controlID = GUIUtility.GetControlID(FocusType.Passive);
                    var eventType = e.GetTypeForControl(controlID);

                    if ( e.rawType == EventType.MouseDown && !rect.Contains(e.mousePosition) ) {
                        ResetInteraction();
                    }

                    if ( e.type == EventType.MouseDown && rect.Contains(e.mousePosition) ) {

                        //if no rect selection, start one.
                        if ( timeSelectionRect == null ) {
                            if ( e.button == 0 ) {
                                selectionStartPos = e.mousePosition.x;
                                e.Use();
                            }

                        } else {

                            //if we have a rect and mouse contains it, initialize original values and keys.
                            if ( pixelSelectionRect.Contains(e.mousePosition) ) {

                                prePickTimes = new List<float>(currentTimes);
                                startDragTime = (float)PosToTime(e.mousePosition.x);
                                preScaleSelectionRect = timeSelectionRect.Value;
                                rectSelectedIndeces = new List<int>();
                                var temp = timeSelectionRect.Value;
                                for ( var i = 0; i < currentTimes.Count; i++ ) {
                                    if ( currentTimes[i] >= temp.xMin && currentTimes[i] <= temp.xMax ) {
                                        rectSelectedIndeces.Add(i);
                                    }
                                }

                                isRetiming = e.button == 0 && retimeInRect.Contains(e.mousePosition) || retimeOutRect.Contains(e.mousePosition);
                                e.Use();

                                //if we have a rect, but mouse is outside, clear all and reset values.
                            } else {
                                ResetInteraction();
                                e.Use();
                            }
                        }
                    }


                    //create the selection rect
                    if ( eventType == EventType.MouseDrag && selectionStartPos != null ) {
                        GUIUtility.hotControl = controlID;
                        var a = PosToTime(selectionStartPos.Value);
                        var b = PosToTime(e.mousePosition.x);
                        var xMin = Mathf.Min(a, b);
                        var xMax = Mathf.Max(a, b);
                        xMin = Mathf.Max(xMin, startTime);
                        xMax = Mathf.Min(xMax, startTime + length);
                        timeSelectionRect = Mathf.Abs(a - b) >= 0.001f ? Rect.MinMaxRect(xMin, rect.yMin, xMax, rect.yMax) : (Rect?)null;
                    }

                    //draw the selection rect
                    if ( timeSelectionRect != null ) {
                        EditorGUIUtility.AddCursorRect(retimeInRect, MouseCursor.ResizeHorizontal);
                        EditorGUIUtility.AddCursorRect(retimeOutRect, MouseCursor.ResizeHorizontal);
                        EditorGUIUtility.AddCursorRect(pixelSelectionRect, MouseCursor.Link);
                        GUI.color = ColorUtility.Grey(0.8f);
                        GUI.DrawTexture(retimeInRect, Texture2D.whiteTexture);
                        GUI.DrawTexture(retimeOutRect, Texture2D.whiteTexture);
                        GUI.color = Color.white;
                    }

                    //move/retime the selection rect
                    if ( eventType == EventType.MouseDrag && timeSelectionRect != null && e.button == 0 && ( startDragTime != null || isRetiming ) ) {
                        GUIUtility.hotControl = controlID;
                        var temp = timeSelectionRect.Value;
                        var pointerTime = PosToTime(e.mousePosition.x);

                        //retime
                        if ( isRetiming ) {

                            var retimeIn = Mathf.Abs(pointerTime - temp.x) < Mathf.Abs(pointerTime - temp.xMax);
                            if ( retimeIn ) { temp.xMin = Mathf.Max(pointerTime, 0); } else { temp.xMax = pointerTime; }

                            foreach ( var index in rectSelectedIndeces ) {
                                var preTime = prePickTimes[index];
                                var norm = Mathf.InverseLerp(preScaleSelectionRect.xMin, preScaleSelectionRect.xMax, preTime);
                                currentTimes[index] = Mathf.Lerp(temp.xMin, temp.xMax, norm);
                            }

                            //move
                        } else {

                            if ( startDragTime != null ) {
                                var delta = pointerTime - (float)startDragTime;
                                if ( temp.x + delta >= 0 ) {
                                    foreach ( var index in rectSelectedIndeces ) {
                                        currentTimes[index] += delta;
                                    }
                                    temp.x += delta;
                                    startDragTime = (float)pointerTime;
                                }
                            }
                        }

                        timeSelectionRect = temp;
                    }

                    //Apply all changes and reset values on MouseUp within or outside the rect
                    if ( eventType == EventType.MouseUp ) {
                        //mouse up when making a selection
                        if ( selectionStartPos != null ) {
                            GUIUtility.hotControl = 0;
                            selectionStartPos = null;
                        }

                        //mouse up when dragging or retiming the existing selection
                        if ( timeSelectionRect != null && ( startDragTime != null || isRetiming ) ) {
                            GUIUtility.hotControl = 0;
                            Apply();
                            isRetiming = false;
                            startDragTime = null;
                            if ( e.button == 0 ) {
                                rectSelectedIndeces = null;
                            }
                        }
                    }

                    //Context click
                    if ( eventType == EventType.ContextClick && rect.Contains(e.mousePosition) ) {
                        if ( pixelSelectionRect.Contains(e.mousePosition) ) {
                            DoMultiKeyContextMenu(e);
                            e.Use();

                        } else {
                            DoVoidContextMenu(e);
                            e.Use();
                        }
                    }
                }
            }

            void ResetInteraction() {
                isRetiming = false;
                startDragTime = null;
                timeSelectionRect = null;
                selectionStartPos = null;
                rectSelectedIndeces = null;
                pickIndex = -1;
            }

            ///Context menu for single key
            void DoSingleKeyContextMenu(Event e, float time, TangentMode tangentMode) {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Jump Here (Double Click)"), false, () => { keyable.root.currentTime = time + keyable.startTime; });
                menu.AddItem(new GUIContent("Tangent Mode/Smooth"), tangentMode == TangentMode.Smooth, () => { SetKeyTangentMode(time, TangentMode.Smooth); });
                menu.AddItem(new GUIContent("Tangent Mode/Linear"), tangentMode == TangentMode.Linear, () => { SetKeyTangentMode(time, TangentMode.Linear); });
                menu.AddItem(new GUIContent("Tangent Mode/Constant"), tangentMode == TangentMode.Constant, () => { SetKeyTangentMode(time, TangentMode.Constant); });
                menu.AddItem(new GUIContent("Tangent Mode/Editable"), tangentMode == TangentMode.Editable, () => { SetKeyTangentMode(time, TangentMode.Editable); });
                menu.AddItem(new GUIContent("Copy Key"), false, () => { CopyKeyAtTime(time); });
                menu.AddItem(new GUIContent("Cut Key"), false, () => { CopyKeyAtTime(time); DeleteKeyAtTime(time); });
                menu.AddSeparator("/");
                menu.AddItem(new GUIContent("Delete Key"), false, () => { DeleteKeyAtTime(time); });
                menu.ShowAsContext();
                pickIndex = -1;
                e.Use();
            }


            ///Context menu for multiple keys
            void DoMultiKeyContextMenu(Event e) {
                var menu = new GenericMenu();
                var rangeText = timeSelectionRange.ToString("0.00") + " sec.";
                if ( rectSelectedIndeces != null && rectSelectedIndeces.Count > 0 ) {
                    menu.AddItem(new GUIContent("Set Keys Tangent Mode/Smooth"), false, () => { SetSelectionTangentMode(TangentMode.Smooth); });
                    menu.AddItem(new GUIContent("Set Keys Tangent Mode/Linear"), false, () => { SetSelectionTangentMode(TangentMode.Linear); });
                    menu.AddItem(new GUIContent("Set Keys Tangent Mode/Constant"), false, () => { SetSelectionTangentMode(TangentMode.Constant); });
                    menu.AddItem(new GUIContent("Set Keys Tangent Mode/Editable"), false, () => { SetSelectionTangentMode(TangentMode.Editable); });
                    menu.AddItem(new GUIContent("Copy Keys"), false, () => { CopyKeysSelected(); });
                    menu.AddItem(new GUIContent("Cut Keys"), false, () => { CopyKeysSelected(); DeleteKeysSelected(); });
                    menu.AddItem(new GUIContent(string.Format("Cut Keys (Ripple {0})", rangeText)), false, () =>
                       {
                           var temp = timeSelectionRect.Value;
                           CopyKeysSelected();
                           DeleteKeysSelected();
                           RemoveTime(temp.xMin, temp.xMax);
                       });
                    menu.AddSeparator("/");
                    menu.AddItem(new GUIContent(string.Format("Delete Keys (Ripple {0})", rangeText)), false, () =>
                       {
                           var temp = timeSelectionRect.Value;
                           DeleteKeysSelected();
                           RemoveTime(temp.xMin, temp.xMax);
                       });

                    menu.AddItem(new GUIContent("Delete Keys"), false, () => { DeleteKeysSelected(); });

                } else {
                    menu.AddItem(new GUIContent(string.Format("Remove Time ({0})", rangeText)), false, () =>
                       {
                           RemoveTime(timeSelectionRect.Value.xMin, timeSelectionRect.Value.xMax);
                           timeSelectionRect = null;
                       });
                }
                menu.ShowAsContext();
                e.Use();
            }


            //Context menu for empty space
            void DoVoidContextMenu(Event e) {
                var cursorTime = PosToTime(e.mousePosition.x);
                var isCollection = animatable is AnimationDataCollection;
                var menu = new GenericMenu();
                var label = "Create New Key" + ( isCollection ? " (All Parameters)" : "" );
                menu.AddItem(new GUIContent(label), false, () => { CreateNewKey(cursorTime); });
                if ( copyKeyframes != null && copyKeyframes.Length == allCurves.Length ) {
                    menu.AddItem(new GUIContent("Paste Key At Cursor"), false, () => { PasteKeyAtTime(cursorTime); });
                    menu.AddItem(new GUIContent("Paste Key At Scrubber"), false, () => { PasteKeyAtTime(keyable.root.currentTime - keyable.startTime); });
                } else {
                    if ( multiCopyKeyframes != null && multiCopyKeyframes.Count != 0 && multiCopyKeyframes[0].Length == allCurves.Length ) {
                        var from = Mathf.Max(multiCopyKeyframes.FirstOrDefault().Select(k => k.time).ToArray());
                        var to = Mathf.Max(multiCopyKeyframes.LastOrDefault().Select(k => k.time).ToArray());
                        var range = to - from;
                        menu.AddItem(new GUIContent("Paste Keys At Cursor"), false, () => { PasteKeysSelected(cursorTime, range, false); });
                        menu.AddItem(new GUIContent("Paste Keys At Scrubber"), false, () => { PasteKeysSelected(keyable.root.currentTime - keyable.startTime, range, false); });
                        //TODO: BUGGY NEEDS FIX
                        // if (range > 0){
                        // 	menu.AddItem(new GUIContent(string.Format("Paste Keys (Ripple {0} sec.)", range.ToString("0.00")) ), false, ()=>
                        // 		{
                        // 			PasteKeysSelected(cursorTime, range, true);
                        // 		});
                        // }
                    }
                }

                menu.ShowAsContext();
                e.Use();
            }

            //...
            void RecordUndo() {
                UnityEngine.Object obj = keyable as UnityEngine.Object;
                if ( obj != null ) { Undo.RecordObject(obj, "DopeSheet Change"); }
            }

            //...
            void CreateNewKey(float time) {
                RecordUndo();
                animatable.TryKeyIdentity(time);
                refreshDopeKeys = true;
                NotifyChange();
            }

            ///Copy the keyframe at time
            Keyframe[] CopyKeyAtTime(float time) {
                copyKeyframes = new Keyframe[allCurves.Length];
                for ( var i = 0; i < allCurves.Length; i++ ) {
                    var curve = allCurves[i];
                    for ( var j = 0; j < curve.keys.Length; j++ ) {
                        var key = curve.keys[j];
                        if ( Mathf.Abs(key.time - time) <= KEY_PROXIMITY_TOLERANCE ) {
                            copyKeyframes[i] = key;
                        }
                    }
                }
                return copyKeyframes;
            }

            ///Copy selected keys
            void CopyKeysSelected() {
                multiCopyKeyframes = new List<Keyframe[]>();
                foreach ( var index in rectSelectedIndeces ) {
                    var time = currentTimes[index];
                    multiCopyKeyframes.Add(CopyKeyAtTime(time));
                }
                copyKeyframes = null;
            }

            ///Delete the keyframe at time
            void DeleteKeyAtTime(float time) {
                RecordUndo();
                foreach ( var curve in allCurves ) {
                    for ( var i = 0; i < curve.keys.Length; i++ ) {
                        var key = curve.keys[i];
                        if ( Mathf.Abs(key.time - time) <= KEY_PROXIMITY_TOLERANCE ) {
                            curve.RemoveKey(i);
                        }
                    }
                    curve.UpdateTangentsFromMode();
                }
                refreshDopeKeys = true;
                NotifyChange();
            }


            ///Delete selected keyframe indeces
            void DeleteKeysSelected() {
                RecordUndo();
                foreach ( var index in rectSelectedIndeces ) {
                    var time = currentTimes[index];
                    DeleteKeyAtTime(time);
                }

                ResetInteraction();
                refreshDopeKeys = true;
                NotifyChange();
            }

            ///Paste the previously copied keyframe
            void PasteKeyAtTime(float pasteTime) {
                pasteTime = Mathf.Max(0, pasteTime);
                RecordUndo();
                for ( var i = 0; i < allCurves.Length; i++ ) {
                    var curve = allCurves[i];
                    var key = copyKeyframes[i];
                    //avoid pasting empty keys
                    if ( key.time != 0 || key.value != 0 ) {
                        key.time = pasteTime;
                        var index = curve.AddKey(key);
                        curve.MoveKey(index, key);
                        curve.UpdateTangentsFromMode();
                    }
                }
                refreshDopeKeys = true;
                NotifyChange();
            }

            ///Paste previously copied multiple keyframes
            void PasteKeysSelected(float pasteTime, float range, bool ripple) {
                pasteTime = Mathf.Max(0, pasteTime);
                if ( ripple && range > 0 ) { InsertSpaceAtTime(pasteTime, range); }
                RecordUndo();
                //done this way avoid curves without keys
                var firstKeyTime = Mathf.Max(multiCopyKeyframes[0].Select(k => k.time).ToArray());
                var diff = firstKeyTime - pasteTime;
                foreach ( Keyframe[] copiedKeys in multiCopyKeyframes ) {
                    copyKeyframes = copiedKeys;
                    var dopeKeyTime = Mathf.Max(copyKeyframes.Select(k => k.time).ToArray());
                    PasteKeyAtTime(dopeKeyTime - diff);
                }
                copyKeyframes = null;

                refreshDopeKeys = true;
                NotifyChange();

                if ( range > 0 ) {
                    timeSelectionRect = Rect.MinMaxRect(pasteTime, 0, pasteTime + range, 0);
                }
            }

            ///Set key tangent mode at time
            void SetKeyTangentMode(float time, TangentMode mode) {
                RecordUndo();
                foreach ( var curve in allCurves ) {
                    for ( var i = 0; i < curve.keys.Length; i++ ) {
                        var key = curve.keys[i];
                        if ( Mathf.Abs(key.time - time) <= KEY_PROXIMITY_TOLERANCE ) {
                            curve.SetKeyTangentMode(i, mode);
                        }
                    }
                }

                refreshDopeKeys = true;
                NotifyChange();
            }

            ///Set all selected keys tangent mode
            void SetSelectionTangentMode(TangentMode mode) {
                RecordUndo();
                foreach ( var index in rectSelectedIndeces ) {
                    var time = currentTimes[index];
                    SetKeyTangentMode(time, mode);
                }

                refreshDopeKeys = true;
                NotifyChange();
            }


            ///Insert space by moving all keys after forward
            void InsertSpaceAtTime(float timeToInsert, float spaceToInsert) {
                RecordUndo();
                foreach ( var curve in allCurves ) {
                    for ( var i = 0; i < curve.keys.Length; i++ ) {
                        var key = curve.keys[i];
                        if ( key.time > timeToInsert ) {
                            key.time += spaceToInsert;
                            curve.MoveKey(i, key);
                        }
                    }
                    curve.UpdateTangentsFromMode();
                }

                refreshDopeKeys = true;
                NotifyChange();
            }

            ///Remove space by moving all keys after backwards
            void RemoveTime(float timeFrom, float timeTo) {
                RecordUndo();
                foreach ( var curve in allCurves ) {
                    for ( var i = 0; i < curve.keys.Length; i++ ) {
                        var key = curve.keys[i];
                        if ( key.time > timeTo ) {
                            key.time -= timeTo - timeFrom;
                            curve.MoveKey(i, key);
                        }
                    }
                    curve.UpdateTangentsFromMode();
                }

                refreshDopeKeys = true;
                NotifyChange();
            }

            //apply the changed key times
            void Apply() {
                RecordUndo();
                var curveChangeFlags = new bool[allCurves.Length];
                for ( var i = 0; i < prePickTimes.Count; i++ ) {
                    var lastTime = prePickTimes[i];
                    var newTime = currentTimes[i];
                    if ( Mathf.Abs(lastTime - newTime) >= float.Epsilon ) {
                        for ( var c = 0; c < allCurves.Length; c++ ) {
                            var curve = allCurves[c];
                            for ( var k = 0; k < curve.length; k++ ) {
                                var key = curve[k];
                                if ( Mathf.Abs(key.time - lastTime) <= KEY_PROXIMITY_TOLERANCE ) {
                                    key.time = newTime;
                                    curve.MoveKey(k, key);
                                    curveChangeFlags[c] = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                for ( var c = 0; c < allCurves.Length; c++ ) {
                    if ( curveChangeFlags[c] == true ) {
                        allCurves[c].UpdateTangentsFromMode();
                    }
                }

                refreshDopeKeys = true;
                NotifyChange();
            }


            //raise changed event
            void NotifyChange() {
                if ( onCurvesUpdated != null ) {
                    onCurvesUpdated(animatable);
                }
            }



            ///Should be called after each change to the curves
            public void RefreshDopeKeys(IAnimatableData animatable) {

                //get all curve keys
                allCurves = animatable.GetCurves();
                allKeys = new List<Keyframe>();
                for ( var i = 0; i < allCurves.Length; i++ ) {
                    allKeys.AddRange(allCurves[i].keys);
                    //cache wrapmode to parameters
                    if ( animatable is AnimatedParameter && i == 0 ) {
                        preWrapMode = allCurves[i].preWrapMode;
                        postWrapMode = allCurves[i].postWrapMode;
                    }
                }
                allKeys = allKeys.OrderBy(k => k.time).ToList();

                //create dope key times and cache related data
                currentTimes = new List<float>();
                tangentModes = new List<TangentMode>();
                keyLabels = new List<string>();
                for ( var i = 0; i < allKeys.Count; i++ ) {
                    var key = allKeys[i];
                    if ( !currentTimes.Any(t => Mathf.Abs(t - key.time) <= KEY_PROXIMITY_TOLERANCE) ) {
                        currentTimes.Add(key.time);

                        //cache tangent mode
                        var keyTangent = CurveUtility.GetKeyTangentMode(key);
                        foreach ( var otherKey in allKeys.Where(k => k.time == key.time) ) {
                            var otherKeyTangent = CurveUtility.GetKeyTangentMode(otherKey);
                            if ( otherKeyTangent != keyTangent ) {
                                keyTangent = TangentMode.Editable;
                                break;
                            }
                        }
                        tangentModes.Add(keyTangent);

                        //cache key labels
                        keyLabels.Add(string.Format("<size=8>{0}</size>", animatable.GetKeyLabel(key.time)));
                    }
                }
            }
        }

    }
}

#endif