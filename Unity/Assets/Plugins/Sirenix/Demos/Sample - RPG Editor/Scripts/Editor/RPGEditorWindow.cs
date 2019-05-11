#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using UnityEditor;
    using UnityEngine;
    using System.Linq;

    // 
    // This is the main RPG editor that which exposes everything included in this sample project.
    // 
    // This editor window lets users edit and create characters and items. To achieve this, we inherit from OdinMenuEditorWindow 
    // which quickly lets us add menu items for various objects. Each of these objects are then customized with Odin attributes to make
    // the editor user friendly. 
    // 
    // In order to let the user create items and characters, we don't actually make use of the [CreateAssetMenu] attribute 
    // for any of our scriptable objects, instead we've made a custom ScriptableObjectCreator, which we make use of in the 
    // in the custom toolbar drawn in OnBeginDrawEditors method below.
    // 
    // Go on an adventure in various classes to see how things are achived.
    // 

    public class RPGEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Odin Inspector/Demos/RPG Editor")]
        private static void Open()
        {
            var window = GetWindow<RPGEditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            // Adds the character overview table.
            CharacterOverview.Instance.UpdateCharacterOverview();
            tree.Add("Characters", new CharacterTable(CharacterOverview.Instance.AllCharacters));

            // Adds all cahracters.
            tree.AddAllAssetsAtPath("Characters", "Assets/Plugins/Sirenix", typeof(Character), true, true);

            // Add all scriptable object items.
            tree.AddAllAssetsAtPath("", "Assets/Plugins/Sirenix/Demos/SAMPLE - RPG Editor/Items", typeof(Item), true)
                .ForEach(this.AddDragHandles);

            // Add drag handles to items, so they can be easily dragged into the inventory if characters etc...
            tree.EnumerateTree().Where(x => x.Value as Item).ForEach(AddDragHandles);

            // Add icons to characters and items.
            tree.EnumerateTree().AddIcons<Character>(x => x.Icon);
            tree.EnumerateTree().AddIcons<Item>(x => x.Icon);

            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
                {
                    ScriptableObjectCreator.ShowDialog<Item>("Assets/Plugins/Sirenix/Demos/Sample - RPG Editor/Items", obj =>
                    {
                        obj.Name = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Character")))
                {
                    ScriptableObjectCreator.ShowDialog<Character>("Assets/Plugins/Sirenix/Demos/Sample - RPG Editor/Character", obj =>
                    {
                        obj.Name = obj.name;
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}
#endif
