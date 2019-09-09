// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only]
    /// A custom inspector for <see cref="ScriptableObject"/>s which adds a message explaining that changes in play
    /// mode will persist.
    /// </summary>
    [CustomEditor(typeof(ScriptableObject), true, isFallback = true), CanEditMultipleObjects]
    public class ScriptableObjectEditor : UnityEditor.Editor
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Called by the Unity editor to draw the custom inspector GUI elements.
        /// <para></para>
        /// Draws the regular inspector then adds a message explaining that changes in play mode will persist.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (EditorUtility.IsPersistent(target))
            {
                EditorGUILayout.HelpBox("This is an asset, not a scene object," +
                    " which means that any changes you make to it are permanent" +
                    " and will NOT be undone when you exit Play Mode.", MessageType.Info);
            }
        }

        /************************************************************************************************************************/
    }
}

#endif
