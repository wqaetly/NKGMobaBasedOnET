// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only] Draws the inspector GUI for an <see cref="AnimancerState"/>.</summary>
    public abstract class ParametizedAnimancerStateDrawer<T> : AnimancerStateDrawer<T> where T : AnimancerState
    {
        /************************************************************************************************************************/

        /// <summary>The number of parameters being managed by the target state.</summary>
        public virtual int ParameterCount { get { return 0; } }

        /// <summary>Returns the name of a parameter being managed by the target state.</summary>
        /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
        public virtual string GetParameterName(int index) { throw new NotSupportedException(); }

        /// <summary>Returns the type of a parameter being managed by the target state.</summary>
        /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
        public virtual AnimatorControllerParameterType GetParameterType(int index) { throw new NotSupportedException(); }

        /// <summary>Returns the value of a parameter being managed by the target state.</summary>
        /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
        public virtual object GetParameterValue(int index) { throw new NotSupportedException(); }

        /// <summary>Sets the value of a parameter being managed by the target state.</summary>
        /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
        public virtual void SetParameterValue(int index, object value) { throw new NotSupportedException(); }

        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="ParametizedAnimancerStateDrawer{T}"/> to manage the inspector GUI for the 'state'.
        /// </summary>
        public ParametizedAnimancerStateDrawer(T state) : base(state) { }

        /************************************************************************************************************************/

        /// <summary> Draws the details of the target state in the GUI.</summary>
        protected override void DoDetailsGUI(IAnimancerComponent owner)
        {
            base.DoDetailsGUI(owner);

            var animator = owner.Animator;
            if (animator == null)
                return;

            var count = ParameterCount;
            if (count <= 0)
                return;

            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth -= AnimancerEditorUtilities.IndentSize;

            var area = AnimancerEditorUtilities.GetRect(true);
            area = EditorGUI.IndentedRect(area);
            EditorGUI.LabelField(area, "Parameters", count.ToString());

            for (int i = 0; i < count; i++)
            {
                var type = GetParameterType(i);
                if (type == 0)
                    continue;

                var name = GetParameterName(i);
                var value = GetParameterValue(i);

                EditorGUI.BeginChangeCheck();

                area = AnimancerEditorUtilities.GetRect(true);
                area = EditorGUI.IndentedRect(area);

                switch (type)
                {
                    case AnimatorControllerParameterType.Float:
                        value = EditorGUI.FloatField(area, name, (float)value);
                        break;

                    case AnimatorControllerParameterType.Int:
                        value = EditorGUI.IntField(area, name, (int)value);
                        break;

                    case AnimatorControllerParameterType.Bool:
                        value = EditorGUI.Toggle(area, name, (bool)value);
                        break;

                    case AnimatorControllerParameterType.Trigger:
                        value = EditorGUI.Toggle(area, name, (bool)value, EditorStyles.radioButton);
                        break;

                    default:
                        EditorGUI.LabelField(area, name, "Unhandled Type: " + type);
                        break;
                }

                if (EditorGUI.EndChangeCheck())
                    SetParameterValue(i, value);
            }

            EditorGUIUtility.labelWidth = labelWidth;
        }

        /************************************************************************************************************************/
    }
}

#endif
