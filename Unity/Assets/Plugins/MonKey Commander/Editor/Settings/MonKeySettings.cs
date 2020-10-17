using MonKey.Editor.Internal;
using MonKey.Extensions;
using MonKey.Internal;
using MonKey.Settings.Internal;
using UnityEditor;
using UnityEngine;

public class MonKeySettings : Editor
{
    public static readonly string defaultMonKeyInstallFolder = "Assets/Plugins/MonKey Commander/Editor";

    public static MonKeySettings Instance
    {
        get
        {
            return !instance ? InitSettings() : instance;
        }
    }

    private static MonKeySettings instance;

    public static MonKeySettings InitSettings()
    {
        string[] settingsPaths = AssetDatabase.FindAssets("t:MonKeySettings");
        if (settingsPaths.Length == 0)
        {
            return CreateNewInstance();
        }

        if (settingsPaths.Length > 1)
        {
            Debug.LogWarning(
                "MonKey Warning: More than one MonKey Settings were found: this is not allowed, please leave only one");
        }

        instance = AssetDatabase.LoadAssetAtPath<MonKeySettings>(
            AssetDatabase.GUIDToAssetPath(settingsPaths[0]));

        if (!instance)
        {
            AssetDatabase.DeleteAsset(defaultMonKeyInstallFolder + "/Settings/MonKey Settings.asset");
            return CreateNewInstance();
        }

        SavePrefs();

        CommandManager.FindInstance();
        return instance;
    }

    private static MonKeySettings CreateNewInstance()
    {
        if (!AssetDatabase.IsValidFolder(defaultMonKeyInstallFolder))
            AssetDatabase.CreateFolder("Assets", "/Plugins/MonKey Commander/Editor/Settings");

        instance = CreateInstance<MonKeySettings>();

        AssetDatabase.CreateAsset(instance, defaultMonKeyInstallFolder + "/Settings/MonKey Settings.asset");
        AssetDatabase.SaveAssets();
        SavePrefs();
        return instance;
    }


#if UNITY_2018_3_OR_NEWER

    private class SettingsWindow : EditorWindow
    {

        [MenuItem("Tools/MonKey Commander/Settings")]
        public static void Settings()
        {
            SettingsWindow window = GetWindow<SettingsWindow>();
            window.Show();
        }

        private void OnGUI()
        {
            Instance.titleStyle = new GUIStyle()
            {
                richText = true,
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(0, 0, 5, 5)
            };

            GUI.contentColor = Color.white;
            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;

            Instance.CheckGeneralOptions();
            Instance.CheckMenuItemInclusion();
            Instance.CheckAssemblyInclusion();
            Instance.CheckNameSpaceInclusion();
            Instance.CheckSearchOptions();

            Instance.CheckPerformanceOptions();

            if (GUI.changed)
            {
                SavePrefs();
                EditorUtility.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }
        }
    }


#else
    [PreferenceItem("Monkey\nCommander")]

    public static void PreferencesGUI()
    {
        Instance.titleStyle = new GUIStyle()
        {
            richText = true,
            alignment = TextAnchor.MiddleCenter,
            margin = new RectOffset(0, 0, 5, 5)
        };

        GUI.contentColor = Color.white;
        GUI.color = Color.white;
        GUI.backgroundColor = Color.white;

        Instance.CheckGeneralOptions();
        Instance.CheckMenuItemInclusion();
        Instance.CheckAssemblyInclusion();
        Instance.CheckNameSpaceInclusion();
        Instance.CheckSearchOptions();

        Instance.CheckPerformanceOptions();

        if (GUI.changed)
        {
            SavePrefs();
            EditorUtility.SetDirty(Instance);
            AssetDatabase.SaveAssets();
        }

    }
#endif

