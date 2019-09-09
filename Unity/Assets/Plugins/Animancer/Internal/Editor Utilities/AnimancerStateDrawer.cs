// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

#pragma warning disable IDE0018 // Inline variable declaration.
#pragma warning disable IDE0041 // Use 'is null' check.

using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only] Draws the inspector GUI for an <see cref="AnimancerState"/>.</summary>
    public interface IAnimancerStateDrawer
    {
        /// <summary>Draws the details and controls for the target state in the inspector.</summary>
        void DoGUI(IAnimancerComponent owner);
    }

    /************************************************************************************************************************/

    /// <summary>[Editor-Only] Draws the inspector GUI for an <see cref="AnimancerState"/>.</summary>
    public class AnimancerStateDrawer<T> : IAnimancerStateDrawer where T : AnimancerState
    {
        /************************************************************************************************************************/

        /// <summary>The state being managed.</summary>
        public readonly T State;

        /// <summary>If true, the details of this state will be expanded in the inspector.</summary>
        private bool _IsExpanded;

        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="AnimancerStateDrawer{T}"/> to manage the inspector GUI for the 'state'.
        /// </summary>
        public AnimancerStateDrawer(T state)
        {
            State = state;
        }

        /************************************************************************************************************************/

        /// <summary>Draws the details and controls for the target <see cref="State"/> in the inspector.</summary>
        public virtual void DoGUI(IAnimancerComponent owner)
        {
            GUILayout.BeginVertical();
            {
                var position = AnimancerEditorUtilities.GetRect(true);

                string label;
                DoFoldoutGUI(position, out label);
                DoLabelGUI(ref position, label);

                if (_IsExpanded)
                    DoDetailsGUI(owner);
            }
            GUILayout.EndVertical();

            CheckContextMenu(GUILayoutUtility.GetLastRect());
        }

        /************************************************************************************************************************/

        /// <summary>Draws a foldout arrow to expand/collapse the state details.</summary>
        protected void DoFoldoutGUI(Rect area, out string label)
        {
            var key = State.Key;

            bool isAssetUsedAsKey = key == null || ReferenceEquals(key, State.MainObject);

            float foldoutWidth;
            if (isAssetUsedAsKey)
            {
                foldoutWidth = EditorGUI.indentLevel * AnimancerEditorUtilities.IndentSize;
                label = "";
            }
            else
            {
                foldoutWidth = EditorGUIUtility.labelWidth;

                if (key is string)
                    label = "\"" + key + "\"";
                else
                    label = key.ToString();
            }

            area.xMin -= 2;
            area.width = foldoutWidth;

            _IsExpanded = EditorGUI.Foldout(area, _IsExpanded, GUIContent.none, !isAssetUsedAsKey);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Draws the state's main label: an <see cref="Object"/> field if it has a
        /// <see cref="AnimancerState.MainObject"/>, otherwise just a simple text label.
        /// <para></para>
        /// Also handles Ctrl + Click on the label to CrossFade the animation and shows a bar to indicate its progress.
        /// </summary>
        protected virtual void DoLabelGUI(ref Rect area, string label)
        {
            var currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseUp &&
                currentEvent.control &&
                area.Contains(currentEvent.mousePosition))
            {
                var fadeDuration = State.CalculateEditorFadeDuration(AnimancerPlayable.DefaultFadeDuration);
                State.Root.CrossFade(State, fadeDuration);
                currentEvent.Use();
            }

            AnimancerEditorUtilities.DoWeightLabelGUI(ref area, State.Weight);

            if (!ReferenceEquals(State.MainObject, null))
            {
                EditorGUI.BeginChangeCheck();

                EditorGUI.ObjectField(area, label, State.MainObject, typeof(Object), false);

                if (EditorGUI.EndChangeCheck())
                    Debug.LogWarning("Assigning an asset to a state using the inspector is not supported." +
                        " You can drag and drop an AnimationClip onto the layer header if you wish to create a new state for it.");
            }
            else
            {
                EditorGUI.LabelField(area, label, State.ToString());
            }

            // Highlight a section of the label based on the time like a loading bar.
            if (State.HasLength && (State.IsPlaying || State.Time != 0))
            {
                var color = GUI.color;

                // Green = Playing, Yelow = Paused.
                GUI.color = State.IsPlaying ? new Color(0.25f, 1, 0.25f, 0.25f) : new Color(1, 1, 0.25f, 0.25f);

                area.xMin += AnimancerEditorUtilities.IndentSize;
                area.width -= 18;

                float length;
                float wrappedTime = GetWrappedTime(out length);
                if (length > 0)
                    area.width *= Mathf.Clamp01(wrappedTime / length);

                GUI.Box(area, GUIContent.none);

                GUI.color = color;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Gets the current <see cref="AnimancerState.Time"/>.
        /// If the 'state' is looping, the value is modulo by the <see cref="AnimancerState.Length"/>.
        /// </summary>
        private float GetWrappedTime(out float length)
        {
            var time = State.Time;
            length = State.Length;

            if (State.IsLooping)
            {
                time %= length;
                if (time < 0)
                    time += length;
                else if (time == 0 && State.Time != 0)
                    time = length;
            }

            return time;
        }

        /************************************************************************************************************************/

        /// <summary> Draws the details of the target state in the GUI.</summary>
        protected virtual void DoDetailsGUI(IAnimancerComponent owner)
        {
            EditorGUI.indentLevel++;
            DoTimeSliderGUI();
            DoPlayingDetailsGUI();
            State.DoFadeDetailsGUI(AnimancerEditorUtilities.GetRect(true));
            DoOnEndGUI();
            EditorGUI.indentLevel--;
        }

        /************************************************************************************************************************/

        /// <summary>Draws a slider for controlling the current <see cref="AnimancerState.Time"/>.</summary>
        private void DoTimeSliderGUI()
        {
            if (!State.HasLength)
                return;

            float length;
            var time = GetWrappedTime(out length);

            if (length == 0)
                return;

            var area = AnimancerEditorUtilities.GetRect(true);

            var normalized = DoNormalizedTimeToggle(ref area);

            string label;
            float max;
            if (normalized)
            {
                label = "Normalized Time";
                time /= length;
                max = 1;
            }
            else
            {
                label = "Time";
                max = length;
            }

            DoLoopCounterGUI(ref area, length);

            EditorGUI.BeginChangeCheck();
            label = AnimancerEditorUtilities.BeginTightLabel(label);
            time = EditorGUI.Slider(area, label, time, 0, max);
            AnimancerEditorUtilities.EndTightLabel();
            if (EditorGUI.EndChangeCheck())
            {
                if (normalized)
                    State.NormalizedTime = time;
                else
                    State.Time = time;

                State.Root.Evaluate();
            }
        }

        /************************************************************************************************************************/

        private static readonly BoolPref UseNormalizedTimeSliders = new BoolPref("UseNormalizedTimeSliders", false);

        private static float _UseNormalizedTimeSlidersWidth;

        private bool DoNormalizedTimeToggle(ref Rect area)
        {
            var content = AnimancerEditorUtilities.TempContent("N");
            var style = AnimancerEditorUtilities.Styles.MiniButton;

            if (_UseNormalizedTimeSlidersWidth == 0)
                _UseNormalizedTimeSlidersWidth = style.CalculateWidth(content);

            var toggleArea = AnimancerEditorUtilities.StealWidth(ref area, _UseNormalizedTimeSlidersWidth);

            UseNormalizedTimeSliders.Value = GUI.Toggle(toggleArea, UseNormalizedTimeSliders, content, style);
            return UseNormalizedTimeSliders;
        }

        /************************************************************************************************************************/

        private void DoLoopCounterGUI(ref Rect area, float length)
        {
            int loops = Mathf.Abs((int)(State.Time / length));
            var label = "x" + loops;
            var style = GUI.skin.label;

            var width = style.CalculateWidth(label);

            var labelArea = AnimancerEditorUtilities.StealWidth(ref area, width);

            GUI.Label(labelArea, label);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Draws controls for <see cref="AnimancerState.IsPlaying"/>, <see cref="AnimancerState.Speed"/>, and
        /// <see cref="AnimancerState.Weight"/>.
        /// </summary>
        protected void DoPlayingDetailsGUI()
        {
            var labelWidth = EditorGUIUtility.labelWidth;

            var area = AnimancerEditorUtilities.GetRect(true);

            var right = area.xMax;

            // Is Playing.
            var label = AnimancerEditorUtilities.BeginTightLabel("Is Playing");
            area.width = EditorGUIUtility.labelWidth + 16;
            State.IsPlaying = EditorGUI.Toggle(area, label, State.IsPlaying);
            AnimancerEditorUtilities.EndTightLabel();

            area.x += area.width;
            area.xMax = right;

            float speedWidth, weightWidth;
            Rect speedRect, weightRect;
            AnimancerEditorUtilities.SplitHorizontally(area, "Speed", "Weight", out speedWidth, out weightWidth, out speedRect, out weightRect);

            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Speed.
            EditorGUIUtility.labelWidth = speedWidth;
            EditorGUI.BeginChangeCheck();
            var speed = EditorGUI.FloatField(speedRect, "Speed", State.Speed);
            if (EditorGUI.EndChangeCheck())
                State.Speed = speed;

            // Weight.
            EditorGUIUtility.labelWidth = weightWidth;
            EditorGUI.BeginChangeCheck();
            var weight = EditorGUI.FloatField(weightRect, "Weight", State.Weight);
            if (EditorGUI.EndChangeCheck())
                State.Weight = weight;

            EditorGUI.indentLevel = indentLevel;
            EditorGUIUtility.labelWidth = labelWidth;
        }

        /************************************************************************************************************************/

        private void DoOnEndGUI()
        {
            if (State.OnEnd == null)
                return;

            var area = AnimancerEditorUtilities.GetRect(true);

            EditorGUI.LabelField(area, "OnEnd: " + State.OnEnd.Method);
        }

        /************************************************************************************************************************/
        #region Context Menu
        /************************************************************************************************************************/

        /// <summary>
        /// The menu label prefix used for details about the target state.
        /// </summary>
        protected const string DetailsPrefix = "Details/";

        /// <summary>
        /// Checks if the current event is a context menu click within the 'clickArea' and opens a context menu with various
        /// functions for the <see cref="State"/>.
        /// </summary>
        protected void CheckContextMenu(Rect clickArea)
        {
            if (!AnimancerEditorUtilities.TryUseContextClick(clickArea))
                return;

            var menu = new GenericMenu();

            menu.AddDisabledItem(new GUIContent(DetailsPrefix + "State: " + State.ToString()));

            var key = State.Key;

            if (key != null)
                menu.AddDisabledItem(new GUIContent(DetailsPrefix + "Key: " + key));

            AnimancerEditorUtilities.AddMenuItem(menu, "Play",
                !State.IsPlaying || State.Weight != 1,
                () => State.Root.Play(State));

            AnimancerEditorUtilities.AddFadeFunction(menu, "Cross Fade (Ctrl + Click)",
                State.Weight != 1,
                State, (duration) => State.Root.CrossFade(State, duration));

            AddContextMenuFunctions(menu);

            menu.AddItem(new GUIContent(DetailsPrefix + "Log Details"), false,
                () => Debug.Log("AnimancerState: " + State.GetDescription(true)));

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Destroy State"), false, () => State.Dispose());

            menu.AddSeparator("");
            AnimancerEditorUtilities.AddDocumentationLink(menu, "State Documentation", "/docs/manual/animancer-states");

            menu.ShowAsContext();
        }

        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Adds the details of this state to the menu.
        /// By default, that means a single item showing the path of the <see cref="AnimancerState.MainObject"/>.
        /// </summary>
        protected virtual void AddContextMenuFunctions(GenericMenu menu)
        {
            if (State.HasLength)
                menu.AddDisabledItem(new GUIContent(DetailsPrefix + "Length: " + State.Length));

            menu.AddDisabledItem(new GUIContent(DetailsPrefix + "Playable Path: " + State.GetPath()));

            var mainAsset = State.MainObject;
            if (mainAsset != null)
            {
                var assetPath = AssetDatabase.GetAssetPath(mainAsset);
                if (assetPath != null)
                    menu.AddDisabledItem(new GUIContent(DetailsPrefix + "Asset Path: " + assetPath.Replace("/", "->")));
            }

            if (State.OnEnd != null)
            {
                const string OnEndPrefix = "On End/";

                var label = OnEndPrefix +
                    (State.OnEnd.Target != null ? ("Target: " + State.OnEnd.Target) : "Target: null");

                var targetObject = State.OnEnd.Target as Object;
                AnimancerEditorUtilities.AddMenuItem(menu, label,
                    targetObject != null,
                    () => Selection.activeObject = targetObject);

                menu.AddDisabledItem(new GUIContent(OnEndPrefix + "Declaring Type: " + State.OnEnd.Method.DeclaringType.FullName));
                menu.AddDisabledItem(new GUIContent(OnEndPrefix + "Method: " + State.OnEnd.Method));

                menu.AddItem(new GUIContent(OnEndPrefix + "Clear"), false, () => State.OnEnd = null);
                menu.AddItem(new GUIContent(OnEndPrefix + "Invoke"), false, () => State.OnEnd());
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}

#endif
