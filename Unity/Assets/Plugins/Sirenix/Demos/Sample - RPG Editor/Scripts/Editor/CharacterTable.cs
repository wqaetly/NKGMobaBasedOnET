#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Serialization;

    //
    // This class is used by the RPGEditorWindow to render an overview of all characters using the TableList attribute.
    // All characters are Unity objects though, so they are rendered in the inspector as single Unity object field,
    // which is not exactly what we want in our table. We want to show the members of the unity object.
    //
    // So in order to render the members of the Unity object, we'll create a class that wraps the Unity object
    // and displays the relevant members through properties, which works with the TableList, attribute.
    //

    public class CharacterTable
    {
        [FormerlySerializedAs("allCharecters")]
        [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
        private readonly List<CharacterWrapper> allCharacters;

        public Character this[int index]
        {
            get { return this.allCharacters[index].Character; }
        }

        public CharacterTable(IEnumerable<Character> characters)
        {
            this.allCharacters = characters.Select(x => new CharacterWrapper(x)).ToList();
        }

        private class CharacterWrapper
        {
            private Character character; // Character is a ScriptableObject and would render a unity object
                                         // field if drawn in the inspector, which is not what we want.

            public Character Character
            {
                get { return this.character; }
            }

            public CharacterWrapper(Character character)
            {
                this.character = character;
            }

            [TableColumnWidth(50, false)]
            [ShowInInspector, PreviewField(45, ObjectFieldAlignment.Center)]
            public Texture Icon { get { return this.character.Icon; } set { this.character.Icon = value; EditorUtility.SetDirty(this.character); } }

            [TableColumnWidth(120)]
            [ShowInInspector]
            public string Name { get { return this.character.Name; } set { this.character.Name = value; EditorUtility.SetDirty(this.character); } }

            [ShowInInspector, ProgressBar(0, 100)]
            public float Shooting { get { return this.character.Skills.Shooting; } set { this.character.Skills.Shooting = value; EditorUtility.SetDirty(this.character); } }

            [ShowInInspector, ProgressBar(0, 100)]
            public float Melee { get { return this.character.Skills.Melee; } set { this.character.Skills.Melee = value; EditorUtility.SetDirty(this.character); } }

            [ShowInInspector, ProgressBar(0, 100)]
            public float Social { get { return this.character.Skills.Social; } set { this.character.Skills.Social = value; EditorUtility.SetDirty(this.character); } }

            [ShowInInspector, ProgressBar(0, 100)]
            public float Animals { get { return this.character.Skills.Animals; } set { this.character.Skills.Animals = value; EditorUtility.SetDirty(this.character); } }

            [ShowInInspector, ProgressBar(0, 100)]
            public float Medicine { get { return this.character.Skills.Medicine; } set { this.character.Skills.Medicine = value; EditorUtility.SetDirty(this.character); } }

            [ShowInInspector, ProgressBar(0, 100)]
            public float Crafting { get { return this.character.Skills.Crafting; } set { this.character.Skills.Crafting = value; EditorUtility.SetDirty(this.character); } }
        }
    }
}
#endif
