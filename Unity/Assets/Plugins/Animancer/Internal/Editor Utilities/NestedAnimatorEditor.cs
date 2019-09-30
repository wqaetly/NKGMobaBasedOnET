// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only]
    /// A custom inspector for drawing the <see cref="IAnimancerComponent.Animator"/> nested inside its own inspector.
    /// </summary>
    public sealed class NestedAnimatorEditor : IDisposable
    {
        /************************************************************************************************************************/

        /// <summary>The <see cref="IAnimancerComponent"/> objects being edited.</summary>
        public readonly IAnimancerComponent[] Targets;

        /// <summary>The property that serializes the <see cref="IAnimancerComponent.Animator"/>.</summary>
        public readonly SerializedProperty AnimatorProperty;

        /************************************************************************************************************************/

        /// <summary>The animator referenced by each target.</summary>
        private Animator[] _Animators;

        /// <summary>
        /// Indicates whether all <see cref="_Animators"/> are on the same <see cref="GameObject"/> as the target that
        /// is referencing them.
        /// </summary>
        private bool _IsAnimatorOnSameObject;

        /// <summary>A <see cref="SerializedObject"/> encapsulating the <see cref="_Animators"/>.</summary>
        private SerializedObject _SerializedAnimator;

        /// <summary>A <see cref="SerializedProperty"/> of the <see cref="_SerializedAnimator"/>.</summary>
        private SerializedProperty _Controller, _Avatar, _RootMotion, _UpdateMode, _CullingMode, _KeepStateOnDisable;

        /************************************************************************************************************************/

        /// <summary>
        /// Creates a new <see cref="NestedAnimatorEditor"/> to wrap the 'animatorProperty'.
        /// </summary>
        public NestedAnimatorEditor(IAnimancerComponent[] targets, SerializedProperty animatorProperty)
        {
            Targets = targets;
            AnimatorProperty = animatorProperty;
            GatherAnimatorProperties();

            // We need to enforce the expanded state from the Animator property because the inspector needs to be
            // expanded by Dispose in case the AnimancerComponent is removed (we don't want to leave the Animator
            // collapsed when that happens).
            AnimancerEditorUtilities.SetIsInspectorExpanded(_Animators, animatorProperty.isExpanded);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Cleans up this editor.
        /// </summary>
        public void Dispose()
        {
            AnimancerEditorUtilities.SetIsInspectorExpanded(_Animators, true);

            if (_SerializedAnimator != null)
            {
                _SerializedAnimator.Dispose();
                _SerializedAnimator = null;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Ensures that the <see cref="SerializedObject"/> and properties for the <see cref="_Animators"/> have been
        /// initialised.
        /// </summary>
        private void GatherAnimatorProperties()
        {
            if (!GatherAnimators())
                return;

            if (_SerializedAnimator != null)
                _SerializedAnimator.Dispose();

            // If any of the animators are missing, clear the properties.
            for (int i = 0; i < _Animators.Length; i++)
            {
                if (_Animators[i] == null)
                {
                    _SerializedAnimator = null;
                    _Controller = null;
                    _Avatar = null;
                    _RootMotion = null;
                    _UpdateMode = null;
                    _CullingMode = null;
                    _KeepStateOnDisable = null;
                    return;
                }
            }

            // Otherwise get their properties.
            _SerializedAnimator = new SerializedObject(_Animators);
            _Controller = _SerializedAnimator.FindProperty("m_Controller");
            _Avatar = _SerializedAnimator.FindProperty("m_Avatar");
            _RootMotion = _SerializedAnimator.FindProperty("m_ApplyRootMotion");
            _UpdateMode = _SerializedAnimator.FindProperty("m_UpdateMode");
            _CullingMode = _SerializedAnimator.FindProperty("m_CullingMode");
            _KeepStateOnDisable = _SerializedAnimator.FindProperty("m_KeepAnimatorControllerStateOnDisable");
        }

        /************************************************************************************************************************/

        /// <summary>Gathers the <see cref="_Animators"/> from the <see cref="_Targets"/>.</summary>
        private bool GatherAnimators()
        {
            if (_Animators == null || _Animators.Length != Targets.Length)
                _Animators = new Animator[Targets.Length];

            _IsAnimatorOnSameObject = true;
            bool hasChanged = false;

            for (int i = 0; i < _Animators.Length; i++)
            {
                var target = Targets[i];
                var animator = target.Animator;

                if (animator != null && target.gameObject != animator.gameObject)
                    _IsAnimatorOnSameObject = false;

                if (_Animators[i] != animator)
                {
                    AnimancerEditorUtilities.SetIsInspectorExpanded(_Animators[i], true);
                    _Animators[i] = animator;
                    hasChanged = true;
                }
            }

            return hasChanged;
        }

        /************************************************************************************************************************/

        private static Action _OnEndGUI;

        /// <summary>
        /// Draws the animator reference field followed by its fields that are relevant to Animancer.
        /// </summary>
        public void DoInspectorGUI()
        {
            _OnEndGUI = null;

            DoAnimatorGUI();

            GatherAnimatorProperties();

            if (_SerializedAnimator == null)
                return;

            _SerializedAnimator.Update();

            AnimancerEditorUtilities.BeginVerticalBox(EditorStyles.helpBox);
            {
                if (!_IsAnimatorOnSameObject)
                {
                    EditorGUILayout.HelpBox("It is recommended that you keep this component on the same GameObject" +
                        " as its target Animator so that they get enabled and disabled at the same time.",
                        MessageType.Info);
                }

                DoControllerGUI();
                EditorGUILayout.PropertyField(_Avatar, AnimancerEditorUtilities.TempContent("Avatar", "The Avatar used by the Animator"));
                DoRootMotionGUI();
                DoUpdateModeGUI(true);
                DoCullingModeGUI();
                DoStopOnDisableGUI(_KeepStateOnDisable, false);
            }
            AnimancerEditorUtilities.EndVerticalBox(EditorStyles.helpBox);

            _SerializedAnimator.ApplyModifiedProperties();

            if (_OnEndGUI != null)
            {
                _OnEndGUI();
                _OnEndGUI = null;
            }
        }

        /************************************************************************************************************************/

        private void DoAnimatorGUI()
        {
            var hasAnimator = AnimatorProperty.objectReferenceValue != null;

            var color = GUI.color;
            if (!hasAnimator)
                GUI.color = AnimancerEditorUtilities.WarningFieldColor;

            EditorGUILayout.PropertyField(AnimatorProperty);

            if (!hasAnimator)
            {
                GUI.color = color;

                EditorGUILayout.HelpBox("An Animator is required in order to play animations." +
                    " Click here to search for one nearby.",
                    MessageType.Warning);

                if (AnimancerEditorUtilities.TryUseClickInLastRect())
                {
                    AnimancerEditorUtilities.ForEachTarget(AnimatorProperty, (property) =>
                    {
                        var target = (IAnimancerComponent)property.serializedObject.targetObject;

                        // Can't use AnimancerEditorUtilities.GetComponentInHierarchy
                        // because Animancer Lite leaves it outside the DLL.
                        var animator = target.gameObject.GetComponentInParent<Animator>();
                        if (animator == null)
                        {
                            animator = target.gameObject.GetComponentInChildren<Animator>();
                            if (animator == null)
                            {
                                Debug.Log("No Animator found on '" + target + "' or any of its parents or children." +
                                    " You must assign one manually.", target.gameObject);
                                return;
                            }
                        }

                        property.objectReferenceValue = animator;
                    });
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Draws the <see cref="Animator.runtimeAnimatorController"/> field with a warning if a controller is
        /// assigned.
        /// </summary>
        private void DoControllerGUI()
        {
            if (_Controller == null)
                return;

            var controller = _Animators[0].runtimeAnimatorController;

            var showMixedValue = EditorGUI.showMixedValue;
            for (int i = 1; i < _Animators.Length; i++)
            {
                if (_Animators[i].runtimeAnimatorController != controller)
                {
                    EditorGUI.showMixedValue = true;
                    break;
                }
            }

            if (controller == null && !EditorGUI.showMixedValue)
                return;

            var label = AnimancerEditorUtilities.TempContent("Controller");

            EditorGUI.BeginChangeCheck();

            var area = EditorGUILayout.BeginHorizontal();
            label = EditorGUI.BeginProperty(area, label, _Controller);

            var color = GUI.color;
            GUI.color = AnimancerEditorUtilities.WarningFieldColor;

            controller = (RuntimeAnimatorController)EditorGUILayout.ObjectField(label, controller,
                typeof(RuntimeAnimatorController), false);

            GUI.color = color;

            if (GUILayout.Button("Remove", EditorStyles.miniButton, AnimancerEditorUtilities.DontExpandWidth))
                controller = null;

            GUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(_Animators, "Changed AnimatorController");
                for (int i = 0; i < _Animators.Length; i++)
                {
                    _Animators[i].runtimeAnimatorController = controller;
                }
                OnControllerChanged();
            }

            EditorGUI.showMixedValue = showMixedValue;

            EditorGUILayout.HelpBox(
                "The AnimatorController will not affect the model while Animancer is active," +
                " however Unity will still execute it's state machine in the background," +
                " which may be a waste of processing time if you aren't using it intentionally." +
                " Click here for more information.", MessageType.Warning);

            if (AnimancerEditorUtilities.TryUseClickInLastRect())
                EditorUtility.OpenWithDefaultApp(AnimancerPlayable.APIDocumentationURL + "/docs/manual/animator-controller-states");
        }

        /************************************************************************************************************************/

        private static Action _OnControllerChanged;

        private static void OnControllerChanged()
        {
            if (_OnControllerChanged == null)
            {
                const string TypeName = "UnityEditorInternal.AnimationWindowUtility";
                const string MethodName = "ControllerChanged";

                try
                {
                    var type = typeof(UnityEditorInternal.InternalEditorUtility);
                    type = type.Assembly.GetType(TypeName);
                    var method = type.GetMethod(MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    _OnControllerChanged = (Action)Delegate.CreateDelegate(typeof(Action), method);
                }
                catch
                {
                    AnimancerEditorUtilities.RegisterNonCriticalMissingMember(TypeName, MethodName);
                    _OnControllerChanged = () => { };
                }
            }

            _OnControllerChanged();
        }

        /************************************************************************************************************************/

        private bool _IsRootPositionOrRotationControlledByCurves;

        private void DoRootMotionGUI()
        {
            if (_RootMotion == null)
                return;

            var animator = _Animators[0];
            if (_Animators.Length == 1 && (bool)AnimancerEditorUtilities.Invoke(animator, "get_supportsOnAnimatorMove"))
            {
                EditorGUILayout.LabelField("Apply Root Motion", "Handled by Script");
            }
            else
            {
                EditorGUILayout.PropertyField(_RootMotion, AnimancerEditorUtilities.TempContent("Apply Root Motion",
                    "If enabled, the Animator will automatically move the object using the root motion from the animations"));

                if (Event.current.type == EventType.Layout)
                    _IsRootPositionOrRotationControlledByCurves =
                        (bool)AnimancerEditorUtilities.Invoke(animator, "get_isRootPositionOrRotationControlledByCurves");

                if (_IsRootPositionOrRotationControlledByCurves && !_RootMotion.boolValue)
                    EditorGUILayout.HelpBox("Root position or rotation are controlled by curves", MessageType.Info, true);
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Draws the <see cref="Animator.updateMode"/> field with any appropriate warnings.
        /// </summary>
        private void DoUpdateModeGUI(bool showWithoutWarning)
        {
            if (_UpdateMode == null)
                return;

            var label = AnimancerEditorUtilities.TempContent("Update Mode",
                "Controls when and how often the animations are updated");

            var initialUpdateMode = Targets[0].InitialUpdateMode;
            var updateMode = (AnimatorUpdateMode)_UpdateMode.intValue;

            EditorGUI.BeginChangeCheck();

            if (!EditorApplication.isPlaying || !AnimancerPlayable.HasChangedToOrFromAnimatePhysics(initialUpdateMode, updateMode))
            {
                if (showWithoutWarning)
                    EditorGUILayout.PropertyField(_UpdateMode, label);
            }
            else
            {
                GUILayout.BeginHorizontal();

                var color = GUI.color;
                GUI.color = AnimancerEditorUtilities.WarningFieldColor;
                EditorGUILayout.PropertyField(_UpdateMode, label);
                GUI.color = color;

                label = AnimancerEditorUtilities.TempContent("Revert", "Revert to initial mode");
                if (GUILayout.Button(label, EditorStyles.miniButton, AnimancerEditorUtilities.DontExpandWidth))
                    _UpdateMode.intValue = (int)initialUpdateMode.Value;

                GUILayout.EndHorizontal();

                EditorGUILayout.HelpBox(
                    "Changing to or from AnimatePhysics mode at runtime has no effect when using the" +
                    " Playables API. It will continue using the original mode it had on startup.",
                    MessageType.Warning);

                if (AnimancerEditorUtilities.TryUseClickInLastRect())
                    EditorUtility.OpenWithDefaultApp(AnimancerPlayable.APIDocumentationURL + "/docs/unity-bugs/update-modes");
            }

            if (EditorGUI.EndChangeCheck())
            {
                _OnEndGUI += () =>
                {
                    for (int i = 0; i < _Animators.Length; i++)
                    {
                        var animator = _Animators[i];
                        if (animator != null)
                            AnimancerEditorUtilities.Invoke(animator, "OnUpdateModeChanged");
                    }
                };
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Draws the <see cref="Animator.cullingMode"/> field.
        /// </summary>
        private void DoCullingModeGUI()
        {
            if (_CullingMode == null)
                return;

            var label = AnimancerEditorUtilities.TempContent("Culling Mode",
                "Controls what is updated when the object has been culled (when it is not being rendered by a Camera)");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_CullingMode, label);

            if (EditorGUI.EndChangeCheck())
            {
                _OnEndGUI += () =>
                {
                    for (int i = 0; i < Targets.Length; i++)
                    {
                        var animator = _Animators[i];
                        if (animator != null)
                            AnimancerEditorUtilities.Invoke(animator, "OnCullingModeChanged");
                    }
                };
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Draws the <see cref="Animator.keepAnimatorControllerStateOnDisable"/> field.
        /// </summary>
        public static void DoStopOnDisableGUI(SerializedProperty keepStateOnDisable, bool updateAndApply)
        {
#if UNITY_2018_1_OR_NEWER
            var area = AnimancerEditorUtilities.GetRect();

            var label = AnimancerEditorUtilities.TempContent("Stop On Disable",
                "If true, disabling this object will stop and rewind all animations." +
                " Otherwise they will simply be paused and will resume from their current states when it is re-enabled.");

            if (keepStateOnDisable != null)
            {
                if (updateAndApply)
                    keepStateOnDisable.serializedObject.Update();

                label = EditorGUI.BeginProperty(area, label, keepStateOnDisable);

                keepStateOnDisable.boolValue = !EditorGUI.Toggle(area, label, !keepStateOnDisable.boolValue);

                EditorGUI.EndProperty();

                if (updateAndApply)
                    keepStateOnDisable.serializedObject.ApplyModifiedProperties();
            }
            else
            {
                var enabled = GUI.enabled;
                GUI.enabled = false;
                EditorGUI.Toggle(area, label, false);
                GUI.enabled = enabled;
            }
#endif
        }

        /************************************************************************************************************************/
    }
}

#endif
