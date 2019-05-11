#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    // 
    // With our custom RPG Editor window, this ScriptableObjectCreator is a replacement for the [CreateAssetMenu] attribute Unity provides.
    // 
    // For instance, if we call ScriptableObjectCreator.ShowDialog<Item>(..., ...), it will automatically find all 
    // ScriptableObjects in your project that inherits from Item and prompts the user with a popup where he 
    // can choose the type of item he wants to create. We then also provide the ShowDialog with a default path,
    // to help the user create it in a specific directory.
    // 

    public static class ScriptableObjectCreator
    {
        public static void ShowDialog<T>(string defaultDestinationPath, Action<T> onScritpableObjectCreated = null)
            where T : ScriptableObject
        {
            var selector = new ScriptableObjectSelector<T>(defaultDestinationPath, onScritpableObjectCreated);

            if (selector.SelectionTree.EnumerateTree().Count() == 1)
            {
                // If there is only one scriptable object to choose from in the selector, then 
                // we'll automatically select it and confirm the selection. 
                selector.SelectionTree.EnumerateTree().First().Select();
                selector.SelectionTree.Selection.ConfirmSelection();
            }
            else
            {
                // Else, we'll open up the selector in a popup and let the user choose.
                selector.ShowInPopup(200);
            }
        }

        // Here is the actual ScriptableObjectSelector which inherits from OdinSelector.
        // You can learn more about those in the documentation: http://sirenix.net/odininspector/documentation/sirenix/odininspector/editor/odinselector(t)
        // This one builds a menu-tree of all types that inherit from T, and when the selection is confirmed, it then prompts the user
        // with a dialog to save the newly created scriptable object.

        private class ScriptableObjectSelector<T> : OdinSelector<Type> where T : ScriptableObject
        {
            private Action<T> onScritpableObjectCreated;
            private string defaultDestinationPath;

            public ScriptableObjectSelector(string defaultDestinationPath, Action<T> onScritpableObjectCreated = null)
            {
                this.onScritpableObjectCreated = onScritpableObjectCreated;
                this.defaultDestinationPath = defaultDestinationPath;
                this.SelectionConfirmed += this.ShowSaveFileDialog;
            }

            protected override void BuildSelectionTree(OdinMenuTree tree)
            {
                var scriptableObjectTypes = AssemblyUtilities.GetTypes(AssemblyTypeFlags.CustomTypes)
                    .Where(x => x.IsClass && !x.IsAbstract && x.InheritsFrom(typeof(T)));

                tree.Selection.SupportsMultiSelect = false;
                tree.Config.DrawSearchToolbar = true;
                tree.AddRange(scriptableObjectTypes, x => x.GetNiceName())
                    .AddThumbnailIcons();
            }

            private void ShowSaveFileDialog(IEnumerable<Type> selection)
            {
                var obj = ScriptableObject.CreateInstance(selection.FirstOrDefault()) as T;

                string dest = this.defaultDestinationPath.TrimEnd('/');

                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                    AssetDatabase.Refresh();
                }

                dest = EditorUtility.SaveFilePanel("Save object as", dest, "New " + typeof(T).GetNiceName(), "asset");

                if (!string.IsNullOrEmpty(dest) && PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest))
                {
                    AssetDatabase.CreateAsset(obj, dest);
                    AssetDatabase.Refresh();

                    if (this.onScritpableObjectCreated != null)
                    {
                        this.onScritpableObjectCreated(obj);
                    }
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(obj);
                }
            }
        }
    }
}
#endif
