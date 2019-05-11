#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using System.Linq;
    using UnityEngine;

    // 
    // This is the base-class for all items. It contains a lot of layout using various layout group attributes. 
    // We've also defines a few relevant groups in constant variables, which derived classes can utilize.
    // 
    // Also note that each item deriving from this class, needs to specify which Item types are
    // supported via the SupporteItemTypes property. This is then referenced in ValueDropdown attribute  
    // on the Type field, so that when users only can specify supported item-types.  
    // 

    public abstract class Item : ScriptableObject
    {
        protected const string LEFT_VERTICAL_GROUP             = "Split/Left";
        protected const string STATS_BOX_GROUP                 = "Split/Left/Stats";
        protected const string GENERAL_SETTINGS_VERTICAL_GROUP = "Split/Left/General Settings/Split/Right";

        [HideLabel, PreviewField(55)]
        [VerticalGroup(LEFT_VERTICAL_GROUP)]
        [HorizontalGroup(LEFT_VERTICAL_GROUP + "/General Settings/Split", 55, LabelWidth = 67)]
        public Texture Icon;

        [BoxGroup(LEFT_VERTICAL_GROUP + "/General Settings")]
        [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
        public string Name;

        [BoxGroup("Split/Right/Description")]
        [HideLabel, TextArea(4, 14)]
        public string Description;

        [HorizontalGroup("Split", 0.5f, MarginLeft = 5, LabelWidth = 130)]
        [BoxGroup("Split/Right/Notes")]
        [HideLabel, TextArea(4, 9)]
        public string Notes;

        [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
        [ValueDropdown("SupportedItemTypes")]
        [ValidateInput("IsSupportedType")]
        public ItemTypes Type;

        [VerticalGroup("Split/Right")]
        public StatList Requirements;

        [AssetsOnly]
        [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
        public GameObject Prefab;

        [BoxGroup(STATS_BOX_GROUP)]
        public int ItemStackSize = 1;

        [BoxGroup(STATS_BOX_GROUP)]
        public float ItemRarity;

        public abstract ItemTypes[] SupportedItemTypes { get; }

        private bool IsSupportedType(ItemTypes type)
        {
            return this.SupportedItemTypes.Contains(type);
        }
    }
}
#endif
