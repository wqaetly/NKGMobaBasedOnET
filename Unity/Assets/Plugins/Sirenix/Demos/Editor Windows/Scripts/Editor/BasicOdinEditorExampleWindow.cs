#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEditor;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities.Editor;
    using Sirenix.Utilities;

    public class BasicOdinEditorExampleWindow : OdinEditorWindow
    {
        [MenuItem("Tools/Odin Inspector/Demos/Odin Editor Window Demos/Basic Odin Editor Window")]
        private static void OpenWindow()
        {
            var window = GetWindow<BasicOdinEditorExampleWindow>();

            // Nifty little trick to quickly position the window in the middle of the editor.
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
        }

        [EnumToggleButtons]
        [InfoBox("Inherit from OdinEditorWindow instead of EditorWindow in order to create editor windows like you would inspectors - by exposing members and using attributes.")]
        public ViewTool SomeField;
    }
}
#endif
