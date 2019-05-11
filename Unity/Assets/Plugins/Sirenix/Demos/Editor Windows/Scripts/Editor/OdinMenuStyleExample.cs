#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using Sirenix.OdinInspector.Editor;
    using UnityEngine;
    using UnityEditor;
    using System.Linq;
    using Sirenix.Utilities;
    using System.Collections.Generic;
    using Sirenix.Utilities.Editor;

    public class OdinMenuStyleExample : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Odin Inspector/Demos/Odin Editor Window Demos/Odin Menu Style Example")]
        private static void OpenWindow()
        {
            var window = GetWindow<OdinMenuStyleExample>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);

            var customMenuStyle = new OdinMenuStyle
            {
                BorderPadding = 0f,
                AlignTriangleLeft = true,
                TriangleSize = 16f,
                TrianglePadding = 0f,
                Offset = 20f,
                Height = 23,
                IconPadding = 0f,
                BorderAlpha = 0.323f
            };

            tree.DefaultMenuStyle = customMenuStyle;

            tree.Config.DrawSearchToolbar = true;

            // Adds the custom menu style to the tree, so that you can play around with it.
            // Once you are happy, you can press Copy C# Snippet copy its settings and paste it in code.
            // And remove the "Menu Style" menu item from the tree.
            tree.AddObjectAtPath("Menu Style", customMenuStyle);

            for (int i = 0; i < 5; i++)
            {
                var customObject = new SomeCustomClass() { Name = i.ToString() };
                var customMenuItem = new MyCustomMenuItem(tree, customObject);
                tree.AddMenuItemAtPath("Custom Menu Items", customMenuItem);
            }

            tree.AddAllAssetsAtPath("Scriptable Objects in Plugins Tree", "Plugins", typeof(ScriptableObject), true, false);
            tree.AddAllAssetsAtPath("Scriptable Objects in Plugins Flat", "Plugins", typeof(ScriptableObject), true, true);
            tree.AddAllAssetsAtPath("Only Configs has Icons", "Plugins/Sirenix", true, false);

            tree.EnumerateTree()
                .AddThumbnailIcons()
                .SortMenuItemsByName();

            return tree;
        }

        //// The editor window itself can also be customized.
        //protected override void OnEnable()
        //{
        //    base.OnEnable();

        //    this.MenuWidth = 200;
        //    this.ResizableMenuWidth = true;
        //    this.WindowPadding = new Vector4(10, 10, 10, 10);
        //    this.DrawUnityEditorPreview = true;
        //    this.DefaultEditorPreviewHeight = 20;
        //    this.UseScrollView = true;
        //}

        private class MyCustomMenuItem : OdinMenuItem
        {
            private readonly SomeCustomClass instance;

            public MyCustomMenuItem(OdinMenuTree tree, SomeCustomClass instance) : base(tree, instance.Name, instance)
            {
                this.instance = instance;
            }

            protected override void OnDrawMenuItem(Rect rect, Rect labelRect)
            {
                labelRect.x -= 16;
                this.instance.Enabled = GUI.Toggle(labelRect.AlignMiddle(18).AlignLeft(16), this.instance.Enabled, GUIContent.none);

                // Toggle selection when pressing space.
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
                {
                    var selection = this.MenuTree.Selection
                        .Select(x => x.Value)
                        .OfType<SomeCustomClass>();

                    if (selection.Any())
                    {
                        var enabled = !selection.FirstOrDefault().Enabled;
                        selection.ForEach(x => x.Enabled = enabled);
                        Event.current.Use();
                    }
                }
            }

            public override string SmartName { get { return this.instance.Name; } }
        }

        private class SomeCustomClass
        {
            public bool Enabled = true;
            public string Name;
        }
    }
}
#endif
