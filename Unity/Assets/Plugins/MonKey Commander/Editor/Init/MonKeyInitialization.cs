using System.IO;
using MonKey.Editor.Internal;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class MonKeyInitialization : MonoBehaviour
{
    public static readonly string MonKeyVersion = "1.4";

    private static string defaultDLLInstallPath = "Assets/Plugins/" +
                                               "MonKey Commander/Editor/Bin/";

    private static string[] supportedVersions = new[] {"2018_3","2018_2", "2018", "2017", "5_4" };

    /// <summary>
    /// used in case you change the version of Unity, 
    /// so that you can access all the functionalities without problems
    /// </summary>
    public static void InitMonKey()
    {
        EnsureGoodDll();
        MonKeySettings.InitSettings();
        CommandManager.Instance.RetrieveAllCommands();
    }

    [DidReloadScripts]
    public static void InitAndShowStartupPanel()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        InitMonKey();
        GettingStartedPanel.OpenPanelFirstTime();
    }

    [InitializeOnLoadMethod]
    public static void OpenPanel()
    {
        InitAndShowStartupPanel();
    }

    private static void EnsureGoodDll()
    {
        Object dll = null;
        string currentVersion;

#if UNITY_2018_3_OR_NEWER
       currentVersion = "2018_3";
#elif UNITY_2018_2_OR_NEWER
       currentVersion = "2018_2";
#elif UNITY_2018_1_OR_NEWER
             currentVersion = "2018";
#elif UNITY_2017_1_OR_NEWER
       currentVersion = "2017";
#elif UNITY_5_3_OR_NEWER || UNITY_5_3
        currentVersion = "5_4";
#endif
        dll = AssetDatabase.LoadAssetAtPath<Object>(defaultDLLInstallPath +
                                                    currentVersion + "/MonKey Commander." + currentVersion);

        if (dll != null)
        {
            ResetDlls();

            if (File.Exists(defaultDLLInstallPath + currentVersion + "/MonKey Commander.dll"))
                File.Delete(defaultDLLInstallPath + currentVersion + "/MonKey Commander.dll");

            File.Move(defaultDLLInstallPath +
                      currentVersion + "/MonKey Commander." + currentVersion,
                defaultDLLInstallPath + currentVersion + "/MonKey Commander.dll");
            File.Delete(defaultDLLInstallPath +
                        currentVersion + "/MonKey Commander." + currentVersion+".meta");

            AssetDatabase.Refresh();
        }

    }

    public static void ResetDlls()
    {
        foreach (var version in supportedVersions)
        {
            Object dll = AssetDatabase.LoadAssetAtPath<Object>(defaultDLLInstallPath
                                        + version + "/MonKey Commander." + version);
            if (!dll)
            {
                var missingDll = version;

                Object oldDll = AssetDatabase.LoadAssetAtPath<Object>(
                    defaultDLLInstallPath + missingDll + "/MonKey Commander.dll");

                if (oldDll)
                {
                    if (File.Exists(defaultDLLInstallPath + missingDll + "/MonKey Commander." + missingDll))
                    {
                        File.Delete(defaultDLLInstallPath + missingDll + "/MonKey Commander.dll");
                        File.Delete(defaultDLLInstallPath + missingDll + "/MonKey Commander.dll.meta");

                    }
                    else
                    {
                        File.Move(defaultDLLInstallPath + missingDll + "/MonKey Commander.dll",
                            defaultDLLInstallPath + missingDll + "/MonKey Commander." + missingDll);
                        File.Delete(defaultDLLInstallPath + missingDll + "/MonKey Commander.dll.meta");
                    }

                    AssetDatabase.Refresh();
                }
            }
        }
    }

}
