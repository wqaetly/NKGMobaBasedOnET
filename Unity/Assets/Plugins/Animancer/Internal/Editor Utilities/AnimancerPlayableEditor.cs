// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only]
    /// A custom inspector for components with an <see cref="AnimancerPlayable"/>.
    /// </summary>
    public abstract class AnimancerPlayableEditor : UnityEditor.Editor
    {
        /************************************************************************************************************************/

        /// <summary>A lazy list of information about the layers currently being displayed.</summary>
        private readonly List<AnimancerLayerDrawer>
            LayerInfos = new List<AnimancerLayerDrawer>();

        /// <summary>The number of elements in <see cref="LayerInfos"/> that are currently being used.</summary>
        [NonSerialized]
        private int _LayerCount;

        [NonSerialized]
        private IAnimancerComponent[] _Targets;
        /// <summary><see cref="UnityEditor.Editor.targets"/> casted to <see cref="IAnimancerComponent"/>.</summary>
        public IAnimancerComponent[] Targets { get { return _Targets; } }

        /// <summary>The serialized backing field for the target's <see cref="Animator"/> reference.</summary>
        [NonSerialized]
        private NestedAnimatorEditor _AnimatorEditor;

        /************************************************************************************************************************/

        /// <summary>
        /// Initialises this <see cref="UnityEditor.Editor"/>.
        /// </summary>
        protected virtual void OnEnable()
        {
            var targets = this.targets;
            _Targets = new IAnimancerComponent[targets.Length];
            GatherTargets();

            _AnimatorEditor = new NestedAnimatorEditor(_Targets, serializedObject.FindProperty(_Targets[0].AnimatorFieldName));
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Copies the <see cref="UnityEditor.Editor.targets"/> into the <see cref="_Targets"/> array.
        /// </summary>
        private void GatherTargets()
        {
            for (int i = 0; i < _Targets.Length; i++)
                _Targets[i] = (IAnimancerComponent)targets[i];
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Cleans up this <see cref="UnityEditor.Editor"/>.
        /// </summary>
        protected virtual void OnDisable()
        {
            _AnimatorEditor.Dispose();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by the Unity editor to draw the custom inspector GUI elements.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Normally the targets wouldn't change after OnEnable, but the trick AnimancerComponent.Reset uses to
            // swap the type of an existing component when a new one is added causes the old target to be destroyed.
            GatherTargets();

            serializedObject.Update();

            _AnimatorEditor.DoInspectorGUI();
            DoOtherFieldsGUI();
            DoStatesGUI();

            serializedObject.ApplyModifiedProperties();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// If we have only one object selected and are in play mode, we need to constantly repaint to keep the
        /// inspector up to date with the latest details.
        /// </summary>
        public override bool RequiresConstantRepaint()
        {
            return _Targets.Length == 1 && EditorApplication.isPlaying;
        }

        /************************************************************************************************************************/

        private static readonly GUIContent Label = new GUIContent();

        /// <summary>
        /// Draws the rest of the inspector fields after the Animator field.
        /// </summary>
        protected void DoOtherFieldsGUI()
        {
            var property = _AnimatorEditor.AnimatorProperty.Copy();

            while (property.NextVisible(false))
            {
                Label.text = property.displayName;
                Label.tooltip = property.tooltip;

                // Let the target try to override.
                if (DoOverridePropertyGUI(property.propertyPath, property, Label))
                    continue;

                // Otherwise draw the property normally.
                EditorGUILayout.PropertyField(property, true);
            }
        }

        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Draws any custom GUI for the 'property'. The return value indicates whether the GUI should replace the
        /// regular call to <see cref="EditorGUILayout.PropertyField"/> or not.
        /// </summary>
        protected virtual bool DoOverridePropertyGUI(string name, SerializedProperty property, GUIContent label)
        {
            return false;
        }

        /************************************************************************************************************************/
        #region States
        /************************************************************************************************************************/

        private bool _IsGraphPlaying;

        /// <summary>
        /// Draws the state of each animation that is playing or has been played.
        /// </summary>
        protected void DoStatesGUI()
        {
            // If only multiple objects are selected and or the playable isn't initialised, don't draw anything.

            if (targets.Length != 1)
                return;

            var target = _Targets[0];
            if (!target.IsPlayableInitialised)
            {
                DoPlayableNotInitialisedGUI();
                return;
            }

            // Gather the during the layout event and use the same ones during subsequent events to avoid GUI errors
            // in case they change (they shouldn't, but this is also more efficient).
            if (Event.current.type == EventType.Layout)
            {
                AnimancerLayerDrawer.GatherLayerEditors(target.Playable, LayerInfos, out _LayerCount);
                _IsGraphPlaying = target.Playable.IsGraphPlaying;
            }

            if (!_IsGraphPlaying)
            {
                AnimancerEditorUtilities.BeginVerticalBox(GUI.skin.box);
                _IsGraphPlaying = EditorGUILayout.Toggle("Is Graph Playing", _IsGraphPlaying);
                AnimancerEditorUtilities.EndVerticalBox(GUI.skin.box);

                if (_IsGraphPlaying)
                    target.Playable.UnpauseGraph();
            }

            for (int i = 0; i < _LayerCount; i++)
            {
                LayerInfos[i].DoInspectorGUI(target);
            }

            DoLayerWeightWarningGUI();

            if (AnimancerLayerDrawer.ShowUpdatingNodes)
                target.Playable.DoUpdateListGUI();
        }

        /************************************************************************************************************************/

        private void DoPlayableNotInitialisedGUI()
        {
            if (!EditorApplication.isPlaying)
                return;

            var target = _Targets[0];
            if (EditorUtility.IsPersistent(target.gameObject))
                return;

            EditorGUILayout.HelpBox("Playable is not initialised." +
                " It will be initialised automatically when something needs it, such as playing an animation.",
                 MessageType.Info);

            var area = GUILayoutUtility.GetLastRect();
            AnimancerLayerDrawer.HandleDragAndDropAnimations(area, target, 0);

            if (!AnimancerEditorUtilities.TryUseContextClick(area))
                return;

            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Initialise"), false, () => target.Playable.GetLayer(0));

            AnimancerEditorUtilities.AddDocumentationLink(menu, "Layer Documentation", "/docs/manual/animation-layers");

            menu.ShowAsContext();
        }

        /************************************************************************************************************************/

        private void DoLayerWeightWarningGUI()
        {
            for (int i = 0; i < _LayerCount; i++)
            {
                var layer = LayerInfos[i].Layer;
                if (layer.Weight == 1 &&
                    !layer.IsAdditive &&
                    layer._Mask == null &&
                    Mathf.Approximately(layer.GetTotalWeight(), 1))
                    return;
            }

            EditorGUILayout.HelpBox(
                "There are no Override layers at weight 1, which will likely give undesirable results." +
                " Click here for more information.",
                MessageType.Warning);

            if (AnimancerEditorUtilities.TryUseClickInLastRect())
                EditorUtility.OpenWithDefaultApp(
                    AnimancerPlayable.APIDocumentationURL + "/docs/manual/smooth-transitions");
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}

#endif
