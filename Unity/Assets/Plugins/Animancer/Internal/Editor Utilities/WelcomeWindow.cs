// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only] [Internal]
    /// An introductory <see cref="EditorWindow"/> for when you first import a new version of Animancer.
    /// </summary>
    internal sealed class WelcomeWindow : EditorWindow
    {
        /************************************************************************************************************************/

        /// <summary>
        /// The release ID of this Animancer version.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        ///   <item>[1] = v1.0: 2018-05-02.</item>
        ///   <item>[2] = v1.1: 2018-05-29.</item>
        ///   <item>[3] = v1.2: 2018-08-14.</item>
        ///   <item>[4] = v1.3: 2018-09-12.</item>
        ///   <item>[5] = v2.0: 2018-10-08.</item>
        ///   <item>[6] = v3.0: 2019-05-27.</item>
        /// </list>
        /// </remarks>
        private const int ReleaseNumber = 6;

        /// <summary>The display name of this Animancer version.</summary>
        private const string VersionName = "v3.0";

        /// <summary>The end of the URL for the change log of this Animancer version.</summary>
        private const string ChangeLogSuffix = "v3-0";

        /************************************************************************************************************************/

        /// <summary>The key used to save the release number.</summary>
        private const string PrefKey = "Animancer.ReleaseNumber";

        /// <summary>
        /// A temporary file to ensure that this window only gets shown once instead of every time scripts are
        /// recompiled, even if the user hasn't selected "Don't show this again" yet.
        /// </summary>
        private const string LockFile = "Temp/AnimancerWelcome";

        /// <summary>
        /// The asset path of the Animancer folder.
        /// </summary>
        private const string AnimancerDirectory = "Assets/Plugins/Animancer";

        /// <summary>
        /// The asset path of the first example scene.
        /// </summary>
        private const string ExampleScenePath = AnimancerDirectory + "/Examples/01 Basics/01 Simple Playing/Simple Playing.unity";

        /************************************************************************************************************************/

        [SerializeField]
        private Vector2 _ScrollPosition;

        [NonSerialized]
        private int _PreviousVersion;

        /************************************************************************************************************************/

        /// <summary>
        /// Called after Unity reloads assemblies (such as on startup and when a script is modified).
        /// Checks if the Animancer version has changed and hasn't yet been shown. If so, this method opens it.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Initialise()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode ||
                PlayerPrefs.GetInt(PrefKey, -1) >= ReleaseNumber ||
                File.Exists(LockFile))
                return;

            EditorApplication.delayCall += () =>
            {
                File.WriteAllText(LockFile, "");

                GetWindow<WelcomeWindow>();
            };
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity when this component becomes enabled and active.
        /// Configures the window title and size.
        /// </summary>
        private void OnEnable()
        {
            titleContent = new GUIContent("Animancer");
            minSize = new Vector2(500, 300);

            _PreviousVersion = PlayerPrefs.GetInt(PrefKey, -1);
            if (_PreviousVersion < 0)
                _PreviousVersion = EditorPrefs.GetInt(PrefKey, -1);// Animancer v2.0 used EditorPrefs.
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity to draw the GUI for this window and respond to input events.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Label("Welcome to", Styles.CenteredLabel);
            GUILayout.Label("Animancer " + VersionName, Styles.Headding);

            _ScrollPosition = GUILayout.BeginScrollView(_ScrollPosition);

            if (_PreviousVersion >= 0 && _PreviousVersion < ReleaseNumber)
            {
                if (_PreviousVersion < 4)// Upgraded from before v2.0.
                {
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);

                    EditorGUILayout.HelpBox("It seems you have just upgraded from an earlier version of Animancer (before v2.0)" +
                        " so you will need to restart Unity before you can use it.",
                        MessageType.Warning);
                }

                // Upgraded from any older version.
                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                EditorGUILayout.HelpBox(
                    "You must fully delete any old version of Animancer before importing a new version." +
                    " You can ignore this message if you have already done so." +
                    " Otherwise click here to delete '" + AnimancerDirectory + "' then import Animancer again.",
                    MessageType.Warning);
                CheckDeleteAnimancer();
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            if (GUILayout.Button("Go to first Example Scene"))
            {
                EditorSceneManager.OpenScene(ExampleScenePath);

                var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(ExampleScenePath);

                Selection.activeObject = null;
                EditorApplication.delayCall += () =>
                    EditorApplication.delayCall += () =>
                        Selection.activeObject = scene;
            }

            if (GUILayout.Button("Documentation: " + AnimancerPlayable.DocumentationURL))
                EditorUtility.OpenWithDefaultApp(AnimancerPlayable.DocumentationURL);

            if (GUILayout.Button("Change Log: " + VersionName))
                EditorUtility.OpenWithDefaultApp(AnimancerPlayable.DocumentationURL + "/docs/changes/animancer-" + ChangeLogSuffix);

            if (GUILayout.Button("Support: " + AnimancerComponentEditor.DeveloperEmail))
                AnimancerComponentEditor.EmailTheDeveloper();

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            if (GUILayout.Button("Close and don't show this again"))
            {
                PlayerPrefs.SetInt(PrefKey, ReleaseNumber);
                Close();
            }

            GUILayout.EndScrollView();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Asks if the user wants to delete the root Animancer folder and does so if they confirm.
        /// </summary>
        private void CheckDeleteAnimancer()
        {
            if (!AnimancerEditorUtilities.TryUseClickInLastRect())
                return;

            if (!AssetDatabase.IsValidFolder(AnimancerDirectory))
            {
                Debug.Log(AnimancerDirectory + " doesn't exist." +
                    " You must have moved Animancer somewhere else so you will need to delete it manually.");
                return;
            }

            if (!EditorUtility.DisplayDialog("Delete Animancer?",
                "Would you like to delete " + AnimancerDirectory + "?" +
                "\n\nYou will then need to reimport Animancer manually.",
                "Delete", "Cancel"))
                return;

            AssetDatabase.DeleteAsset(AnimancerDirectory);
            Close();
            File.Delete(LockFile);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// <see cref="GUIStyle"/>s used by this window. They are located in a nested class so they don't get
        /// initialised before they are referenced (because they can't be until the first <see cref="OnGUI"/> call).
        /// </summary>
        private static class Styles
        {
            /// <summary>
            /// The standard <see cref="GUISkin.label"/> with the alignment centered.
            /// </summary>
            public static readonly GUIStyle CenteredLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
            };

            /// <summary>
            /// The standard <see cref="GUISkin.label"/> with the alignment centered and a larger size.
            /// </summary>
            public static readonly GUIStyle Headding = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 26,
            };
        }

        /************************************************************************************************************************/
    }
}

#endif
