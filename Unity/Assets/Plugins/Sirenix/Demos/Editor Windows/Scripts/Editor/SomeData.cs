#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEditor;
    using System;

    [HideLabel]
    [Serializable]
    public class SomeData
    {
        [MultiLineProperty(3), Title("Basic Odin Menu Editor Window", "Inherit from OdinMenuEditorWindow, and build your menu tree")]
        public string Test1 = "This value is persistent cross reloads, but will reset once you restart Unity or close the window.";

        [MultiLineProperty(3), ShowInInspector, NonSerialized]
        public string Test2 = "This value is not persistent cross reloads, and will reset once you hit play or recompile.";

        [MultiLineProperty(3), ShowInInspector]
        private string Test3
        {
            get
            {
                return EditorPrefs.GetString("OdinDemo.PersistentString",
                    "This value is persistent forever, even cross Unity projects. But it's not saved together " +
                    "with your project. That's where ScriptableObejcts and OdinEditorWindows come in handy.");
            }
            set
            {
                EditorPrefs.SetString("OdinDemo.PersistentString", value);
            }
        }
    }
}
#endif
