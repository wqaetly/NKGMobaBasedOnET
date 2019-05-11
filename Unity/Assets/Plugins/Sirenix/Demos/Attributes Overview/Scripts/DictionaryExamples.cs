#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using System.Collections.Generic;

    public class DictionaryExamples : SerializedMonoBehaviour
    {
        [InfoBox("In order to serialize dictionaries, all we need to do is to inherit our class from SerializedMonoBehaviour.")]
        public Dictionary<int, Material> IntMaterialLookup;

        public Dictionary<string, string> StringStringDictionary;

        [DictionaryDrawerSettings(KeyLabel = "Custom Key Name", ValueLabel = "Custom Value Label")]
        public Dictionary<SomeEnum, MyCustomType> CustomLabels;

        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        public Dictionary<string, List<int>> StringListDictionary;

        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public Dictionary<SomeEnum, MyCustomType> EnumObjectLookup;

        [InlineProperty(LabelWidth = 90)]
        public struct MyCustomType
        {
            public int SomeMember;
            public GameObject SomePrefab;
        }

        public enum SomeEnum
        {
            First, Second, Third, Fourth, AndSoOn
        }
    }
}
#endif
