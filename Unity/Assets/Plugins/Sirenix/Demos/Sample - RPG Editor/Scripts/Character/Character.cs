#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using UnityEngine;

    //
    // Instead of adding [CreateAssetMenu] attribute, we've created a Scriptable Object Creator using Odin Selectors.
    // Characters can then be easily created in the RPG Editor window, which also helps ensure that they get located in the right folder.
    //
    // By inheriting from SerializedScriptableObject, we can then also utilize the extra serialization power Odin brings.
    // In this case, Odin serializes the Inventory which is a two-dimensional array. Everything else is serialized by Unity.
    // 

    public class Character : SerializedScriptableObject
    {
        [HorizontalGroup("Split", 55, LabelWidth = 70)]
        [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
        public Texture Icon;

        [VerticalGroup("Split/Meta")]
        public string Name;

        [VerticalGroup("Split/Meta")]
        public string Surname;

        [VerticalGroup("Split/Meta"), Range(0, 100)]
        public int Age;

        [HorizontalGroup("Split", 290), EnumToggleButtons, HideLabel]
        public CharacterAlignment CharacterAlignment;

        [TabGroup("Starting Inventory")]
        public ItemSlot[,] Inventory = new ItemSlot[12, 6];

        [TabGroup("Starting Stats"), HideLabel]
        public CharacterStats Skills = new CharacterStats();

        [HideLabel]
        [TabGroup("Starting Equipment")]
        public CharacterEquipment StartingEquipment;
    }
}
#endif
