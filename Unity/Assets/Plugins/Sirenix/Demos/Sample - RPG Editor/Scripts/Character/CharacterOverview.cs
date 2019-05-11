#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using Sirenix.Utilities;
    using System.Linq;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    // 
    // This is a scriptable object containing a list of all characters available
    // in the Unity project. When a character is added from the RPG editor, the
    // list then gets automatically updated via UpdateCharacterOverview. 
    //
    // If you inspect the Character Overview in the inspector, you will also notice, that
    // the list is not directly modifiable. Instead, we've customized it so it contains a 
    // refresh button, that scans the project and automatically populates the list.
    //
    // CharacterOverview inherits from GlobalConfig which is just a scriptable 
    // object singleton, used by Odin Inspector for configuration files etc, 
    // but it could easily just be a simple scriptable object instead.
    // 

    [GlobalConfig("Plugins/Sirenix/Demos/RPG Editor/Characters", UseAsset = true)]
    public class CharacterOverview : GlobalConfig<CharacterOverview> 
    {
        [ReadOnly]
        [ListDrawerSettings(Expanded = true)]
        public Character[] AllCharacters;

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateCharacterOverview()
        {
            // Finds and assigns all scriptable objects of type Character
            this.AllCharacters = AssetDatabase.FindAssets("t:Character")
                .Select(guid => AssetDatabase.LoadAssetAtPath<Character>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
#endif
    }
}
#endif
