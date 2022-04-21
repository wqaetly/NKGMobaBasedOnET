#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Slate
{

    ///A curve editor and renderer using Unity's native one by reflection
    public static class CurveEditor
    {

        ///Raised when CurveEditor modifies curves, with argument being the IAnimatable the curves belong to.
        public static event Action<IAnimatableData> onCurvesUpdated;

        private static Dictionary<IAnimatableData, CurveRenderer> cache = new Dictionary<IAnimatableData, CurveRenderer>();
        public static void DrawCurves(IAnimatableData animatable, IKeyable keyable, Rect posRect, Rect timeRect) {
            CurveRenderer instance = null;
            if ( !cache.TryGetValue(animatable, out instance) ) {
                cache[animatable] = instance = new CurveRenderer(animatable, keyable, posRect);
            }
            instance.Draw(posRect, timeRect);
        }


        static CurveEditor() {
            AnimatedParameter.onParameterChanged += RefreshCurvesOf;
            DopeSheetEditor.onCurvesUpdated += RefreshCurvesOf;
            CurveEditor3D.onCurvesUpdated += RefreshCurvesOf;
            CutsceneUtility.onRefreshAllAnimationEditors += RefreshCurvesOf;
        }

        ///Refresh curves of target animatable
        static void RefreshCurvesOf(IAnimatableData animatable) {
            CurveRenderer curveRenderer = null;
            if ( cache.TryGetValue(animatable, out curveRenderer) ) {
                curveRenderer.RefreshCurves();
                return;
            }

            if ( animatable is AnimationDataCollection ) {
                var data = (AnimationDataCollection)animatable;
                if ( data.animatedParameters != null ) {
                    foreach ( var animParam in data.animatedParameters ) {
                        if ( cache.TryGetValue(animParam, out curveRenderer) ) {
                            curveRenderer.RefreshCurves();
                        }
                    }
                }
            }
        }


        public static void FrameAllCurvesOf(IAnimatableData animatable) {
            CurveRenderer instance = null;
            if ( !cache.TryGetValue(animatable, out instance) ) {
                return;
            }
            instance.RecalculateBounds();
            instance.FrameClip(true, true);
        }

        ///----------------------------------------------------------------------------------------------

        ///The actual class responsible
        public class CurveRenderer
        {

            public IAnimatableData animatable;
            public IKeyable keyable;

            private AnimationCurve[] curves;
            private Rect posRect;
            private Rect timeRect;

            private static Assembly editorAssembly;
            private static Type cEditorType;
            private static Type cRendererType;
            private static Type cWrapperType;
            private static ConstructorInfo cEditorCTR;

            private object cEditor;

            public CurveRenderer(IAnimatableData animatable, IKeyable keyable, Rect posRect) {
                this.animatable = animatable;
                this.keyable = keyable;
                this.curves = animatable.GetCurves();
                this.posRect = posRect;
                Undo.undoRedoPerformed += () => { RefreshCurves(); };
                Init();
            }

            public CurveRenderer(AnimationCurve[] curves, Rect posRect) {
                this.curves = curves;
                Undo.undoRedoPerformed += () => { RefreshCurves(); };
                Init();
            }

            public void Init() {
                //init meta info
                editorAssembly = typeof(Editor).Assembly;
                cEditorType = editorAssembly.GetType("UnityEditor.CurveEditor");
                cRendererType = editorAssembly.GetType("UnityEditor.NormalCurveRenderer");
                cWrapperType = editorAssembly.GetType("UnityEditor.CurveWrapper");
                cEditorCTR = cEditorType.GetConstructor(new Type[] { typeof(Rect), cWrapperType.MakeArrayType(), typeof(bool) });

                //create curve editor with wrappers
                var wrapperArray = GetCurveWrapperArray(curves);
                cEditor = cEditorCTR.Invoke(new object[] { posRect, wrapperArray, true });

                CreateDelegates();

                //set settings
                var settings = GetCurveEditorSettings();
                cEditorType.GetProperty("settings").SetValue(cEditor, settings, null);

                invSnap = 1f / Prefs.snapInterval;
                lastSnapPref = Prefs.snapInterval;
                ignoreScrollWheelUntilClicked = true;

                RecalculateBounds();
                FrameClip(true, true);
            }

            private Action onGUI;
            private float lastSnapPref;

            private Action<Rect> rectSetter;
            private Func<Rect> rectGetter;

            private Action<Rect> shownAreaSetter;
            private Func<Rect> shownAreaGetter;

            private Action<float> hRangeMaxSetter;
            private Func<float> hRangeMaxGetter;


            Array GetCurveWrapperArray(AnimationCurve[] curves) {
                if ( curves == null ) {
                    return Array.CreateInstance(cWrapperType, 0);
                }

                var wrapperArray = Array.CreateInstance(cWrapperType, curves.Length);
                for ( var i = 0; i < curves.Length; i++ ) {
                    var curve = curves[i];
                    var cWrapper = Activator.CreateInstance(cWrapperType);

                    var clr = Color.white;
                    if ( i == 0 ) clr = Color.red;
                    if ( i == 1 ) clr = Color.green;
                    if ( i == 2 ) clr = Color.blue;
                    cWrapperType.GetField("color").SetValue(cWrapper, clr);
                    cWrapperType.GetField("id").SetValue(cWrapper, i);
                    var cRenderer = Activator.CreateInstance(cRendererType, new object[] { curve });
                    cWrapperType.GetProperty("renderer").SetValue(cWrapper, cRenderer, null);

                    var setWrapMethod = cRendererType.GetMethod("SetWrap", new Type[] { typeof(WrapMode), typeof(WrapMode) });
                    setWrapMethod.Invoke(cRenderer, new object[] { curve.preWrapMode, curve.postWrapMode });

                    wrapperArray.SetValue(cWrapper, i);
                }

                return wrapperArray;
            }

            object GetCurveEditorSettings() {
                var settingsType = editorAssembly.GetType("UnityEditor.CurveEditorSettings");
                var settings = Activator.CreateInstance(settingsType);
                settingsType.GetField("allowDraggingCurvesAndRegions").SetValue(settings, false);
                settingsType.GetField("allowDeleteLastKeyInCurve").SetValue(settings, true);

                settingsType.GetField("undoRedoSelection").SetValue(settings, true);
                settingsType.GetField("rectangleToolFlags", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(settings, 1);

                settingsType.GetProperty("hRangeLocked", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(settings, true, null);
                settingsType.GetProperty("vRangeLocked", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(settings, false, null);
                settingsType.GetProperty("hSlider").SetValue(settings, false, null);
                settingsType.GetProperty("vSlider").SetValue(settings, true, null);
                settingsType.GetProperty("hRangeMin").SetValue(settings, 0, null);

                return settings;
            }


            //create delegates for some properties and methods for performance
            void CreateDelegates() {

                onGUI = cEditorType.GetMethod("OnGUI").RTCreateDelegate<Action>(cEditor);

                rectSetter = cEditorType.GetProperty("rect").GetSetMethod().RTCreateDelegate<Action<Rect>>(cEditor);
                rectGetter = cEditorType.GetProperty("rect").GetGetMethod().RTCreateDelegate<Func<Rect>>(cEditor);

                shownAreaSetter = cEditorType.GetProperty("shownArea").GetSetMethod().RTCreateDelegate<Action<Rect>>(cEditor);
                shownAreaGetter = cEditorType.GetProperty("shownArea").GetGetMethod().RTCreateDelegate<Func<Rect>>(cEditor);

                hRangeMaxSetter = cEditorType.GetProperty("hRangeMax").GetSetMethod().RTCreateDelegate<Action<float>>(cEditor);
                hRangeMaxGetter = cEditorType.GetProperty("hRangeMax").GetGetMethod().RTCreateDelegate<Func<float>>(cEditor);


                //Append OnCurvesUpdated to curve editor event
                var field = cEditorType.GetField("curvesUpdated");
                var methodInfo = this.GetType().GetMethod("OnCurvesUpdated", BindingFlags.Instance | BindingFlags.NonPublic);
                Delegate handler = Delegate.CreateDelegate(field.FieldType, this, methodInfo);
                field.SetValue(cEditor, handler);
            }


            public Rect rect {
                get { return rectGetter(); }
                set { rectSetter(value); }
            }

            public Rect shownArea {
                get { return shownAreaGetter(); }
                set { shownAreaSetter(value); }
            }

            public bool ignoreScrollWheelUntilClicked {
                get { return (bool)cEditorType.GetProperty("ignoreScrollWheelUntilClicked").GetValue(cEditor, null); }
                set { cEditorType.GetProperty("ignoreScrollWheelUntilClicked").SetValue(cEditor, value, null); }
            }

            public bool hRangeLocked {
                get { return (bool)cEditorType.GetProperty("hRangeLocked").GetValue(cEditor, null); }
                set { cEditorType.GetProperty("hRangeLocked").SetValue(cEditor, value, null); }
            }

            public bool vRangeLocked {
                get { return (bool)cEditorType.GetProperty("vRangeLocked").GetValue(cEditor, null); }
                set { cEditorType.GetProperty("vRangeLocked").SetValue(cEditor, value, null); }
            }

            public bool hSlider {
                get { return (bool)cEditorType.GetProperty("hSlider").GetValue(cEditor, null); }
                set { cEditorType.GetProperty("hSlider").SetValue(cEditor, value, null); }
            }

            public bool vSlider {
                get { return (bool)cEditorType.GetProperty("vSlider").GetValue(cEditor, null); }
                set { cEditorType.GetProperty("vSlider").SetValue(cEditor, value, null); }
            }

            public float hRangeMin {
                get { return (float)cEditorType.GetProperty("hRangeMin").GetValue(cEditor, null); }
                set { cEditorType.GetProperty("hRangeMin").SetValue(cEditor, value, null); }
            }

            public float hRangeMax {
                get { return hRangeMaxGetter(); }
                set { hRangeMaxSetter(value); }
            }

            /*
                        public Rect shownAreaInsideMargins{
                            get {return (Rect)cEditorType.GetProperty("shownAreaInsideMargins").GetValue(cEditor, null);}
                            set {cEditorType.GetProperty("shownAreaInsideMargins").SetValue(cEditor, value, null);}
                        }

                        public bool enableMouseInput{
                            get {return (bool)cEditorType.GetProperty("enableMouseInput").GetValue(cEditor, null);}
                            set {cEditorType.GetProperty("enableMouseInput").SetValue(cEditor, value, null);}
                        }

                        public bool hAllowExceedBaseRangeMin{
                            get {return (bool)cEditorType.GetProperty("hAllowExceedBaseRangeMin").GetValue(cEditor, null);}
                            set {cEditorType.GetProperty("hAllowExceedBaseRangeMin").SetValue(cEditor, value, null);}								
                        }

                        public float vRangeMin{
                            get {return (float)cEditorType.GetProperty("vRangeMin").GetValue(cEditor, null);}
                            set {cEditorType.GetProperty("vRangeMin").SetValue(cEditor, value, null);}
                        }

                        public float vRangeMax{
                            get {return (float)cEditorType.GetProperty("vRangeMax").GetValue(cEditor, null);}
                            set {cEditorType.GetProperty("vRangeMax").SetValue(cEditor, value, null);}
                        }

                        public bool hasSelection{
                            get {return (bool)cEditorType.GetProperty("hasSelection").GetValue(cEditor, null);}
                            set {cEditorType.GetProperty("hasSelection").SetValue(cEditor, value, null);}
                        }

                        public void SetShownVRangeInsideMargins(float min, float max){
                            cEditorType.GetMethod("SetShownVRangeInsideMargins").Invoke(cEditor, new object[]{min, max});
                        }
            */

            public float invSnap {
                get { return (float)cEditorType.GetField("invSnap").GetValue(cEditor); }
                set { cEditorType.GetField("invSnap").SetValue(cEditor, value); }
            }

            public int axisLock {
                set { cEditorType.GetField("m_AxisLock", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(cEditor, value); }
            }

            public void FrameClip(bool h, bool v) {
                cEditorType.GetMethod("FrameClip").Invoke(cEditor, new object[] { h, v });
            }

            public void FrameSelected(bool h, bool v) {
                cEditorType.GetMethod("FrameSelected").Invoke(cEditor, new object[] { h, v });
            }

            public void SelectNone() {
                cEditorType.GetMethod("SelectNone").Invoke(cEditor, null);
            }

            public void RecalculateBounds() {
                cEditorType.GetProperty("animationCurves").SetValue(cEditor, GetCurveWrapperArray(curves), null);
            }

            public void RefreshCurves() {
                cEditorType.GetProperty("animationCurves").SetValue(cEditor, GetCurveWrapperArray(curves), null);
            }


            public void Draw(Rect posRect, Rect timeRect) {

                if ( curves == null || curves.Length == 0 ) {
                    GUI.Label(posRect, "No Animation Curves to Display", Styles.centerLabel);
                    return;
                }

                var e = Event.current;
                // hRangeMax = timeRect.xMax;
                rect = posRect;
                shownArea = Rect.MinMaxRect(timeRect.xMin, shownArea.yMin, timeRect.xMax, shownArea.yMax);

                if ( Prefs.lockHorizontalCurveEditing && e.rawType == EventType.MouseDrag ) {
                    axisLock = 2;
                }

                if ( Prefs.snapInterval != lastSnapPref ) {
                    lastSnapPref = Prefs.snapInterval;
                    invSnap = 1 / Prefs.snapInterval;
                }

                if ( e.rawType == EventType.MouseUp ) {
                    RecalculateBounds();
                    if ( GUIUtility.hotControl != 0 ) {
                        OnCurvesUpdated();
                    }
                }

                if ( e.type == EventType.MouseDown && e.button == 0 && e.clickCount == 2 && posRect.Contains(e.mousePosition) ) {
                    FrameClip(true, true);
                    e.Use();
                }


                //INFO
                GUI.color = new Color(1, 1, 1, 0.2f);
                var infoWidth = Mathf.Min(posRect.width, 125);
                var labelRect = new Rect(posRect.xMax - infoWidth - 10, posRect.y + 2, infoWidth, 18);
                GUI.Label(labelRect, "(F: Frame Selection)");
                labelRect.y += 18;
                GUI.Label(labelRect, "(Click x2: Frame All)");
                labelRect.y += 18;
                GUI.Label(labelRect, "(Alt+: Pan/Zoom)");
                GUI.color = Color.white;


                //OnGUI
                try { onGUI(); }
                catch ( Exception exc ) { SelectNone(); Debug.LogException(exc); }
            }

            //raise event
            void OnCurvesUpdated() {
                if ( onCurvesUpdated != null ) {
                    onCurvesUpdated(animatable);
                }
            }

        }
    }
}

#endif