// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

#pragma warning disable IDE0018 // Inline variable declaration.
#pragma warning disable IDE0031 // Use null propagation.

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Internal]
    /// A custom inspector for an <see cref="AnimancerLayer"/> which sorts and exposes some of its internal values.
    /// </summary>
    public sealed class AnimancerLayerDrawer
    {
        /************************************************************************************************************************/

        /// <summary>The target layer. Set by <see cref="GatherStates"/>.</summary>
        public AnimancerLayer Layer { get; private set; }

        /// <summary>The states in the target layer which have non-zero <see cref="AnimancerState.Weight"/>.</summary>
        public readonly List<AnimancerState> ActiveStates = new List<AnimancerState>();

        /// <summary>The states in the target layer which have zero <see cref="AnimancerState.Weight"/>.</summary>
        public readonly List<AnimancerState> InactiveStates = new List<AnimancerState>();

        /************************************************************************************************************************/

        /// <summary>
        /// Initialises an editor in the list for each layer in the 'animancer'.
        /// <para></para>
        /// The 'count' indicates the number of elements actually being used. Spare elements are kept in the list in
        /// case they need to be used again later.
        /// </summary>
        internal static void GatherLayerEditors(AnimancerPlayable animancer, List<AnimancerLayerDrawer> editors, out int count)
        {
            count = animancer.LayerCount;
            for (int i = 0; i < count; i++)
            {
                AnimancerLayerDrawer editor;
                if (editors.Count <= i)
                {
                    editor = new AnimancerLayerDrawer();
                    editors.Add(editor);
                }
                else
                {
                    editor = editors[i];
                }

                editor.GatherStates(animancer.GetLayer(i));
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Sets the target 'layer' and sorts its states and their keys into the active/inactive lists.
        /// </summary>
        private void GatherStates(AnimancerLayer layer)
        {
            Layer = layer;

            ActiveStates.Clear();
            InactiveStates.Clear();

            foreach (var state in layer)
            {
                if (HideInactiveStates && state.Weight == 0)
                    continue;

                if (!SeparateActiveFromInactiveStates || state.Weight != 0)
                {
                    ActiveStates.Add(state);
                }
                else
                {
                    InactiveStates.Add(state);
                }
            }

            SortAndGatherKeys(ActiveStates);
            SortAndGatherKeys(InactiveStates);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Sorts any entries that use another state as their key to come right after that state.
        /// See <see cref="AnimancerPlayable.CrossFadeFromStart"/>.
        /// </summary>
        private static void SortAndGatherKeys(List<AnimancerState> states)
        {
            var count = states.Count;
            if (count == 0)
                return;

            if (SortStatesByName)
            {
                states.Sort((x, y) =>
                {
                    if (x.MainObject == null)
                        return y.MainObject == null ? 0 : 1;
                    else if (y.MainObject == null)
                        return -1;

                    return x.MainObject.name.CompareTo(y.MainObject.name);
                });
            }

            // Sort any states that use another state as their key to be right after the key.
            for (int i = 0; i < states.Count; i++)
            {
                var state = states[i];
                var key = state.Key;

                var keyState = key as AnimancerState;
                if (keyState == null)
                    continue;

                var keyStateIndex = states.IndexOf(keyState);
                if (keyStateIndex < 0 || keyStateIndex + 1 == i)
                    continue;

                states.RemoveAt(i);

                if (keyStateIndex < i)
                    keyStateIndex++;

                states.Insert(keyStateIndex, state);

                i--;
            }
        }

        /************************************************************************************************************************/

        /// <summary>Draws all states in the given layer.</summary>
        public void DoInspectorGUI(IAnimancerComponent owner)
        {
            AnimancerEditorUtilities.BeginVerticalBox(GUI.skin.box);

            DoHeaderGUI();

            if (HideInactiveStates)
            {
                DoStatesGUI("Active States", ActiveStates, owner);
            }
            else if (SeparateActiveFromInactiveStates)
            {
                DoStatesGUI("Active States", ActiveStates, owner);
                DoStatesGUI("Inactive States", InactiveStates, owner);
            }
            else
            {
                DoStatesGUI("States", ActiveStates, owner);
            }

            if (Layer.PortIndex == 0 &&
                Layer.Weight != 0 &&
                !Layer.IsAdditive &&
                !Mathf.Approximately(Layer.GetTotalWeight(), 1))
            {
                EditorGUILayout.HelpBox(
                    "The total Weight of all states in this layer does not equal 1, which will likely give undesirable results." +
                    " Click here for more information.",
                    MessageType.Warning);

                if (AnimancerEditorUtilities.TryUseClickInLastRect())
                    EditorUtility.OpenWithDefaultApp(
                        AnimancerPlayable.APIDocumentationURL + "/docs/manual/smooth-transitions");
            }

            AnimancerEditorUtilities.EndVerticalBox(GUI.skin.box);

            var totalArea = GUILayoutUtility.GetLastRect();
            HandleDragAndDropAnimations(totalArea, owner, Layer.PortIndex);
            CheckContextMenu(totalArea);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Draws the name and other details of the <see cref="Layer"/> in the GUI.
        /// </summary>
        private void DoHeaderGUI()
        {
            if (Layer.Root.LayerCount <= 1 && Layer.Weight == 1 && Layer._Mask == null)
                return;

            var area = AnimancerEditorUtilities.GetRect();

            const float FoldoutIndent = 12;
            area.xMin += FoldoutIndent;
            Layer._IsInspectorExpanded = EditorGUI.Foldout(area, Layer._IsInspectorExpanded, GUIContent.none, true);

            AnimancerEditorUtilities.DoWeightLabelGUI(ref area, Layer.Weight);

            var label = Layer.IsAdditive ? "Additive" : "Override";
            if (Layer._Mask != null)
                label = string.Concat(label, " (", Layer._Mask.name, ")");

            EditorGUIUtility.labelWidth -= FoldoutIndent;
            EditorGUI.LabelField(area, Layer.Name, label);
            EditorGUIUtility.labelWidth += FoldoutIndent;

            if (Layer._IsInspectorExpanded)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(FoldoutIndent);
                GUILayout.BeginVertical();

                EditorGUI.indentLevel++;

                Layer.IsAdditive = EditorGUILayout.Toggle("Is Additive", Layer.IsAdditive);

                EditorGUI.BeginChangeCheck();
                Layer._Mask = (AvatarMask)EditorGUILayout.ObjectField("Mask", Layer._Mask, typeof(AvatarMask), false);
                if (EditorGUI.EndChangeCheck())
                    Layer.SetMask(Layer._Mask);

                EditorGUI.BeginChangeCheck();
                var weight = EditorGUILayout.FloatField("Weight", Layer.Weight);
                if (EditorGUI.EndChangeCheck())
                    Layer.Weight = weight;

                area = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                Layer.DoFadeDetailsGUI(area);

                EditorGUI.indentLevel--;

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        /************************************************************************************************************************/

        /// <summary>Draws all 'states' in the given list.</summary>
        private void DoStatesGUI(string label, List<AnimancerState> states, IAnimancerComponent owner)
        {
            var area = AnimancerEditorUtilities.GetRect();

            var width = GUI.skin.label.CalculateWidth("Weight");
            GUI.Label(AnimancerEditorUtilities.StealWidth(ref area, width), "Weight");

            EditorGUI.LabelField(area, label, states.Count.ToString());

            EditorGUI.indentLevel++;
            for (int i = 0; i < states.Count; i++)
            {
                DoStateGUI(states[i], owner);
            }
            EditorGUI.indentLevel--;
        }

        /************************************************************************************************************************/

        /// <summary>Cached inspectors that have already been created for states.</summary>
        private readonly Dictionary<AnimancerState, IAnimancerStateDrawer>
            StateInspectors = new Dictionary<AnimancerState, IAnimancerStateDrawer>();

        /// <summary>Draws the inspector for the given 'state'.</summary>
        private void DoStateGUI(AnimancerState state, IAnimancerComponent owner)
        {
            IAnimancerStateDrawer inspector;
            if (!StateInspectors.TryGetValue(state, out inspector))
            {
                inspector = state.GetDrawer();
                StateInspectors.Add(state, inspector);
            }

            inspector.DoGUI(owner);
            DoChildStatesGUI(state, owner);
        }

        /************************************************************************************************************************/

        /// <summary>Draws all child states of the 'state'.</summary>
        private void DoChildStatesGUI(AnimancerState state, IAnimancerComponent owner)
        {
            EditorGUI.indentLevel++;

            foreach (var child in state)
            {
                if (child == null)
                    continue;

                DoStateGUI(child, owner);
            }

            EditorGUI.indentLevel--;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// If <see cref="AnimationClip"/>s or <see cref="IAnimationClipSource"/>s are dropped inside the 'dropArea',
        /// this method creates a new state in the 'target' for each animation.
        /// </summary>
        public static void HandleDragAndDropAnimations(Rect dropArea, IAnimancerComponent target, int layerIndex)
        {
            AnimancerEditorUtilities.HandleDragAndDropAnimations(dropArea, (clip) =>
            {
                target.GetOrCreateState(clip, layerIndex);
            });
        }

        /************************************************************************************************************************/
#region Context Menu
        /************************************************************************************************************************/

        /// <summary>
        /// Checks if the current event is a context menu click within the 'clickArea' and opens a context menu with various
        /// functions for the <see cref="Layer"/>.
        /// </summary>
        private void CheckContextMenu(Rect clickArea)
        {
            if (!AnimancerEditorUtilities.TryUseContextClick(clickArea))
                return;

            var menu = new GenericMenu();

            menu.AddDisabledItem(new GUIContent(Layer.ToString()));

            AnimancerEditorUtilities.AddMenuItem(menu, "Stop",
                HasAnyStates((state) => state.IsPlaying || state.Weight != 0),
                () => Layer.Stop());

            AnimancerEditorUtilities.AddFadeFunction(menu, "Fade In",
                Layer.PortIndex > 0 && Layer.Weight != 1, Layer,
                (duration) => Layer.StartFade(1, duration));
            AnimancerEditorUtilities.AddFadeFunction(menu, "Fade Out",
                Layer.PortIndex > 0 && Layer.Weight != 0, Layer,
                (duration) => Layer.StartFade(0, duration));

            menu.AddItem(new GUIContent("Inverse Kinematics/Apply Animator IK"),
                Layer.ApplyAnimatorIK,
                () => Layer.ApplyAnimatorIK = !Layer.ApplyAnimatorIK);
            menu.AddItem(new GUIContent("Inverse Kinematics/Default Apply Animator IK"),
                Layer.DefaultApplyAnimatorIK,
                () => Layer.DefaultApplyAnimatorIK = !Layer.DefaultApplyAnimatorIK);
            menu.AddItem(new GUIContent("Inverse Kinematics/Apply Foot IK"),
                Layer.ApplyFootIK,
                () => Layer.ApplyFootIK = !Layer.ApplyFootIK);
            menu.AddItem(new GUIContent("Inverse Kinematics/Default Apply Foot IK"),
                Layer.DefaultApplyFootIK,
                () => Layer.DefaultApplyFootIK = !Layer.DefaultApplyFootIK);

            menu.AddSeparator("");

            AnimancerEditorUtilities.AddMenuItem(menu, "Destroy States",
                ActiveStates.Count > 0 || InactiveStates.Count > 0,
                () => Layer.DestroyStates());

            AnimancerEditorUtilities.AddMenuItem(menu, "Add Layer",
                Layer.Root.LayerCount < AnimancerPlayable.maxLayerCount,
                () => Layer.Root.LayerCount++);
            AnimancerEditorUtilities.AddMenuItem(menu, "Remove Layer",
                Layer.Root.LayerCount > 0,
                () => Layer.Root.LayerCount--);

            menu.AddSeparator("");

            menu.AddItem(new GUIContent("Keep Weightless Playables Connected"),
                Layer.Root.KeepPlayablesConnected,
                () => Layer.Root.KeepPlayablesConnected = !Layer.Root.KeepPlayablesConnected);

            AddPrefFunctions(menu);

            menu.AddSeparator("");

            AnimancerEditorUtilities.AddDocumentationLink(menu, "Layer Documentation", "/docs/manual/animation-layers");
            AddPlayableGraphVisualizerFunction(menu);

            menu.ShowAsContext();
        }

        /************************************************************************************************************************/

        private bool HasAnyStates(Func<AnimancerState, bool> condition)
        {
            foreach (var state in Layer)
            {
                if (condition(state))
                    return true;
            }

            return false;
        }

        /************************************************************************************************************************/

        private void AddPlayableGraphVisualizerFunction(GenericMenu menu)
        {
            var type = Type.GetType("GraphVisualizer.PlayableGraphVisualizerWindow, Unity.PlayableGraphVisualizer.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            if (type == null)
                return;

            menu.AddItem(new GUIContent("Playable Graph Visualizer"), false, () =>
            {
                var window = EditorWindow.GetWindow(type);

                var field = type.GetField("m_CurrentGraph",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (field != null)
                    field.SetValue(window, Layer.Root._Graph);
            });
        }

        /************************************************************************************************************************/
#endregion
        /************************************************************************************************************************/
#region Prefs
        /************************************************************************************************************************/

        internal const string PrefPrefix = "Display Options/";
        private static readonly BoolPref
            SortStatesByName = new BoolPref(PrefPrefix + "Sort By Name", true),
            HideInactiveStates = new BoolPref(PrefPrefix + "Hide Inactive", false),
            SeparateActiveFromInactiveStates = new BoolPref(PrefPrefix + "Separate Active From Inactive", false);
        internal static readonly BoolPref
            ShowUpdatingNodes = new BoolPref(PrefPrefix + "Show Dirty Nodes", false);

        /************************************************************************************************************************/

        private static void AddPrefFunctions(GenericMenu menu)
        {
            SortStatesByName.AddToggleFunction(menu);
            HideInactiveStates.AddToggleFunction(menu);
            SeparateActiveFromInactiveStates.AddToggleFunction(menu);
            ShowUpdatingNodes.AddToggleFunction(menu);
            AnimancerPlayable.RepaintConstantlyInEditMode.AddToggleFunction(menu);
        }

        /************************************************************************************************************************/
#endregion
        /************************************************************************************************************************/
    }
}

#endif