    private void CheckPerformanceOptions()
    {
        GUILayout.Label("Performances".Bold(), titleStyle);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Use Sorted Selection");
        UseSortedSelection = EditorGUILayout.Toggle(UseSortedSelection);
        EditorGUILayout.EndHorizontal();

        if (!UseSortedSelection)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Show Warning In Log On Sort Sensitive Command");
            ShowSortedSelectionWarning = EditorGUILayout.Toggle(ShowSortedSelectionWarning);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Maximum Sorted Objects (to avoid Editor Freeze)");
            MaxSortedSelectionSize = EditorGUILayout.IntField(MaxSortedSelectionSize);
            EditorGUILayout.EndHorizontal();
        }

    }

    private void CheckGeneralOptions()
    {
        GUILayout.Label("Preferences".Bold(), titleStyle);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Custom Monkey Console Toggle HotKey");
        EditorGUILayout.LabelField("Warning: Only single keys are supported");
        MonkeyConsoleOverrideHotKey = EditorGUILayout.TextArea(MonkeyConsoleOverrideHotKey);
        EditorGUILayout.EndVertical();

        if (!MonkeyConsoleOverrideHotKey.IsNullOrEmpty())
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Using a custom hotkey will make the default hotkeys not work anymore:" +
                            " make sure you chose a convenient key!");
            EditorGUILayout.EndVertical();

        }



        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(MonKeyLocManager.CurrentLoc.PauseOnUsage);
        PauseGameOnConsoleOpen = EditorGUILayout.Toggle(PauseGameOnConsoleOpen);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Force Focus In Dock Mode");
        ForceFocusOnDocked = EditorGUILayout.Toggle(ForceFocusOnDocked);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Show Command Help Only On Selected Command");
        ShowHelpOnSelectedOnly = EditorGUILayout.Toggle(ShowHelpOnSelectedOnly);
        EditorGUILayout.EndHorizontal();
    }

    private void CheckAssemblyInclusion()
    {
        GUILayout.Label("Excluded Assemblies");
        ExcludedAssemblies = GUILayout.TextArea(ExcludedAssemblies);
    }


    private void CheckNameSpaceInclusion()
    {
        GUILayout.Label("Excluded Namespaces");
        ExcludedNameSpaces = GUILayout.TextArea(ExcludedNameSpaces);

    }


    private void CheckSearchOptions()
    {
        GUILayout.Label("Search Options".Bold(), titleStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(MonKeyLocManager.CurrentLoc.PutInvalidAtEnd);
        PutInvalidCommandAtEndOfSearch = EditorGUILayout.
            Toggle(PutInvalidCommandAtEndOfSearch);
        EditorGUILayout.EndHorizontal();
    }

    private void CheckMenuItemInclusion()
    {

        GUILayout.Label("Command Search Inclusion".Bold(), titleStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(MonKeyLocManager.CurrentLoc.IncludeMenuItems);
        IncludeMenuItems = EditorGUILayout.Toggle(IncludeMenuItems);
        EditorGUILayout.EndHorizontal();

        if (IncludeMenuItems)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(MonKeyLocManager.CurrentLoc.OnlyMenuItemsWithHotKeys);
            IncludeOnlyMenuItemsWithHotKeys = EditorGUILayout.Toggle(IncludeOnlyMenuItemsWithHotKeys);
            EditorGUILayout.EndHorizontal();
        }

    }


    private static void SavePrefs()
    {
        MonKeyInternalSettings internalSettings = MonKeyInternalSettings.Instance;

        if (!internalSettings)
            return;

        internalSettings.UseSortedSelection = instance.UseSortedSelection;
        internalSettings.MaxSortedSelectionSize = instance.MaxSortedSelectionSize;
        internalSettings.ShowSortedSelectionWarning = instance.ShowSortedSelectionWarning;
        internalSettings.MonkeyConsoleOverrideHotKey = instance.MonkeyConsoleOverrideHotKey;
        internalSettings.PauseGameOnConsoleOpen = instance.PauseGameOnConsoleOpen;
        internalSettings.PutInvalidCommandAtEndOfSearch = instance.PutInvalidCommandAtEndOfSearch;
        internalSettings.IncludeMenuItems = instance.IncludeMenuItems;
        internalSettings.IncludeOnlyMenuItemsWithHotKeys = instance.IncludeOnlyMenuItemsWithHotKeys;
        internalSettings.ExcludedAssemblies = instance.ExcludedAssemblies;
        internalSettings.ExcludedNameSpaces = instance.ExcludedNameSpaces;
        internalSettings.ForceFocusOnDocked = instance.ForceFocusOnDocked;
        internalSettings.ShowHelpOnlyOnActiveCommand = instance.ShowHelpOnSelectedOnly;

        internalSettings.PostSave();

    }

    [HideInInspector]
    public bool UseSortedSelection = true;
    [HideInInspector]
    public int MaxSortedSelectionSize = 1000;
    [HideInInspector]
    public bool ShowSortedSelectionWarning = true;

    [HideInInspector]
    public string MonkeyConsoleOverrideHotKey = "";

    public bool UseCustomConsoleKey
    {
        get
        {
            return !MonkeyConsoleOverrideHotKey.IsNullOrEmpty();
        }
    }

    [HideInInspector]
    public bool PauseGameOnConsoleOpen = true;

    [HideInInspector]
    public bool PutInvalidCommandAtEndOfSearch = false;

    [HideInInspector]
    public bool IncludeMenuItems = true;
    [HideInInspector]
    public bool IncludeOnlyMenuItemsWithHotKeys = false;

    [HideInInspector]
    public string ExcludedAssemblies = "";
    [HideInInspector]
    public string ExcludedNameSpaces = "";

    [HideInInspector]
    public bool ForceFocusOnDocked = false;

    [HideInInspector]
    public bool ShowHelpOnSelectedOnly=false;

    private GUIStyle titleStyle;
}