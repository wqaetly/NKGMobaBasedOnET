// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only]
    /// A custom inspector for <see cref="NamedAnimancerComponent"/>s.
    /// </summary>
    [CustomEditor(typeof(NamedAnimancerComponent), true), CanEditMultipleObjects]
    public class NamedAnimancerComponentEditor : AnimancerComponentEditor
    {
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Draws any custom GUI for the 'property'. The return value indicates whether the GUI should replace the
        /// regular call to <see cref="EditorGUILayout.PropertyField"/> or not.
        /// </summary>
        protected override bool DoOverridePropertyGUI(string name, SerializedProperty property, GUIContent label)
        {
            switch (name)
            {
                case "_PlayAutomatically":
                    return !ShouldShowAnimationFields();

                case "_Animations":
                    if (ShouldShowAnimationFields())
                    {
                        DoDefaultAnimationField(property);
                        DoAnimationsField(property, label);
                    }
                    return true;

                default:
                    return base.DoOverridePropertyGUI(name, property, label);
            }
        }

        /************************************************************************************************************************/

        private bool ShouldShowAnimationFields()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
                return true;

            for (int i = 0; i < Targets.Length; i++)
            {
                if (!Targets[i].enabled)
                    return true;
            }

            return false;
        }

        /************************************************************************************************************************/

        private void DoDefaultAnimationField(SerializedProperty property)
        {
            var area = AnimancerEditorUtilities.GetRect();

            var label = AnimancerEditorUtilities.TempContent("Default Animation",
                "If 'Play Automatically' is enabled, this animation will be played by OnEnable");

            SerializedProperty firstElement;
            AnimationClip clip;

            if (property.arraySize > 0)
            {
                firstElement = property.GetArrayElementAtIndex(0);
                clip = (AnimationClip)firstElement.objectReferenceValue;
                label = EditorGUI.BeginProperty(area, label, firstElement);
            }
            else
            {
                firstElement = null;
                clip = null;
                label = EditorGUI.BeginProperty(area, label, property);
            }

            EditorGUI.BeginChangeCheck();

            clip = (AnimationClip)EditorGUI.ObjectField(area, label, clip, typeof(AnimationClip), true);

            if (EditorGUI.EndChangeCheck())
            {
                if (clip != null)
                {
                    if (firstElement == null)
                    {
                        property.arraySize = 1;
                        firstElement = property.GetArrayElementAtIndex(0);
                    }

                    firstElement.objectReferenceValue = clip;
                }
                else
                {
                    if (firstElement == null || property.arraySize == 1)
                        property.arraySize = 0;
                    else
                        firstElement.objectReferenceValue = clip;
                }
            }

            EditorGUI.EndProperty();

        }

        /************************************************************************************************************************/

        private ReorderableList _Animations;
        private static int _RemoveAnimationIndex;

        private void DoAnimationsField(SerializedProperty property, GUIContent label)
        {
            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing - 1);

            if (_Animations == null)
            {
                _Animations = new ReorderableList(property.serializedObject, property.Copy())
                {
                    drawHeaderCallback = DrawAnimationsHeader,
                    drawElementCallback = DrawAnimationElement,
                    elementHeight = EditorGUIUtility.singleLineHeight,
                    onRemoveCallback = RemoveSelectedElement,
                };
            }

            _RemoveAnimationIndex = -1;

            GUILayout.BeginVertical();
            _Animations.DoLayoutList();
            GUILayout.EndVertical();

            if (_RemoveAnimationIndex >= 0)
            {
                property.DeleteArrayElementAtIndex(_RemoveAnimationIndex);
            }

            AnimancerEditorUtilities.HandleDragAndDropAnimations(GUILayoutUtility.GetLastRect(), (clip) =>
            {
                var index = property.arraySize;
                property.arraySize = index + 1;
                var element = property.GetArrayElementAtIndex(index);
                element.objectReferenceValue = clip;
            });

            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
        }

        /************************************************************************************************************************/

        private SerializedProperty _AnimationsArraySize;

        private void DrawAnimationsHeader(Rect area)
        {
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth -= 6;

            area.width += 5;

            var property = _Animations.serializedProperty;
            var label = AnimancerEditorUtilities.TempContent(property.displayName, property.tooltip);
            EditorGUI.BeginProperty(area, label, property);

            if (_AnimationsArraySize == null)
            {
                _AnimationsArraySize = property.Copy();
                _AnimationsArraySize.Next(true);
                _AnimationsArraySize.Next(true);
            }

            EditorGUI.PropertyField(area, _AnimationsArraySize, label);

            EditorGUI.EndProperty();

            EditorGUIUtility.labelWidth = labelWidth;
        }

        /************************************************************************************************************************/

        private static readonly HashSet<Object>
            PreviousAnimations = new HashSet<Object>();

        private void DrawAnimationElement(Rect area, int index, bool isActive, bool isFocused)
        {
            if (index < PreviousAnimations.Count)
                PreviousAnimations.Clear();

            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth -= 20;

            var element = _Animations.serializedProperty.GetArrayElementAtIndex(index);

            var color = GUI.color;
            var animation = element.objectReferenceValue;
            if (PreviousAnimations.Contains(animation))
                GUI.color = AnimancerEditorUtilities.WarningFieldColor;
            else
                PreviousAnimations.Add(animation);

            EditorGUI.BeginChangeCheck();
            EditorGUI.ObjectField(area, element, GUIContent.none);

            if (EditorGUI.EndChangeCheck() && element.objectReferenceValue == null)
                _RemoveAnimationIndex = index;

            GUI.color = color;
            EditorGUIUtility.labelWidth = labelWidth;
        }

        /************************************************************************************************************************/

        private static void RemoveSelectedElement(ReorderableList list)
        {
            var property = list.serializedProperty;
            var element = property.GetArrayElementAtIndex(list.index);

            // Deleting a non-null element sets it to null, so we make sure it's null to actually remove it.
            if (element.objectReferenceValue != null)
                element.objectReferenceValue = null;

            property.DeleteArrayElementAtIndex(list.index);

            if (list.index >= property.arraySize - 1)
                list.index = property.arraySize - 1;
        }

        /************************************************************************************************************************/
        #region Menu Items
        /************************************************************************************************************************/

        [MenuItem("CONTEXT/NamedAnimancerComponent/Play Animations in Sequence", validate = true)]
        [MenuItem("CONTEXT/NamedAnimancerComponent/Cross Fade Animations in Sequence", validate = true)]
        private static bool IsPlaying()
        {
            return EditorApplication.isPlaying;
        }

        /************************************************************************************************************************/

        /// <summary>Starts <see cref="NamedAnimancerComponent.PlayAnimationsInSequence"/> as a coroutine.</summary>
        [MenuItem("CONTEXT/NamedAnimancerComponent/Play Animations in Sequence", priority = MenuItemPriority)]
        private static void PlayAnimationsInSequence(MenuCommand command)
        {
            var animancer = command.context as NamedAnimancerComponent;
            animancer.StartCoroutine(animancer.PlayAnimationsInSequence());
        }

        /// <summary>Starts <see cref="NamedAnimancerComponent.CrossFadeAnimationsInSequence"/> as a coroutine.</summary>
        [MenuItem("CONTEXT/NamedAnimancerComponent/Cross Fade Animations in Sequence", priority = MenuItemPriority)]
        private static void CrossFadeAnimationsInSequence(MenuCommand command)
        {
            var animancer = command.context as NamedAnimancerComponent;
            animancer.StartCoroutine(animancer.CrossFadeAnimationsInSequence());
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}

#endif
