// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

using System.Text;
using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only]
    /// A custom inspector for <see cref="AnimancerComponent"/>s.
    /// </summary>
    [CustomEditor(typeof(AnimancerComponent), true), CanEditMultipleObjects]
    public class AnimancerComponentEditor : AnimancerPlayableEditor
    {
        /************************************************************************************************************************/

        /// <summary>The priority of all context menu items added by this class.</summary>
        protected const int MenuItemPriority = 2000;

        /************************************************************************************************************************/

        /// <summary>Returns <see cref="AnimancerPlayable.IsGraphPlaying"/>.</summary>
        [MenuItem("CONTEXT/AnimancerComponent/Pause Graph", validate = true)]
        private static bool ValidatePauseGraph(MenuCommand command)
        {
            var animancer = command.context as AnimancerComponent;
            return
                animancer.IsPlayableInitialised &&
                animancer.Playable.IsGraphPlaying;
        }

        /// <summary>Calls <see cref="AnimancerPlayable.PauseGraph()"/>.</summary>
        [MenuItem("CONTEXT/AnimancerComponent/Pause Graph", priority = MenuItemPriority)]
        private static void PauseGraph(MenuCommand command)
        {
            var animancer = command.context as AnimancerComponent;
            animancer.Playable.PauseGraph();
        }

        /************************************************************************************************************************/

        /// <summary>Returns !<see cref="AnimancerPlayable.IsGraphPlaying"/>.</summary>
        [MenuItem("CONTEXT/AnimancerComponent/Unpause Graph", validate = true)]
        private static bool ValidatePlayGraph(MenuCommand command)
        {
            var animancer = command.context as AnimancerComponent;
            return
                animancer.IsPlayableInitialised &&
                !animancer.Playable.IsGraphPlaying;
        }

        /// <summary>Calls <see cref="AnimancerPlayable.UnpauseGraph()"/>.</summary>
        [MenuItem("CONTEXT/AnimancerComponent/Unpause Graph", priority = MenuItemPriority)]
        private static void PlayGraph(MenuCommand command)
        {
            var animancer = command.context as AnimancerComponent;
            animancer.Playable.UnpauseGraph();
            animancer.Evaluate();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns <see cref="AnimancerComponent.IsPlayableInitialised"/>.
        /// </summary>
        [MenuItem("CONTEXT/AnimancerComponent/Stop All Animations", validate = true)]
        private static bool ValidateStop(MenuCommand command)
        {
            var animancer = command.context as AnimancerComponent;
            return animancer.IsPlayableInitialised;
        }

        /// <summary>Calls <see cref="AnimancerComponent.Stop()"/>.</summary>
        [MenuItem("CONTEXT/AnimancerComponent/Stop All Animations", priority = MenuItemPriority)]
        private static void Stop(MenuCommand command)
        {
            var animancer = command.context as AnimancerComponent;
            animancer.Stop();
            animancer.Evaluate();
        }

        /************************************************************************************************************************/

        /// <summary>Logs a description of all states currently in the <see cref="AnimancerComponent.Playable"/>.</summary>
        [MenuItem("CONTEXT/AnimancerComponent/Log Description of States", priority = MenuItemPriority)]
        private static void LogDescriptionOfStates(MenuCommand command)
        {
            var animancer = command.context as AnimancerComponent;
            var message = new StringBuilder();
            message.Append(animancer.ToString());
            if (animancer.IsPlayableInitialised)
            {
                message.Append(":\n");
                animancer.Playable.AppendDescription(message);
            }
            else
            {
                message.Append(": Playable is not initialised.");
            }

            AnimancerEditorUtilities.AppendNonCriticalIssues(message);

            Debug.Log(message, animancer);
        }

        /************************************************************************************************************************/

        /// <summary>Opens the Animancer Documentation website.</summary>
        [MenuItem("CONTEXT/AnimancerComponent/Animancer Documentation", priority = MenuItemPriority)]
        private static void OpenUserManual(MenuCommand command)
        {
            EditorUtility.OpenWithDefaultApp(AnimancerPlayable.APIDocumentationURL);
        }

        /************************************************************************************************************************/

        /// <summary>The email address which handles support for Animancer.</summary>
        public const string DeveloperEmail = "AnimancerUnityPlugin@gmail.com";

        /// <summary>Opens an email to "AnimancerUnityPlugin@gmail.com" and copies that string to the clipboard.</summary>
        [MenuItem("CONTEXT/AnimancerComponent/Email Support (" + DeveloperEmail + ")", priority = MenuItemPriority)]
        public static void EmailTheDeveloper()
        {
            EditorGUIUtility.systemCopyBuffer = DeveloperEmail;
            Debug.Log("Copied '" + DeveloperEmail + "' to clipboard.");
            Application.OpenURL("mailto:" + DeveloperEmail + "?subject=Animancer");
        }

        /************************************************************************************************************************/
    }
}

#endif
